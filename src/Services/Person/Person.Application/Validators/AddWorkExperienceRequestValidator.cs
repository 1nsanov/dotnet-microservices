using FluentValidation;
using Person.Application.Contracts.Requests;
using Person.Application.Validators.Extensions;

namespace Person.Application.Validators;

public class AddWorkExperienceRequestValidator : AbstractValidator<AddWorkExperienceRequest>
{
    public AddWorkExperienceRequestValidator()
    {
        RuleFor(x => x.Position).ValidatePosition();
        RuleFor(x => x.Organization).ValidateOrganization();
        RuleFor(x => x.Description).ValidateDescription();

        RuleFor(x => x.CountryCode).ValidateCountryCode();
        RuleFor(x => x.City).ValidateCity();
        RuleFor(x => x.Street).ValidateStreet();
        RuleFor(x => x.HouseNumber).ValidateHouseNumber();
        RuleFor(x => x.PostalCode).ValidatePostalCode();
        RuleFor(x => x.Apartment).ValidateApartment();

        RuleFor(x => x.DateEmployment).ValidateDateEmployment();
        RuleFor(x => x.DateTermination).ValidateDateTermination();
    }
}