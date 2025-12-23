using BuildingBlocks.Domain.Exceptions;
using FluentAssertions;
using Person.Domain.UnitTests.TestData;
using WorkExperienceEntity = Person.Domain.Entities.WorkExperience;

namespace Person.Domain.UnitTests.Entities.WorkExperience;

public class WorkExperienceNegativeTests
{
    #region Create Tests - Position Validation

    [Fact]
    public void Create_EmptyPosition_ThrowsInvalidEntityException()
    {
        // Act
        var act = () => WorkExperienceEntity.Create(
            "",
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*Position cannot be empty*");
    }

    [Fact]
    public void Create_WhitespacePosition_ThrowsInvalidEntityException()
    {
        // Act
        var act = () => WorkExperienceEntity.Create(
            "   ",
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*Position cannot be empty*");
    }

    [Fact]
    public void Create_PositionTooLong_ThrowsInvalidEntityException()
    {
        // Act
        var act = () => WorkExperienceEntity.Create(
            WorkExperienceTestData.LongPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*Position cannot exceed 200 characters*");
    }

    #endregion

    #region Create Tests - Organization Validation

    [Fact]
    public void Create_EmptyOrganization_ThrowsInvalidEntityException()
    {
        // Act
        var act = () => WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            "",
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*Organization cannot be empty*");
    }

    [Fact]
    public void Create_WhitespaceOrganization_ThrowsInvalidEntityException()
    {
        // Act
        var act = () => WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            "   ",
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*Organization cannot be empty*");
    }

    [Fact]
    public void Create_OrganizationTooLong_ThrowsInvalidEntityException()
    {
        // Act
        var act = () => WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.LongOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*Organization cannot exceed 200 characters*");
    }

    #endregion

    #region Create Tests - Address Validation

    [Fact]
    public void Create_NullAddress_ThrowsInvalidEntityException()
    {
        // Act
        var act = () => WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            null!,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*Address cannot be null*");
    }

    #endregion

    #region Create Tests - Description Validation

    [Fact]
    public void Create_EmptyDescription_ThrowsInvalidEntityException()
    {
        // Act
        var act = () => WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            "",
            WorkExperienceTestData.ValidEmploymentDate);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*Description cannot be empty*");
    }

    [Fact]
    public void Create_WhitespaceDescription_ThrowsInvalidEntityException()
    {
        // Act
        var act = () => WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            "   ",
            WorkExperienceTestData.ValidEmploymentDate);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*Description cannot be empty*");
    }

    [Fact]
    public void Create_DescriptionTooLong_ThrowsInvalidEntityException()
    {
        // Act
        var act = () => WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.LongDescription,
            WorkExperienceTestData.ValidEmploymentDate);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*Description cannot exceed 2000 characters*");
    }

    #endregion

    #region Create Tests - DateEmployment Validation

    [Fact]
    public void Create_FutureDateEmployment_ThrowsInvalidEntityException()
    {
        // Act
        var act = () => WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.FutureDate);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*employment date cannot be in the future*");
    }

    #endregion

    #region Create Tests - DateTermination Validation

    [Fact]
    public void Create_TerminationBeforeEmployment_ThrowsInvalidEntityException()
    {
        // Arrange
        var employmentDate = new DateTime(2020, 1, 1);
        var terminationDate = new DateTime(2019, 12, 31);

        // Act
        var act = () => WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            employmentDate,
            terminationDate);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*termination date cannot be earlier than employment date*");
    }

    [Fact]
    public void Create_FutureDateTermination_ThrowsInvalidEntityException()
    {
        // Act
        var act = () => WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate,
            WorkExperienceTestData.FutureDate);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*termination date cannot be in the future*");
    }

    #endregion

    #region Update Tests - Validation

    [Fact]
    public void Update_EmptyPosition_ThrowsInvalidEntityException()
    {
        // Arrange
        var workExperience = WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate);

        // Act
        var act = () => workExperience.Update(
            "",
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate,
            null);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*Position cannot be empty*");
    }

    [Fact]
    public void Update_NullAddress_ThrowsInvalidEntityException()
    {
        // Arrange
        var workExperience = WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate);

        // Act
        var act = () => workExperience.Update(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            null!,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate,
            null);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*Address cannot be null*");
    }

    [Fact]
    public void Update_FutureDateEmployment_ThrowsInvalidEntityException()
    {
        // Arrange
        var workExperience = WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate);

        // Act
        var act = () => workExperience.Update(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.FutureDate,
            null);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*employment date cannot be in the future*");
    }

    [Fact]
    public void Update_TerminationBeforeEmployment_ThrowsInvalidEntityException()
    {
        // Arrange
        var workExperience = WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate);

        var terminationDate = WorkExperienceTestData.ValidEmploymentDate.AddDays(-1);

        // Act
        var act = () => workExperience.Update(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate,
            terminationDate);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*termination date cannot be earlier than employment date*");
    }

    #endregion
}