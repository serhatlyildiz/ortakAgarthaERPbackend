using Autofac;
using Autofac.Extensions.DependencyInjection;
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
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Autofac ayarlarý
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
builder.Services.AddSingleton<ILogEventEnricher, UsernameEnricher>(); // Enricher'ý da burada ekliyoruz
builder.Services.AddSingleton<ILogEventEnricher, EmailEnricher>(); // Enricher'ý da burada ekliyoruz
builder.Services.AddSingleton<ILogEventEnricher, IpAddressEnricher>(); // Enricher'ý da burada ekliyoruz
builder.Services.AddScoped<IPasswordResetDal, EfPasswordResetDal>();
builder.Services.AddScoped<ProductImageManager>();

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
            NameClaimType = ClaimTypes.Name //JWT üzerinde name claimne karþýlýk gelen deðeri User.Identity.Name propertysinden elde edebiliriz.
        };
    });

builder.Services.AddDependencyRevolvers(new ICoreModule[]
{
    new CoreModule()
});

var columnOptions = new ColumnOptions();
columnOptions.Store.Clear();
columnOptions.Store.Add(StandardColumn.Id);
columnOptions.Store.Add(StandardColumn.Message);
columnOptions.Store.Add(StandardColumn.Level);
columnOptions.Store.Add(StandardColumn.TimeStamp);
columnOptions.Store.Add(StandardColumn.Exception);
columnOptions.Store.Add(StandardColumn.LogEvent);

// MSSQL Server için user_name adlý bir kolon ekliyoruz
columnOptions.AdditionalColumns = new Collection<SqlColumn>
{
    new SqlColumn { ColumnName = "user_name", DataType = SqlDbType.NVarChar, DataLength = 100 },
    new SqlColumn { ColumnName = "email", DataType = SqlDbType.NVarChar, DataLength = 100 },
    new SqlColumn { ColumnName = "machine_name", DataType = SqlDbType.NVarChar, DataLength = 100 },
    new SqlColumn { ColumnName = "ip_address", DataType = SqlDbType.NVarChar, DataLength = 45 }, // IP adresi için uygun uzunluk
    new SqlColumn { ColumnName = "request_id", DataType = SqlDbType.NVarChar, DataLength = 50 } // Request ID için sütun
};

// Logger yapýlandýrmasý
var log = new LoggerConfiguration()
    .Enrich.With(new UsernameEnricher(builder.Services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>())) // Enricher ile kullanýcý adýný ekliyoruz
    .Enrich.With(new EmailEnricher(builder.Services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>())) // Dinamik e-posta ekliyoruz
    .Enrich.With(new IpAddressEnricher(builder.Services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>())) // Dinamik IP adresini ekliyoruz
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
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. " +
        "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// HTTP isteði boru hattýný yapýlandýrýn
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRouting();
app.UseCors(builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader());
app.UseHttpsRedirection();

app.UseHttpLogging();
app.UseSerilogRequestLogging(); // buradan sonrakileri yani aþaðýda kalanlarý logla

app.UseAuthentication();
app.UseAuthorization();


app.Use(async (context, next) =>
{
    // Kimlik doðrulamasý yapýlmýþ kullanýcý adý
    var username = context.User?.Identity?.IsAuthenticated == true
        ? context.User.Identity.Name
        : "Anonymous";  // Kimlik doðrulamasý yapýlmamýþsa 'Anonymous' yazdýrýlýr

    // Kullanýcýnýn e-posta bilgisi
    var email = context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "Email Not Found";

    // Bilgisayar adý
    var machineName = Environment.MachineName;

    // IP adresi (IHttpContextAccessor kullanarak alýnabilir)
    var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "IP Not Found";

    var requestId = Guid.NewGuid().ToString();
    context.Items["request_id"] = requestId;
    context.Response.Headers.Add("X-Request-ID", requestId);

    // LogContext'e property'leri ekliyoruz
    LogContext.PushProperty("user_name", username);
    LogContext.PushProperty("email", email);
    LogContext.PushProperty("machine_name", machineName);
    LogContext.PushProperty("ip_address", ipAddress);
    LogContext.PushProperty("request_id", requestId);

    // Sonraki middleware'e geçiþ yapýyoruz
    await next();
});

app.MapControllers();

app.Run();