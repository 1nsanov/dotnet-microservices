using Person.Domain.Common;
using Person.Domain.Enums;
using Person.Domain.Exceptions;
using Person.Domain.ValueObjects;

namespace Person.Domain.Entities;

public class Person : EntityBase, IAggregateRoot
{
    private const int MinAge = 0;
    private const int MaxAge = 150;
    private const int MaxCommentLength = 1000;

    private readonly List<WorkExperience> _workExperiences = new();
    private IReadOnlyCollection<WorkExperience>? _workExperiencesReadOnly;

    private Person()
    {
    }

    private Person(FullName fullName, Email email, Phone phone, DateTime dateBirth, Gender gender, string? comment)
    {
        Id = Guid.NewGuid();
        SetCreatedDate();

        FullName = fullName ?? throw new InvalidEntityException(nameof(Person), "FullName cannot be null");
        Email = email ?? throw new InvalidEntityException(nameof(Person), "Email cannot be null");
        Phone = phone ?? throw new InvalidEntityException(nameof(Person), "Phone cannot be null");
        DateBirth = ValidateDateBirth(dateBirth);
        Gender = ValidateGender(gender);
        Comment = ValidateComment(comment);
    }

    public FullName FullName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Phone Phone { get; private set; } = null!;
    public DateTime DateBirth { get; private set; }
    public Gender Gender { get; private set; }
    public string? Comment { get; private set; }
    public IReadOnlyCollection<WorkExperience> WorkExperiences => 
        _workExperiencesReadOnly ??= _workExperiences.AsReadOnly();

    public int Age => CalculateAge(DateBirth);

    public static Person Create(FullName fullName, Email email, Phone phone, DateTime dateBirth, Gender gender,
        string? comment = null)
    {
        return new Person(fullName, email, phone, dateBirth, gender, comment);
    }

    public void UpdatePersonalInfo(FullName fullName, Phone phone, Gender gender, string? comment)
    {
        FullName = fullName ?? throw new InvalidEntityException(nameof(Person), "FullName cannot be null");
        Phone = phone ?? throw new InvalidEntityException(nameof(Person), "Phone cannot be null");
        Gender = ValidateGender(gender);
        Comment = ValidateComment(comment);
        SetLastModifiedDate();
    }

    public void UpdateEmail(Email email)
    {
        Email = email ?? throw new InvalidEntityException(nameof(Person), "Email cannot be null");
        SetLastModifiedDate();
    }

    public void AddWorkExperience(string position, string organization, Address address, string description,
        DateTime dateEmployment, DateTime? dateTermination = null)
    {
        var workExperience =
            WorkExperience.Create(position, organization, address, description, dateEmployment, dateTermination);
        _workExperiences.Add(workExperience);
        _workExperiencesReadOnly = null;
        SetLastModifiedDate();
    }

    public void RemoveWorkExperience(Guid workExperienceId)
    {
        var workExperience = _workExperiences.FirstOrDefault(we => we.Id == workExperienceId);
        if (workExperience == null)
            throw new InvalidEntityException(nameof(Person), $"WorkExperience with Id {workExperienceId} not found");

        _workExperiences.Remove(workExperience);
        _workExperiencesReadOnly = null;
        SetLastModifiedDate();
    }

    public void UpdateWorkExperience(Guid workExperienceId, string position, string organization, Address address,
        string description, DateTime dateEmployment, DateTime? dateTermination = null)
    {
        var workExperience = _workExperiences.FirstOrDefault(we => we.Id == workExperienceId);
        if (workExperience == null)
            throw new InvalidEntityException(nameof(Person), $"WorkExperience with Id {workExperienceId} not found");

        workExperience.Update(position, organization, address, description, dateEmployment, dateTermination);
        _workExperiencesReadOnly = null;
        SetLastModifiedDate();
    }

    private static DateTime ValidateDateBirth(DateTime dateBirth)
    {
        var age = CalculateAge(dateBirth);

        if (age < MinAge)
            throw new InvalidEntityException(nameof(Person), "date of birth cannot be in the future");

        if (age > MaxAge)
            throw new InvalidEntityException(nameof(Person), $"age cannot exceed {MaxAge} years");

        return dateBirth;
    }

    private static Gender ValidateGender(Gender gender)
    {
        if (!Enum.IsDefined(typeof(Gender), gender))
            throw new InvalidEntityException(nameof(Person), "invalid gender value");

        if (gender == Gender.None)
            throw new InvalidEntityException(nameof(Person), "gender cannot be None");

        return gender;
    }

    private static string? ValidateComment(string? comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
            return null;

        var trimmedComment = comment.Trim();

        if (trimmedComment.Length > MaxCommentLength)
            throw new InvalidEntityException(nameof(Person), $"comment cannot exceed {MaxCommentLength} characters");

        return trimmedComment;
    }

    private static int CalculateAge(DateTime dateBirth)
    {
        var today = DateTime.UtcNow.Date;
        var age = today.Year - dateBirth.Year;

        if (dateBirth.Date > today.AddYears(-age))
            age--;

        return age;
    }
}