using Serilog.Core;
using Serilog.Events;
using Microsoft.AspNetCore.Http;

public class IpAddressEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    // IHttpContextAccessor'ı constructor'a enjekte ediyoruz
    public IpAddressEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        // Dinamik IP adresini al ve ekle
        string ipAddress = GetClientIpAddress();
        var property = propertyFactory.CreateProperty("ClientIpAddress", ipAddress);
        logEvent.AddPropertyIfAbsent(property);
    }


    private string GetClientIpAddress()
    {
        // HTTP bağlamından IP adresini alıyoruz
        var remoteIpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

        // Proxy veya yük dengeleici arkasında iseniz, X-Forwarded-For başlığından IP alabilirsiniz
        if (string.IsNullOrEmpty(remoteIpAddress))
        {
            remoteIpAddress = _httpContextAccessor.HttpContext?.Request?.Headers["X-Forwarded-For"];
        }

        return remoteIpAddress ?? "IP Address Not Found";
    }
}
