using FluentAssertions;
using Person.Application.Contracts.Requests;
using Person.Application.Validators;

namespace Person.Application.UnitTests.Validators;

public class AddWorkExperienceRequestValidatorTests
{
    private readonly AddWorkExperienceRequestValidator _validator;

    public AddWorkExperienceRequestValidatorTests()
    {
        _validator = new AddWorkExperienceRequestValidator();
    }

    [Fact]
    public void Validate_ValidRequest_ReturnsNoErrors()
    {
        // Arrange
        var request = new AddWorkExperienceRequest
        {
            Position = "Software Engineer",
            Organization = "Tech Corporation",
            CountryCode = "US",
            City = "San Francisco",
            Street = "Market Street",
            HouseNumber = "100",
            PostalCode = "94105",
            Apartment = "Suite 200",
            Description = "Developed enterprise applications",
            DateEmployment = new DateTime(2020, 1, 15),
            DateTermination = null
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
        var request = new AddWorkExperienceRequest
        {
            Position = position!,
            Organization = "Tech Corporation",
            CountryCode = "US",
            City = "San Francisco",
            Street = "Market Street",
            HouseNumber = "100",
            Description = "Test description",
            DateEmployment = new DateTime(2020, 1, 15)
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
        var request = new AddWorkExperienceRequest
        {
            Position = "Software Engineer",
            Organization = organization!,
            CountryCode = "US",
            City = "San Francisco",
            Street = "Market Street",
            HouseNumber = "100",
            Description = "Test description",
            DateEmployment = new DateTime(2020, 1, 15)
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
        var request = new AddWorkExperienceRequest
        {
            Position = "Software Engineer",
            Organization = "Tech Corporation",
            CountryCode = "US",
            City = "San Francisco",
            Street = "Market Street",
            HouseNumber = "100",
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
        var request = new AddWorkExperienceRequest
        {
            Position = "Software Engineer",
            Organization = "Tech Corporation",
            CountryCode = "US",
            City = "San Francisco",
            Street = "Market Street",
            HouseNumber = "100",
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