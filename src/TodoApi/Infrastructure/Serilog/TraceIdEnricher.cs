using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace TodoApi.Infrastructure.Serilog;

public class TraceIdEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TraceIdEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory factory)
    {
        var traceId = Activity.Current?.Id ?? _httpContextAccessor.HttpContext?.TraceIdentifier;

        logEvent.AddPropertyIfAbsent(factory.CreateProperty(nameof(Activity.TraceId), traceId));
    }
}
