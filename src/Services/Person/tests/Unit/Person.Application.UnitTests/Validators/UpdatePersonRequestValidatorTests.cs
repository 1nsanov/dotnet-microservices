using FluentAssertions;
using Person.Application.Contracts.Requests;
using Person.Application.Validators;
using Person.Domain.Enums;

namespace Person.Application.UnitTests.Validators;

public class UpdatePersonRequestValidatorTests
{
    private readonly UpdatePersonRequestValidator _validator;

    public UpdatePersonRequestValidatorTests()
    {
        _validator = new UpdatePersonRequestValidator();
    }

    [Fact]
    public void Validate_ValidRequest_ReturnsNoErrors()
    {
        // Arrange
        var request = new UpdatePersonRequest
        {
            Surname = "Johnson",
            FirstName = "Peter",
            Patronymic = "David",
            Email = "peter.johnson@example.com",
            Phone = "+12025559876",
            DateBirth = new DateTime(1985, 5, 15),
            Gender = Gender.Male,
            Comment = "Updated comment"
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_InvalidSurname_ReturnsError(string? surname)
    {
        // Arrange
        var request = new UpdatePersonRequest
        {
            Surname = surname!,
            FirstName = "Peter",
            Email = "peter.johnson@example.com",
            Phone = "+12025559876",
            DateBirth = new DateTime(1985, 5, 15),
            Gender = Gender.Male
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Surname");
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@example.com")]
    [InlineData("user@")]
    public void Validate_InvalidEmail_ReturnsError(string email)
    {
        // Arrange
        var request = new UpdatePersonRequest
        {
            Surname = "Johnson",
            FirstName = "Peter",
            Email = email,
            Phone = "+12025559876",
            DateBirth = new DateTime(1985, 5, 15),
            Gender = Gender.Male
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Validate_FutureDateBirth_ReturnsError()
    {
        // Arrange
        var request = new UpdatePersonRequest
        {
            Surname = "Johnson",
            FirstName = "Peter",
            Email = "peter.johnson@example.com",
            Phone = "+12025559876",
            DateBirth = DateTime.UtcNow.AddDays(1),
            Gender = Gender.Male
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DateBirth");
    }
}