using FluentValidation;
using Person.Application.Contracts.Requests;
using Person.Application.Validators.Extensions;

namespace Person.Application.Validators;

public class UpdatePersonRequestValidator : AbstractValidator<UpdatePersonRequest>
{
    public UpdatePersonRequestValidator()
    {
        RuleFor(x => x.Surname).ValidateSurname();
        RuleFor(x => x.FirstName).ValidateFirstName();
        RuleFor(x => x.Patronymic).ValidatePatronymic();
        RuleFor(x => x.Email).ValidateEmail();
        RuleFor(x => x.Phone).ValidatePhone();
        RuleFor(x => x.DateBirth).ValidateDateBirth();
        RuleFor(x => x.Gender).ValidateGender();
        RuleFor(x => x.Comment).ValidateComment();
    }
}