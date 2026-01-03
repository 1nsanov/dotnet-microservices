namespace Person.API.IntegrationTests.Fixtures;

public class IntegrationTestFixture : IAsyncLifetime
{
    public DatabaseFixture DatabaseFixture { get; }
    public PersonApiFactory Factory { get; private set; } = null!;

    public IntegrationTestFixture()
    {
        DatabaseFixture = new DatabaseFixture();
    }

    public async Task InitializeAsync()
    {
        await DatabaseFixture.InitializeAsync();
        Factory = new PersonApiFactory(DatabaseFixture);
        await Factory.InitializeAsync();
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();
        await DatabaseFixture.DisposeAsync();
    }
}

[CollectionDefinition("Integration Tests")]
public class IntegrationTestCollection : ICollectionFixture<IntegrationTestFixture>
{
}