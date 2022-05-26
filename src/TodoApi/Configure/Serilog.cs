using TodoApi.Infrastructure.Serilog;

namespace TodoApi.Configure;

public static class Serilog
{
    public static WebApplicationBuilder AddCustomSerilog(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<UserEnricher>();
        builder.Services.AddTransient<TraceIdEnricher>();

        builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
            .Enrich.FromLogContext()
            .Enrich.WithProperty(nameof(builder.Environment.EnvironmentName), builder.Environment.EnvironmentName)
            .Enrich.WithProperty(nameof(builder.Environment.ApplicationName), builder.Environment.ApplicationName)
            .Enrich.With(
                services.GetRequiredService<UserEnricher>(),
                services.GetRequiredService<TraceIdEnricher>())
            .WriteTo.Console()
            .ReadFrom.Configuration(context.Configuration));

        return builder;
    }

    public static WebApplication UseCustomSerilog(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        return app;
    }
}
