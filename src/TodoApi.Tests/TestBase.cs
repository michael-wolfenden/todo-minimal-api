using System.Net;
using Respawn;
using Respawn.Graph;

namespace TodoApi.Tests;

// Because all the tests are in the same test collection they will
// be run serially so we don't have to worry about parallel processes
// mutating the database
[Collection(nameof(TextFixture))]
[UsesVerify]
public abstract class TestBase : IAsyncLifetime
{
    private readonly Checkpoint _checkpoint = new()
    {
        TablesToIgnore = new Table[]
        {
            "__EFMigrationsHistory"
        },
        WithReseed = true
    };

    private readonly string _sqlSeverConnectionString;

    protected TestBase(TextFixture textFixture)
    {
        _sqlSeverConnectionString = textFixture.SqlSeverContainer.ConnectionString;
    }

    // This will run ONCE BEFORE each test
    public async Task InitializeAsync()
    {
        await ClearAllDataInAllTablesAndReseed();
    }

    // This will run ONCE AFTER each test
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    protected HttpClient AuthorizedApi(string username = "Test User", params string[] roles)
    {
        return UnauthorizedApi().SetFakeBearerToken(username, roles);
    }

    protected HttpClient UnauthorizedApi()
    {
        return new TodoApiFactory(_sqlSeverConnectionString).CreateClient();
    }

    private async Task ClearAllDataInAllTablesAndReseed()
    {
        await _checkpoint.Reset(_sqlSeverConnectionString);
    }
}