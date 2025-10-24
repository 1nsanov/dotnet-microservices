using System.Text.RegularExpressions;
using Person.Domain.Common;
using Person.Domain.Exceptions;

namespace Person.Domain.ValueObjects;

public sealed class Phone : ValueObjectBase
{
    private const int MinLength = 10;
    private const int MaxLength = 15;

    private static readonly Regex PhoneRegex = new(
        @"^\+?[0-9\s\-\(\)]+$",
        RegexOptions.Compiled);

    public Phone(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidValueObjectException(nameof(Phone), "value cannot be empty");

        var digitsOnly = Regex.Replace(value, @"[^\d]", "");

        if (digitsOnly.Length < MinLength)
            throw new InvalidValueObjectException(nameof(Phone),
                $"phone number must contain at least {MinLength} digits");

        if (digitsOnly.Length > MaxLength)
            throw new InvalidValueObjectException(nameof(Phone),
                $"phone number cannot contain more than {MaxLength} digits");

        if (!PhoneRegex.IsMatch(value))
            throw new InvalidValueObjectException(nameof(Phone), "invalid phone format");

        Value = value.Trim();
    }

    public string Value { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}