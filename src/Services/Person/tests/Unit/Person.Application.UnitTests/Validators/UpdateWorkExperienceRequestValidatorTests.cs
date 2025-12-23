using FluentAssertions;
using Person.Application.Contracts.Requests;
using Person.Application.Validators;

namespace Person.Application.UnitTests.Validators;

public class UpdateWorkExperienceRequestValidatorTests
{
    private readonly UpdateWorkExperienceRequestValidator _validator;

    public UpdateWorkExperienceRequestValidatorTests()
    {
        _validator = new UpdateWorkExperienceRequestValidator();
    }

    [Fact]
    public void Validate_ValidRequest_ReturnsNoErrors()
    {
        // Arrange
        var request = new UpdateWorkExperienceRequest
        {
            Position = "Senior Software Engineer",
            Organization = "Innovation Labs",
            CountryCode = "US",
            City = "New York",
            Street = "Broadway",
            HouseNumber = "500",
            PostalCode = "10012",
            Apartment = "Floor 15",
            Description = "Led development team",
            DateEmployment = new DateTime(2018, 6, 1),
            DateTermination = new DateTime(2023, 12, 31)
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
    public void Validate_InvalidPosition_ReturnsError(string? position)
    {
        // Arrange
        var request = new UpdateWorkExperienceRequest
        {
            Position = position!,
            Organization = "Innovation Labs",
            CountryCode = "US",
            City = "New York",
            Street = "Broadway",
            HouseNumber = "500",
            Description = "Test description",
            DateEmployment = new DateTime(2018, 6, 1)
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Position");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_InvalidOrganization_ReturnsError(string? organization)
    {
        // Arrange
        var request = new UpdateWorkExperienceRequest
        {
            Position = "Senior Software Engineer",
            Organization = organization!,
            CountryCode = "US",
            City = "New York",
            Street = "Broadway",
            HouseNumber = "500",
            Description = "Test description",
            DateEmployment = new DateTime(2018, 6, 1)
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Organization");
    }

    [Fact]
    public void Validate_FutureDateEmployment_ReturnsError()
    {
        // Arrange
        var request = new UpdateWorkExperienceRequest
        {
            Position = "Senior Software Engineer",
            Organization = "Innovation Labs",
            CountryCode = "US",
            City = "New York",
            Street = "Broadway",
            HouseNumber = "500",
            Description = "Test description",
            DateEmployment = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DateEmployment");
    }

    [Fact]
    public void Validate_DateTerminationBeforeDateEmployment_ReturnsError()
    {
        // Arrange
        var request = new UpdateWorkExperienceRequest
        {
            Position = "Senior Software Engineer",
            Organization = "Innovation Labs",
            CountryCode = "US",
            City = "New York",
            Street = "Broadway",
            HouseNumber = "500",
            Description = "Test description",
            DateEmployment = new DateTime(2020, 1, 15),
            DateTermination = new DateTime(2019, 1, 15)
        };

        // Act
        var result = _validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "DateTermination");
    }
}