using TodoApi.Configure;
using TodoApi.Todos.Endpoints;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args)
        .AddCustomSerilog()
        .AddCustomDependencies()
        .AddCustomPersistence()
        .AddCustomExceptionHandling()
        .AddCustomSwagger()
        .AddCustomAuthnAndAuthz();

    var application = builder.Build()
        .UseCustomSerilog()
        .UseCustomExceptionHandling()
        .UseCustomSwagger()
        .UseCustomAuthnAndAuthz()
        .MapTodoEndpoints();

    await application.EnsureDbCreated();

    application.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "An exception was thrown during startup");

    return 1;
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

// needed to allow WebApplicationFactory<Program> in tests
public partial class Program { }
