using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Extensions;

namespace TodoApi.Tests.Infrastructure;

public class SqlSeverContainer : IDisposable
{
    private const string Password = "My!P@ssw0rd1";

    private readonly IContainerService _containerService = new Builder()
        .UseContainer()
        .UseImage("mcr.microsoft.com/mssql/server")
        .WithEnvironment($"SA_PASSWORD={Password}", "ACCEPT_EULA=Y")
        .ExposePort(1433)
        .WaitForPort("1433/tcp", TimeSpan.FromSeconds(30).TotalMilliseconds)
        .Build();

    public string ConnectionString
    {
        get
        {
            var endpoint = _containerService.ToHostExposedEndpoint("1433/tcp");
            return $"Data Source=host.docker.internal,{endpoint.Port};Initial Catalog=TodoDB;User ID=sa;Password={Password};TrustServerCertificate=True";
        }
    }

    public void Dispose()
    {
        try
        {
            _containerService.Dispose();
        }
        catch
        {
            // Ignore
        }
    }

    public void Start()
    {
        try
        {
            _containerService.Start();
        }
        catch
        {
            _containerService.Dispose();
            throw;
        }
    }
}
