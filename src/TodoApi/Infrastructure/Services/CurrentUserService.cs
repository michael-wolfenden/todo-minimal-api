namespace TodoApi.Infrastructure.Services;

public interface ICurrentUserService
{
    string User { get; }
}

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string User => _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "Anonymous";
}
