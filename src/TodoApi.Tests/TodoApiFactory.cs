using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TodoApi.Configure;
using WebMotions.Fake.Authentication.JwtBearer;

namespace TodoApi.Tests;

public class TodoApiFactory : WebApplicationFactory<Program>
{
    private readonly string _sqlServerConnectionString;

    public TodoApiFactory(string sqlServerConnectionString)
    {
        _sqlServerConnectionString = sqlServerConnectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseEnvironment("Test");

        builder.ConfigureTestServices(services =>
        {
            services.AddCustomPersistence(_sqlServerConnectionString);

            // Allows customizing user / roles via a SetFakeBearerToken extension
            // method on the the HttpClient
            services.AddAuthentication(FakeJwtBearerDefaults.AuthenticationScheme).AddFakeJwtBearer();
        });
    }
}