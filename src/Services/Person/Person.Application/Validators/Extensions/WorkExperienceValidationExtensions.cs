using FluentValidation;
using Person.Application.Validators.Common;

namespace Person.Application.Validators.Extensions;

public static class WorkExperienceValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ValidatePosition<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.WorkExperience.PositionRequired)
            .Length(ValidationConstants.WorkExperience.MinFieldLength,
                ValidationConstants.WorkExperience.MaxPositionLength)
            .WithMessage(string.Format(ValidationMessages.WorkExperience.PositionLengthRangeFormat,
                ValidationConstants.WorkExperience.MinFieldLength,
                ValidationConstants.WorkExperience.MaxPositionLength));
    }

    public static IRuleBuilderOptions<T, string> ValidateOrganization<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.WorkExperience.OrganizationRequired)
            .Length(ValidationConstants.WorkExperience.MinFieldLength,
                ValidationConstants.WorkExperience.MaxOrganizationLength)
            .WithMessage(string.Format(ValidationMessages.WorkExperience.OrganizationLengthRangeFormat,
                ValidationConstants.WorkExperience.MinFieldLength,
                ValidationConstants.WorkExperience.MaxOrganizationLength));
    }

    public static IRuleBuilderOptions<T, string> ValidateDescription<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.WorkExperience.DescriptionRequired)
            .Length(ValidationConstants.WorkExperience.MinFieldLength,
                ValidationConstants.WorkExperience.MaxDescriptionLength)
            .WithMessage(string.Format(ValidationMessages.WorkExperience.DescriptionLengthRangeFormat,
                ValidationConstants.WorkExperience.MinFieldLength,
                ValidationConstants.WorkExperience.MaxDescriptionLength));
    }

    public static IRuleBuilderOptions<T, DateTime> ValidateDateEmployment<T>(this IRuleBuilder<T, DateTime> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage(ValidationMessages.WorkExperience.DateEmploymentRequired)
            .Must(date => date <= DateTime.UtcNow)
            .WithMessage(ValidationMessages.WorkExperience.DateEmploymentCannotBeInFuture);
    }

    public static void ValidateDateTermination<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
    {
        ruleBuilder
            .Must(date => !date.HasValue || date.Value <= DateTime.UtcNow)
            .WithMessage(ValidationMessages.WorkExperience.DateTerminationCannotBeInFuture);

        ruleBuilder
            .Must((instance, dateTermination) =>
            {
                if (!dateTermination.HasValue)
                    return true;

                var dateEmploymentProperty = typeof(T).GetProperty("DateEmployment");
                if (dateEmploymentProperty == null)
                    return true;

                var dateEmployment = (DateTime?)dateEmploymentProperty.GetValue(instance);
                return !dateEmployment.HasValue || dateTermination.Value >= dateEmployment.Value;
            })
            .WithMessage(ValidationMessages.WorkExperience.DateTerminationMustBeAfterEmployment);
    }
}