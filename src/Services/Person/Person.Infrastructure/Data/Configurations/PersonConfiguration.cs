using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Person.Domain.Enums;
using Person.Domain.ValueObjects;

namespace Person.Infrastructure.Data.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Domain.Entities.Person>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Person> builder)
    {
        builder.ToTable("Persons");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.OwnsOne(p => p.FullName, fullName =>
        {
            fullName.Property(fn => fn.FirstName)
                .HasColumnName(nameof(FullName.FirstName))
                .HasMaxLength(100)
                .IsRequired();

            fullName.Property(fn => fn.Surname)
                .HasColumnName(nameof(FullName.Surname))
                .HasMaxLength(100)
                .IsRequired();

            fullName.Property(fn => fn.Patronymic)
                .HasColumnName(nameof(FullName.Patronymic))
                .HasMaxLength(100)
                .IsRequired(false);
        });

        builder.OwnsOne(p => p.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName(nameof(Domain.Entities.Person.Email))
                .HasMaxLength(254)
                .IsRequired();

            email.HasIndex(e => e.Value)
                .IsUnique();
        });

        builder.OwnsOne(p => p.Phone, phone =>
        {
            phone.Property(ph => ph.Value)
                .HasColumnName(nameof(Domain.Entities.Person.Phone))
                .HasMaxLength(20)
                .IsRequired();
        });

        builder.Property(p => p.DateBirth)
            .IsRequired();

        builder.Property(p => p.Gender)
            .HasDefaultValue(Gender.None)
            .IsRequired();

        builder.Property(p => p.Comment)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(p => p.CreatedDate)
            .IsRequired();

        builder.Property(p => p.LastModifiedDate)
            .IsRequired(false);

        builder.HasMany(p => p.WorkExperiences)
            .WithOne()
            .HasForeignKey("PersonId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(p => p.Age);

        builder.Navigation(p => p.WorkExperiences)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .AutoInclude();
    }
}