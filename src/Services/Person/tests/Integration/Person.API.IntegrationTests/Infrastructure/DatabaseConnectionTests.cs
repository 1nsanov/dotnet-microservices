using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Person.API.IntegrationTests.Fixtures;
using Person.Infrastructure.Data;
using Xunit.Abstractions;

namespace Person.API.IntegrationTests.Infrastructure;

public class DatabaseConnectionTests : IntegrationTestBase
{
    private readonly ITestOutputHelper _output;

    public DatabaseConnectionTests(IntegrationTestFixture fixture, ITestOutputHelper output) : base(fixture)
    {
        _output = output;
    }

    [Fact]
    public async Task Database_ShouldBeAccessible()
    {
        using var scope = ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PersonDbContext>();

        var connectionString = dbContext.Database.GetConnectionString();
        _output.WriteLine($"Connection string: {connectionString}");

        try
        {
            var canConnect = await dbContext.Database.CanConnectAsync();
            canConnect.Should().BeTrue("database should be accessible from test container");
            _output.WriteLine("Database connection successful");
        }
        catch (Exception ex)
        {
            _output.WriteLine($"Connection failed: {ex.Message}");
            _output.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }

    [Fact]
    public async Task Database_ShouldHaveAppliedMigrations()
    {
        using var scope = ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PersonDbContext>();

        var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

        appliedMigrations.Should().NotBeEmpty("migrations should be applied");
        pendingMigrations.Should().BeEmpty("all migrations should be applied");

        _output.WriteLine($"Applied migrations: {string.Join(", ", appliedMigrations)}");
    }

    [Fact]
    public async Task Database_ShouldBeEmptyInitially()
    {
        using var scope = ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<PersonDbContext>();

        var personCount = await dbContext.Persons.CountAsync();
        var workExperienceCount = await dbContext.WorkExperiences.CountAsync();

        personCount.Should().Be(0, "Persons table should be empty initially");
        workExperienceCount.Should().Be(0, "WorkExperiences table should be empty initially");

        _output.WriteLine("Database is clean and ready for tests");
    }
}