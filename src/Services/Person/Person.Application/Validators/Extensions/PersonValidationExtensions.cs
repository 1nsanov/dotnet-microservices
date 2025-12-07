using FluentValidation;
using Person.Application.Validators.Common;
using Person.Domain.Enums;

namespace Person.Application.Validators.Extensions;

public static class PersonValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ValidateSurname<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.Person.SurnameRequired)
            .Length(ValidationConstants.Name.MinLength, ValidationConstants.Name.MaxLength)
            .WithMessage(string.Format(ValidationMessages.Person.NameLengthRangeFormat, 
                ValidationConstants.Name.MinLength, ValidationConstants.Name.MaxLength))
            .Matches(ValidationRegex.Name).WithMessage(ValidationMessages.Person.NameInvalidCharacters);
    }

    public static IRuleBuilderOptions<T, string> ValidateFirstName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.Person.FirstNameRequired)
            .Length(ValidationConstants.Name.MinLength, ValidationConstants.Name.MaxLength)
            .WithMessage(string.Format(ValidationMessages.Person.NameLengthRangeFormat, 
                ValidationConstants.Name.MinLength, ValidationConstants.Name.MaxLength))
            .Matches(ValidationRegex.Name).WithMessage(ValidationMessages.Person.NameInvalidCharacters);
    }

    public static void ValidatePatronymic<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        ruleBuilder
            .Length(ValidationConstants.Name.MinLength, ValidationConstants.Name.MaxLength)
            .When(x => !string.IsNullOrWhiteSpace(GetPatronymic(x)))
            .WithMessage(string.Format(ValidationMessages.Person.NameLengthRangeFormat, 
                ValidationConstants.Name.MinLength, ValidationConstants.Name.MaxLength));
        
        ruleBuilder
            .Matches(ValidationRegex.Name)
            .When(x => !string.IsNullOrWhiteSpace(GetPatronymic(x)))
            .WithMessage(ValidationMessages.Person.NameInvalidCharacters);
    }

    private static string? GetPatronymic<T>(T instance)
    {
        var property = typeof(T).GetProperty("Patronymic");
        return property?.GetValue(instance) as string;
    }

    public static IRuleBuilderOptions<T, string> ValidatePhone<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.Person.PhoneRequired)
            .Matches(ValidationRegex.Phone).WithMessage(ValidationMessages.Person.PhoneInvalidFormat)
            .Must(HaveValidPhoneDigitCount)
            .WithMessage(string.Format(ValidationMessages.Person.PhoneDigitsRangeFormat, 
                ValidationConstants.Phone.MinDigits, ValidationConstants.Phone.MaxDigits));
    }

    public static IRuleBuilderOptions<T, string> ValidateEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.Person.EmailRequired)
            .EmailAddress().WithMessage(ValidationMessages.Person.EmailInvalidFormat)
            .MaximumLength(ValidationConstants.Email.MaxLength)
            .WithMessage(string.Format(ValidationMessages.Person.EmailMaxLengthFormat, 
                ValidationConstants.Email.MaxLength));
    }

    public static IRuleBuilderOptions<T, Gender> ValidateGender<T>(this IRuleBuilder<T, Gender> ruleBuilder)
    {
        return ruleBuilder
            .IsInEnum().WithMessage(ValidationMessages.Person.InvalidGenderValue)
            .NotEqual(Gender.None).WithMessage(ValidationMessages.Person.GenderRequired);
    }

    public static void ValidateComment<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        ruleBuilder
            .MaximumLength(ValidationConstants.Person.MaxCommentLength)
            .When(x => !string.IsNullOrWhiteSpace(GetComment(x)))
            .WithMessage(string.Format(ValidationMessages.Person.CommentMaxLengthFormat, 
                ValidationConstants.Person.MaxCommentLength));
    }

    private static string? GetComment<T>(T instance)
    {
        var property = typeof(T).GetProperty("Comment");
        return property?.GetValue(instance) as string;
    }

    public static IRuleBuilderOptions<T, DateTime> ValidateDateBirth<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.Person.DateBirthRequired)
            .Must(BeValidDateBirth)
            .WithMessage(string.Format(ValidationMessages.Person.DateBirthAgeRangeFormat, 
                ValidationConstants.Person.MinAge, ValidationConstants.Person.MaxAge));
    }

    private static bool HaveValidPhoneDigitCount(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        var digitsOnly = ValidationRegex.DigitsOnly.Replace(phone, "");
        return digitsOnly.Length >= ValidationConstants.Phone.MinDigits && 
               digitsOnly.Length <= ValidationConstants.Phone.MaxDigits;
    }

    private static bool BeValidDateBirth(DateTime dateBirth)
    {
        var today = DateTime.UtcNow.Date;
        var age = today.Year - dateBirth.Year;

        if (dateBirth.Date > today.AddYears(-age))
            age--;

        return age >= ValidationConstants.Person.MinAge && age <= ValidationConstants.Person.MaxAge;
    }

}

