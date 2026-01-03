using FluentAssertions;
using Person.Application.Contracts.Requests;
using Person.Application.Validators;
using Person.Domain.Enums;

namespace Person.Application.UnitTests.Validators;

public class CreatePersonRequestValidatorTests
{
    private readonly CreatePersonRequestValidator _validator;

    public CreatePersonRequestValidatorTests()
    {
        _validator = new CreatePersonRequestValidator();
    }

    [Fact]
    public void Validate_ValidRequest_ReturnsNoErrors()
    {
        // Arrange
        var request = new CreatePersonRequest
        {
            Surname = "Smith",
            FirstName = "John",
            Patronymic = "Michael",
            Email = "john.smith@example.com",
            Phone = "+12025551234",
            DateBirth = new DateTime(1990, 1, 1),
            Gender = Gender.Male,
            Comment = "Test comment"
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
        var request = new CreatePersonRequest
        {
            Surname = surname!,
            FirstName = "John",
            Email = "john.smith@example.com",
            Phone = "+12025551234",
            DateBirth = new DateTime(1990, 1, 1),
            Gender = Gender.Male
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Surname");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_InvalidFirstName_ReturnsError(string? firstName)
    {
        // Arrange
        var request = new CreatePersonRequest
        {
            Surname = "Smith",
            FirstName = firstName!,
            Email = "john.smith@example.com",
            Phone = "+12025551234",
            DateBirth = new DateTime(1990, 1, 1),
            Gender = Gender.Male
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FirstName");
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@example.com")]
    [InlineData("user@")]
    [InlineData("")]
    public void Validate_InvalidEmail_ReturnsError(string email)
    {
        // Arrange
        var request = new CreatePersonRequest
        {
            Surname = "Smith",
            FirstName = "John",
            Email = email,
            Phone = "+12025551234",
            DateBirth = new DateTime(1990, 1, 1),
            Gender = Gender.Male
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Theory]
    [InlineData("123")]
    [InlineData("invalid-phone")]
    [InlineData("")]
    public void Validate_InvalidPhone_ReturnsError(string phone)
    {
        // Arrange
        var request = new CreatePersonRequest
        {
            Surname = "Smith",
            FirstName = "John",
            Email = "john.smith@example.com",
            Phone = phone,
            DateBirth = new DateTime(1990, 1, 1),
            Gender = Gender.Male
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Phone");
    }

    [Fact]
    public void Validate_FutureDateBirth_ReturnsError()
    {
        // Arrange
        var request = new CreatePersonRequest
        {
            Surname = "Smith",
            FirstName = "John",
            Email = "john.smith@example.com",
            Phone = "+12025551234",
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