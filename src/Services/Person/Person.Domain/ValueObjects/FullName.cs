using System.Text.RegularExpressions;
using BuildingBlocks.Domain.Common;
using BuildingBlocks.Domain.Exceptions;

namespace Person.Domain.ValueObjects;

public sealed class FullName : ValueObjectBase
{
    private const int MinLength = 1;
    private const int MaxLength = 100;

    private static readonly Regex NameRegex = new(
        @"^[\p{L}\s\-']+$",
        RegexOptions.Compiled);

    public FullName(string surname, string firstName, string? patronymic = null)
    {
        Surname = ValidateAndFormat(surname, nameof(Surname));
        FirstName = ValidateAndFormat(firstName, nameof(FirstName));
        Patronymic = ValidateAndFormatOptional(patronymic, nameof(Patronymic));
    }

    public string Surname { get; }
    public string FirstName { get; }
    public string? Patronymic { get; }

    private static string ValidateAndFormat(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidValueObjectException(nameof(FullName), $"{fieldName} cannot be empty");

        var trimmedValue = value.Trim();

        if (trimmedValue.Length < MinLength)
            throw new InvalidValueObjectException(nameof(FullName),
                $"{fieldName} must contain at least {MinLength} character");

        if (trimmedValue.Length > MaxLength)
            throw new InvalidValueObjectException(nameof(FullName),
                $"{fieldName} cannot exceed {MaxLength} characters");

        if (!NameRegex.IsMatch(trimmedValue))
            throw new InvalidValueObjectException(nameof(FullName), $"{fieldName} contains invalid characters");

        return trimmedValue;
    }

    private static string? ValidateAndFormatOptional(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var trimmedValue = value.Trim();

        if (trimmedValue.Length < MinLength)
            throw new InvalidValueObjectException(nameof(FullName),
                $"{fieldName} must contain at least {MinLength} character");

        if (trimmedValue.Length > MaxLength)
            throw new InvalidValueObjectException(nameof(FullName),
                $"{fieldName} cannot exceed {MaxLength} characters");

        if (!NameRegex.IsMatch(trimmedValue))
            throw new InvalidValueObjectException(nameof(FullName), $"{fieldName} contains invalid characters");

        return trimmedValue;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Surname;
        yield return FirstName;
        yield return Patronymic;
    }

    public override string ToString()
    {
        return string.IsNullOrWhiteSpace(Patronymic)
            ? $"{Surname} {FirstName}"
            : $"{Surname} {FirstName} {Patronymic}";
    }
}