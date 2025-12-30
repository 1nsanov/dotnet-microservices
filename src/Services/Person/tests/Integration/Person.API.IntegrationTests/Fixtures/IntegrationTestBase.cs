using Refit;

namespace Person.API.IntegrationTests.Fixtures;

[Collection("Integration Tests")]
public abstract class IntegrationTestBase : IAsyncLifetime
{
    private readonly DatabaseFixture _databaseFixture;
    private readonly HttpClient _httpClient;
    protected readonly IServiceProvider ServiceProvider;

    protected IntegrationTestBase(IntegrationTestFixture fixture)
    {
        var factory = fixture.Factory;
        _databaseFixture = fixture.DatabaseFixture;
        _httpClient = factory.CreateClient();
        ServiceProvider = factory.Services;
    }

    protected TRefitClient CreateRefitClient<TRefitClient>()
    {
        return RestService.For<TRefitClient>(_httpClient);
    }

    public virtual Task InitializeAsync() => Task.CompletedTask;

    public virtual async Task DisposeAsync()
    {
        await _databaseFixture.ResetDatabaseAsync();
    }
}