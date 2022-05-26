using TodoApi.Infrastructure.Mediatr;
using TodoApi.Infrastructure.Services;

namespace TodoApi.Configure;

public static class Dependencies
{
    public static WebApplicationBuilder AddCustomDependencies(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddMediatR(typeof(Program));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        builder.Services.AddValidatorsFromAssemblyContaining<Program>();

        return builder;
    }
}
