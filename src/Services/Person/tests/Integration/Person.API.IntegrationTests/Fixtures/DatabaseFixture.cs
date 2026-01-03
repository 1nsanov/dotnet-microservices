using Microsoft.EntityFrameworkCore;
using Npgsql;
using Person.Domain.Enums;
using Person.Infrastructure.Data;
using Respawn;
using Testcontainers.PostgreSql;

namespace Person.API.IntegrationTests.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;
    private Respawner _respawner = null!;
    private NpgsqlConnection _connection = null!;

    public string ConnectionString { get; private set; } = string.Empty;

    public DatabaseFixture()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:17-alpine")
            .WithDatabase("PersonTestDb")
            .WithUsername("testuser")
            .WithPassword("testpass")
            .WithPortBinding(5433, 5432)
            .WithCleanUp(true)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        ConnectionString = _container.GetConnectionString();

        await InitializeDatabaseSchema();
        await InitializeRespawner();
    }

    private async Task InitializeDatabaseSchema()
    {
        var options = new DbContextOptionsBuilder<PersonDbContext>()
            .UseNpgsql(ConnectionString, o => o.MapEnum<Gender>("gender"))
            .Options;

        await using var context = new PersonDbContext(options);

        await context.Database.MigrateAsync();
    }

    private async Task InitializeRespawner()
    {
        _connection = new NpgsqlConnection(ConnectionString);
        await _connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
            TablesToIgnore = ["__EFMigrationsHistory"]
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_connection);
    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
        await _container.DisposeAsync();
    }
}