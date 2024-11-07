using Serilog.Core;
using Serilog.Events;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class EmailEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    // IHttpContextAccessor'ı constructor'a enjekte ediyoruz
    public EmailEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        // Dinamik e-posta adresini al ve ekle
        string email = GetUserEmail();
        var property = propertyFactory.CreateProperty("Email", email);
        logEvent.AddPropertyIfAbsent(property);
    }

    private string GetUserEmail()
    {
        var emailClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email);

        return emailClaim?.Value ?? "Email Not Found";
    }
}
