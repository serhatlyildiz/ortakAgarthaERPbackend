using Serilog.Core;
using Serilog.Events;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class UsernameEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Parametresiz yapıcı ekledik, ancak IHttpContextAccessor injection yapacağız
    public UsernameEnricher() { }

    // IHttpContextAccessor'ı dışarıdan alıyoruz
    public UsernameEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var user = _httpContextAccessor?.HttpContext?.User;

        if (user == null || !user.Identity.IsAuthenticated)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("user_name", "Anonymous"));
            return;
        }

        // Kullanıcının ismini adresini alıyoruz
        var name = user.FindFirst(ClaimTypes.Name)?.Value;

        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("user_name", name ?? "Unknown"));
    }
}
