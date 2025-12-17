using Microsoft.EntityFrameworkCore;

namespace Person.Infrastructure.Data;

public class PersonDbContext : DbContext
{
    public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Person> Persons { get; set; } = null!;
    public DbSet<Domain.Entities.WorkExperience> WorkExperiences { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersonDbContext).Assembly);
    }
}