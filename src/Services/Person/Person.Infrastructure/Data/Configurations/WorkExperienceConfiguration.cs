using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Person.Domain.Entities;
using Person.Domain.ValueObjects;

namespace Person.Infrastructure.Data.Configurations;

public class WorkExperienceConfiguration : IEntityTypeConfiguration<WorkExperience>
{
    public void Configure(EntityTypeBuilder<WorkExperience> builder)
    {
        builder.ToTable("WorkExperiences");

        builder.HasKey(we => we.Id);

        builder.Property(we => we.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property<Guid>("PersonId")
            .IsRequired();

        builder.Property(we => we.Position)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(we => we.Organization)
            .HasMaxLength(200)
            .IsRequired();

        builder.OwnsOne(we => we.Address, address =>
        {
            address.Property(a => a.CountryCode)
                .HasColumnName(nameof(Address.CountryCode))
                .HasMaxLength(3)
                .IsRequired();

            address.Property(a => a.City)
                .HasColumnName(nameof(Address.City))
                .HasMaxLength(100)
                .IsRequired();

            address.Property(a => a.Street)
                .HasColumnName(nameof(Address.Street))
                .HasMaxLength(200)
                .IsRequired();

            address.Property(a => a.HouseNumber)
                .HasColumnName(nameof(Address.HouseNumber))
                .HasMaxLength(20)
                .IsRequired();

            address.Property(a => a.PostalCode)
                .HasColumnName(nameof(Address.PostalCode))
                .HasMaxLength(10)
                .IsRequired(false);

            address.Property(a => a.Apartment)
                .HasColumnName(nameof(Address.Apartment))
                .HasMaxLength(20)
                .IsRequired(false);
        });

        builder.Property(we => we.Description)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(we => we.DateEmployment)
            .IsRequired();

        builder.Property(we => we.DateTermination)
            .IsRequired(false);

        builder.Property(we => we.CreatedDate)
            .IsRequired();

        builder.Property(we => we.LastModifiedDate)
            .IsRequired(false);

        builder.Ignore(we => we.IsCurrentJob);
    }
}