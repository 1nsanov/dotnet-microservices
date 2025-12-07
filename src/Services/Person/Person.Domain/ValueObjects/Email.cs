using System.Text.RegularExpressions;
using BuildingBlocks.Domain.Common;
using BuildingBlocks.Domain.Exceptions;

namespace Person.Domain.ValueObjects;

public sealed class Email : ValueObjectBase
{
    private const int MaxLength = 254;
    private const int MaxLocalPartLength = 64;
    private const int MaxDomainPartLength = 255;
    
    private static readonly Regex EmailRegex = new(
        @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidValueObjectException(nameof(Email), "value cannot be empty");

        var trimmedValue = value.Trim();

        if (trimmedValue.Length > MaxLength)
            throw new InvalidValueObjectException(nameof(Email), $"length cannot exceed {MaxLength} characters");

        var parts = trimmedValue.Split('@');
        if (parts.Length != 2)
            throw new InvalidValueObjectException(nameof(Email), "invalid email format");

        if (parts[0].Length > MaxLocalPartLength)
            throw new InvalidValueObjectException(nameof(Email), 
                $"local part cannot exceed {MaxLocalPartLength} characters");

        if (parts[1].Length > MaxDomainPartLength)
            throw new InvalidValueObjectException(nameof(Email), 
                $"domain part cannot exceed {MaxDomainPartLength} characters");

        if (!EmailRegex.IsMatch(trimmedValue))
            throw new InvalidValueObjectException(nameof(Email), "invalid email format");

        Value = trimmedValue.ToLowerInvariant();
    }

    public string Value { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}