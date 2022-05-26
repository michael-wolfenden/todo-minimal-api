using TodoApi.Infrastructure.Exceptions;
using TodoApi.Infrastructure.Services;
using ValidationException = TodoApi.Infrastructure.Exceptions.ValidationException;

namespace TodoApi.Infrastructure.Mediatr;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (Exception ex) when (ex is not NotFoundException and not ValidationException)
        {
            // CreateTodo.Request => CreateTodo
            var requestType = typeof(TRequest).DeclaringType?.Name;

            var userId = _currentUserService.User;

            _logger.LogError(ex, "An unhandled exception has occurred for request type {RequestType} [{@Request}]",
                requestType, request);

            throw;
        }
    }
}
