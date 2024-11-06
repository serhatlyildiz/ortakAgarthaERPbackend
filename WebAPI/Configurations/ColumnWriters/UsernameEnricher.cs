using Serilog.Core;
using Serilog.Events;

public class UsernameEnricher : ILogEventEnricher
{
    private readonly string _username;

    public UsernameEnricher(string username)
    {
        _username = username;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("user_name", _username));
    }
}
