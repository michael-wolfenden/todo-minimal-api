using TodoApi.Tests.Infrastructure;

namespace TodoApi.Tests;

[CollectionDefinition(nameof(TextFixture))]
public class TextFixture : IDisposable, ICollectionFixture<TextFixture>
{
    public readonly SqlSeverContainer SqlSeverContainer = new();

    // This will run ONCE BEFORE all tests are started
    public TextFixture()
    {
        SqlSeverContainer.Start();

        // HACK: ensures Program is run at least once to bootstrap database
        // or else the respawn checkpoint delete will fail on the first test
        // as the database doesn't exist
        _ = new TodoApiFactory(SqlSeverContainer.ConnectionString).Server;
    }

    // This will run ONCE AFTER all tests are finished
    public void Dispose()
    {
        SqlSeverContainer.Dispose();
    }
}