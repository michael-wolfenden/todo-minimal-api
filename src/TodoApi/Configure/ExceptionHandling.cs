using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TodoApi.Infrastructure.Exceptions;
using ProblemDetailsFactory = Hellang.Middleware.ProblemDetails.ProblemDetailsFactory;
using ValidationException = TodoApi.Infrastructure.Exceptions.ValidationException;

namespace TodoApi.Configure;

public static class ExceptionHandling
{
    public static WebApplicationBuilder AddCustomExceptionHandling(this WebApplicationBuilder builder)
    {
        builder.Services.TryAddSingleton<IActionResultExecutor<ObjectResult>, MinimalApiProblemDetailsResultExecutor>();

        builder.Services.AddProblemDetails(o =>
        {
            o.MapToStatusCode<NotFoundException>(Status404NotFound);

            o.Map<ValidationException>((context, ex) => context
                .RequestServices
                .GetRequiredService<ProblemDetailsFactory>()
                .CreateValidationProblemDetails(context, ex.Errors));
        });

        return builder;
    }

    public static WebApplication UseCustomExceptionHandling(this WebApplication app)
    {
        app.UseProblemDetails();

        return app;
    }

    private class MinimalApiProblemDetailsResultExecutor : IActionResultExecutor<ObjectResult>
    {
        public virtual Task ExecuteAsync(ActionContext context, ObjectResult result) =>
            Json(result.Value, null, "application/problem+json", result.StatusCode)
                .ExecuteAsync(context.HttpContext);
    }
}
