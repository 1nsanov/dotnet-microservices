using Person.Domain.Common;
using Person.Domain.Exceptions;
using Person.Domain.ValueObjects;

namespace Person.Domain.Entities;

public class WorkExperience : EntityBase
{
    private const int MinFieldLength = 1;
    private const int MaxPositionLength = 200;
    private const int MaxOrganizationLength = 200;
    private const int MaxDescriptionLength = 2000;

    private WorkExperience()
    {
    }

    private WorkExperience(string position, string organization, Address address, string description,
        DateTime dateEmployment, DateTime? dateTermination)
    {
        Id = Guid.NewGuid();
        SetCreatedDate();

        Position = ValidateField(position, nameof(Position), MaxPositionLength);
        Organization = ValidateField(organization, nameof(Organization), MaxOrganizationLength);
        Address = address ?? throw new InvalidEntityException(nameof(WorkExperience), "Address cannot be null");
        Description = ValidateField(description, nameof(Description), MaxDescriptionLength);
        DateEmployment = ValidateDateEmployment(dateEmployment);
        DateTermination = ValidateDateTermination(dateEmployment, dateTermination);
    }

    public string Position { get; private set; } = null!;
    public string Organization { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public DateTime DateEmployment { get; private set; }
    public DateTime? DateTermination { get; private set; }

    public bool IsCurrentJob => DateTermination == null;

    internal static WorkExperience Create(string position, string organization, Address address, string description,
        DateTime dateEmployment, DateTime? dateTermination = null)
    {
        return new WorkExperience(position, organization, address, description, dateEmployment, dateTermination);
    }

    internal void Update(string position, string organization, Address address, string description,
        DateTime dateEmployment, DateTime? dateTermination)
    {
        Position = ValidateField(position, nameof(Position), MaxPositionLength);
        Organization = ValidateField(organization, nameof(Organization), MaxOrganizationLength);
        Address = address ?? throw new InvalidEntityException(nameof(WorkExperience), "Address cannot be null");
        Description = ValidateField(description, nameof(Description), MaxDescriptionLength);
        DateEmployment = ValidateDateEmployment(dateEmployment);
        DateTermination = ValidateDateTermination(dateEmployment, dateTermination);
        SetLastModifiedDate();
    }

    private static string ValidateField(string value, string fieldName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidEntityException(nameof(WorkExperience), $"{fieldName} cannot be empty");

        var trimmedValue = value.Trim();

        if (trimmedValue.Length < MinFieldLength)
            throw new InvalidEntityException(nameof(WorkExperience),
                $"{fieldName} must contain at least {MinFieldLength} character");

        if (trimmedValue.Length > maxLength)
            throw new InvalidEntityException(nameof(WorkExperience),
                $"{fieldName} cannot exceed {maxLength} characters");

        return trimmedValue;
    }

    private static DateTime ValidateDateEmployment(DateTime dateEmployment)
    {
        if (dateEmployment > DateTime.UtcNow)
            throw new InvalidEntityException(nameof(WorkExperience), "employment date cannot be in the future");

        return dateEmployment;
    }

    private static DateTime? ValidateDateTermination(DateTime dateEmployment, DateTime? dateTermination)
    {
        if (dateTermination == null)
            return null;

        if (dateTermination.Value < dateEmployment)
            throw new InvalidEntityException(nameof(WorkExperience),
                "termination date cannot be earlier than employment date");

        if (dateTermination.Value > DateTime.UtcNow)
            throw new InvalidEntityException(nameof(WorkExperience), "termination date cannot be in the future");

        return dateTermination.Value;
    }
}