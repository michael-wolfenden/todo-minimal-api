using Serilog.Core;
using Serilog.Events;
using TodoApi.Infrastructure.Services;

namespace TodoApi.Infrastructure.Serilog;

public class UserEnricher : ILogEventEnricher
{
    private readonly ICurrentUserService _currentUserService;

    public UserEnricher(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory factory)
    {
        var userId = _currentUserService.User;

        logEvent.AddPropertyIfAbsent(factory.CreateProperty(nameof(_currentUserService.User), userId));
    }
}
