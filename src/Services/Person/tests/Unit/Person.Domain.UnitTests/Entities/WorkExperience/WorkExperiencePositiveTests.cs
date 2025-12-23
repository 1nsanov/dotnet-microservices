using FluentAssertions;
using Person.Domain.UnitTests.TestData;
using WorkExperienceEntity = Person.Domain.Entities.WorkExperience;

namespace Person.Domain.UnitTests.Entities.WorkExperience;

public class WorkExperiencePositiveTests
{
    #region Create Tests

    [Fact]
    public void Create_ValidDataWithoutTermination_ReturnsWorkExperienceWithCorrectProperties()
    {
        // Act
        var workExperience = WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate);

        // Assert
        workExperience.Should().NotBeNull();
        workExperience.Id.Should().NotBeEmpty();
        workExperience.Position.Should().Be(WorkExperienceTestData.ValidPosition);
        workExperience.Organization.Should().Be(WorkExperienceTestData.ValidOrganization);
        workExperience.Address.Should().Be(WorkExperienceTestData.ValidAddress);
        workExperience.Description.Should().Be(WorkExperienceTestData.ValidDescription);
        workExperience.DateEmployment.Should().Be(WorkExperienceTestData.ValidEmploymentDate);
        workExperience.DateTermination.Should().BeNull();
        workExperience.IsCurrentJob.Should().BeTrue();
        workExperience.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        workExperience.LastModifiedDate.Should().BeNull();
    }

    [Fact]
    public void Create_ValidDataWithTermination_ReturnsWorkExperienceWithCorrectProperties()
    {
        // Act
        var workExperience = WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate,
            WorkExperienceTestData.ValidTerminationDate);

        // Assert
        workExperience.Should().NotBeNull();
        workExperience.Position.Should().Be(WorkExperienceTestData.ValidPosition);
        workExperience.DateEmployment.Should().Be(WorkExperienceTestData.ValidEmploymentDate);
        workExperience.DateTermination.Should().Be(WorkExperienceTestData.ValidTerminationDate);
        workExperience.IsCurrentJob.Should().BeFalse();
    }

    [Fact]
    public void Create_FieldsWithWhitespace_TrimsFields()
    {
        // Arrange
        var positionWithWhitespace = "  Software Engineer  ";
        var organizationWithWhitespace = "  Tech Corp  ";

        // Act
        var workExperience = WorkExperienceEntity.Create(
            positionWithWhitespace,
            organizationWithWhitespace,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.DescriptionWithWhitespace,
            WorkExperienceTestData.ValidEmploymentDate);

        // Assert
        workExperience.Position.Should().Be("Software Engineer");
        workExperience.Organization.Should().Be("Tech Corp");
        workExperience.Description.Should().Be(WorkExperienceTestData.TrimmedDescription);
    }

    #endregion

    #region Update Tests

    [Fact]
    public void Update_ValidData_UpdatesSuccessfully()
    {
        // Arrange
        var workExperience = WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate);

        // Act
        workExperience.Update(
            WorkExperienceTestData.AlternativePosition,
            WorkExperienceTestData.AlternativeOrganization,
            WorkExperienceTestData.AlternativeAddress,
            WorkExperienceTestData.AlternativeDescription,
            WorkExperienceTestData.AlternativeEmploymentDate,
            WorkExperienceTestData.AlternativeTerminationDate);

        // Assert
        workExperience.Position.Should().Be(WorkExperienceTestData.AlternativePosition);
        workExperience.Organization.Should().Be(WorkExperienceTestData.AlternativeOrganization);
        workExperience.Address.Should().Be(WorkExperienceTestData.AlternativeAddress);
        workExperience.Description.Should().Be(WorkExperienceTestData.AlternativeDescription);
        workExperience.DateEmployment.Should().Be(WorkExperienceTestData.AlternativeEmploymentDate);
        workExperience.DateTermination.Should().Be(WorkExperienceTestData.AlternativeTerminationDate);
        workExperience.LastModifiedDate.Should().NotBeNull();
        workExperience.LastModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Update_RemoveTerminationDate_SetsToNull()
    {
        // Arrange
        var workExperience = WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate,
            WorkExperienceTestData.ValidTerminationDate);

        // Act
        workExperience.Update(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate,
            null);

        // Assert
        workExperience.DateTermination.Should().BeNull();
        workExperience.IsCurrentJob.Should().BeTrue();
    }

    #endregion

    #region IsCurrentJob Tests

    [Fact]
    public void IsCurrentJob_WithoutTerminationDate_ReturnsTrue()
    {
        // Arrange
        var workExperience = WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate);

        // Act & Assert
        workExperience.IsCurrentJob.Should().BeTrue();
    }

    [Fact]
    public void IsCurrentJob_WithTerminationDate_ReturnsFalse()
    {
        // Arrange
        var workExperience = WorkExperienceEntity.Create(
            WorkExperienceTestData.ValidPosition,
            WorkExperienceTestData.ValidOrganization,
            WorkExperienceTestData.ValidAddress,
            WorkExperienceTestData.ValidDescription,
            WorkExperienceTestData.ValidEmploymentDate,
            WorkExperienceTestData.ValidTerminationDate);

        // Act & Assert
        workExperience.IsCurrentJob.Should().BeFalse();
    }

    #endregion
}