using FluentValidation;
using Person.Application.Validators.Common;

namespace Person.Application.Validators.Extensions;

public static class AddressValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ValidateCountryCode<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.Address.CountryCodeRequired)
            .Matches(ValidationRegex.CountryCode)
            .WithMessage(ValidationMessages.Address.CountryCodeInvalidFormat);
    }

    public static IRuleBuilderOptions<T, string> ValidateCity<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.Address.CityRequired)
            .MaximumLength(ValidationConstants.Address.MaxCityLength)
            .WithMessage(string.Format(ValidationMessages.Address.CityMaxLengthFormat,
                ValidationConstants.Address.MaxCityLength));
    }

    public static IRuleBuilderOptions<T, string> ValidateStreet<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.Address.StreetRequired)
            .MaximumLength(ValidationConstants.Address.MaxStreetLength)
            .WithMessage(string.Format(ValidationMessages.Address.StreetMaxLengthFormat,
                ValidationConstants.Address.MaxStreetLength));
    }

    public static IRuleBuilderOptions<T, string> ValidateHouseNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.Address.HouseNumberRequired)
            .MaximumLength(ValidationConstants.Address.MaxHouseNumberLength)
            .WithMessage(string.Format(ValidationMessages.Address.HouseNumberMaxLengthFormat,
                ValidationConstants.Address.MaxHouseNumberLength));
    }

    public static void ValidatePostalCode<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        ruleBuilder
            .Matches(ValidationRegex.PostalCode)
            .When(x => !string.IsNullOrWhiteSpace(GetPostalCode(x)))
            .WithMessage(ValidationMessages.Address.PostalCodeInvalidFormat);
    }

    public static void ValidateApartment<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        ruleBuilder
            .MaximumLength(ValidationConstants.Address.MaxApartmentLength)
            .When(x => !string.IsNullOrWhiteSpace(GetApartment(x)))
            .WithMessage(string.Format(ValidationMessages.Address.ApartmentMaxLengthFormat,
                ValidationConstants.Address.MaxApartmentLength));
    }

    private static string? GetPostalCode<T>(T instance)
    {
        var property = typeof(T).GetProperty("PostalCode");
        return property?.GetValue(instance) as string;
    }

    private static string? GetApartment<T>(T instance)
    {
        var property = typeof(T).GetProperty("Apartment");
        return property?.GetValue(instance) as string;
    }
}