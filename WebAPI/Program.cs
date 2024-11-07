using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.Abstract;
using Business.Concrete;
using Business.DependencyRevolvers.Autofac;
using Core.DependencyRevolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Mapping;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Autofac ayarlar�
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule(new AutofacBusinessModule());
    });

// Servisleri ekleyin
builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ILogEventEnricher, UsernameEnricher>(); // Enricher'� da burada ekliyoruz
builder.Services.AddSingleton<ILogEventEnricher, EmailEnricher>(); // Enricher'� da burada ekliyoruz
builder.Services.AddSingleton<ILogEventEnricher, IpAddressEnricher>(); // Enricher'� da burada ekliyoruz


var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
            NameClaimType = ClaimTypes.Name //JWT �zerinde name claimne kar��l�k gelen de�eri User.Identity.Name propertysinden elde edebiliriz.
        };
    });

builder.Services.AddDependencyRevolvers(new ICoreModule[]
{
    new CoreModule()
});

var columnOptions = new ColumnOptions();
columnOptions.Store.Clear();
columnOptions.Store.Add(StandardColumn.Message);
columnOptions.Store.Add(StandardColumn.Level);
columnOptions.Store.Add(StandardColumn.TimeStamp);
columnOptions.Store.Add(StandardColumn.Exception);
columnOptions.Store.Add(StandardColumn.LogEvent);

// MSSQL Server i�in user_name adl� bir kolon ekliyoruz
columnOptions.AdditionalColumns = new Collection<SqlColumn>
{
    new SqlColumn { ColumnName = "user_name", DataType = SqlDbType.NVarChar, DataLength = 100 },
    new SqlColumn { ColumnName = "email", DataType = SqlDbType.NVarChar, DataLength = 100 },
    new SqlColumn { ColumnName = "machine_name", DataType = SqlDbType.NVarChar, DataLength = 100 },
    new SqlColumn { ColumnName = "ip_address", DataType = SqlDbType.NVarChar, DataLength = 45 } // IP adresi i�in uygun uzunluk
};

// Logger yap�land�rmas�
var log = new LoggerConfiguration()
    .Enrich.With(new UsernameEnricher(builder.Services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>())) // Enricher ile kullan�c� ad�n� ekliyoruz
    .Enrich.With(new IpAddressEnricher(builder.Services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>())) // Dinamik IP adresini ekliyoruz
    .Enrich.With(new EmailEnricher(builder.Services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>())) // Dinamik e-posta ekliyoruz
    .Enrich.WithMachineName()
    .WriteTo.Seq(builder.Configuration["Seq:ServerURL"])
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.MSSqlServer(
        connectionString: @"Server=(localdb)\MSSQLLocalDB;Database=Northwind;Trusted_Connection=True",
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true
        },
        columnOptions: columnOptions
    )
    .CreateLogger();

builder.Host.UseSerilog(log);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");    
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

// Swagger eklemek
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// HTTP iste�i boru hatt�n� yap�land�r�n
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(); // buradan sonrakileri yani a�a��da kalanlar� logla
app.UseHttpLogging();
app.UseRouting();
app.UseCors(builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader());
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.Use(async (context, next) =>
{
    // Kimlik do�rulamas� yap�lm�� kullan�c� ad�
    var username = context.User?.Identity?.IsAuthenticated == true
        ? context.User.Identity.Name
        : "Anonymous";  // Kimlik do�rulamas� yap�lmam��sa 'Anonymous' yazd�r�l�r

    // Kullan�c�n�n e-posta bilgisi
    var email = context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "Email Not Found";

    // Bilgisayar ad�
    var machineName = Environment.MachineName;

    // IP adresi (IHttpContextAccessor kullanarak al�nabilir)
    //var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "IP Not Found";
    var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "IP Not Found";

    // LogContext'e property'leri ekliyoruz
    LogContext.PushProperty("user_name", username);
    LogContext.PushProperty("email", email);
    LogContext.PushProperty("machine_name", machineName);
    LogContext.PushProperty("ip_address", ipAddress);

    // Sonraki middleware'e ge�i� yap�yoruz
    await next();
});



app.MapControllers();

app.Run();


