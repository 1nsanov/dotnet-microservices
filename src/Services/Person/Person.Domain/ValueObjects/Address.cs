using System.Text.RegularExpressions;
using BuildingBlocks.Domain.Common;
using BuildingBlocks.Domain.Exceptions;

namespace Person.Domain.ValueObjects;

public sealed class Address : ValueObjectBase
{
    private const int MaxCityLength = 100;
    private const int MaxStreetLength = 200;
    private const int MaxHouseNumberLength = 20;
    private const int MaxApartmentLength = 20;
    private static readonly Regex CountryCodeRegex = new(@"^[A-Z]{2,3}$", RegexOptions.Compiled);
    private static readonly Regex PostalCodeRegex = new(@"^[\d\s\-]{3,10}$", RegexOptions.Compiled);

    public Address(string countryCode, string city, string street, string houseNumber, string? postalCode,
        string? apartment)
    {
        CountryCode = ValidateCountryCode(countryCode);
        City = ValidateRequiredField(city, nameof(City), MaxCityLength);
        Street = ValidateRequiredField(street, nameof(Street), MaxStreetLength);
        HouseNumber = ValidateRequiredField(houseNumber, nameof(HouseNumber), MaxHouseNumberLength);
        PostalCode = ValidateOptionalPostalCode(postalCode);
        Apartment = ValidateOptionalField(apartment, nameof(Apartment), MaxApartmentLength);
    }

    public string CountryCode { get; }
    public string City { get; }
    public string Street { get; }
    public string HouseNumber { get; }
    public string? PostalCode { get; }
    public string? Apartment { get; }

    private static string ValidateCountryCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidValueObjectException(nameof(Address), "country code cannot be empty");

        var upperValue = value.Trim().ToUpperInvariant();

        if (!CountryCodeRegex.IsMatch(upperValue))
            throw new InvalidValueObjectException(nameof(Address),
                "country code must be in ISO format (2-3 uppercase letters)");

        return upperValue;
    }

    private static string ValidateRequiredField(string value, string fieldName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidValueObjectException(nameof(Address), $"{fieldName} cannot be empty");

        var trimmedValue = value.Trim();

        if (trimmedValue.Length > maxLength)
            throw new InvalidValueObjectException(nameof(Address), $"{fieldName} cannot exceed {maxLength} characters");

        return trimmedValue;
    }

    private static string? ValidateOptionalField(string? value, string fieldName, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var trimmedValue = value.Trim();

        if (trimmedValue.Length > maxLength)
            throw new InvalidValueObjectException(nameof(Address), $"{fieldName} cannot exceed {maxLength} characters");

        return trimmedValue;
    }

    private static string? ValidateOptionalPostalCode(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var trimmedValue = value.Trim();

        if (!PostalCodeRegex.IsMatch(trimmedValue))
            throw new InvalidValueObjectException(nameof(Address), "invalid postal code format");

        return trimmedValue;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CountryCode;
        yield return City;
        yield return Street;
        yield return HouseNumber;
        yield return PostalCode;
        yield return Apartment;
    }

    public override string ToString()
    {
        var parts = new List<string>
        {
            $"{Street} {HouseNumber}"
        };

        if (!string.IsNullOrWhiteSpace(Apartment))
            parts.Add($"apt {Apartment}");

        parts.Add(City);

        if (!string.IsNullOrWhiteSpace(PostalCode))
            parts.Add(PostalCode);

        parts.Add(CountryCode);

        return string.Join(", ", parts);
    }
}