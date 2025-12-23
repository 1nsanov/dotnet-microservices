using FluentAssertions;
using Person.Domain.UnitTests.TestData;
using PersonEntity = Person.Domain.Entities.Person;
using WorkExperienceEntity = Person.Domain.Entities.WorkExperience;

namespace Person.Domain.UnitTests.Entities.Person;

public class PersonPositiveTests
{
    #region Create Tests

    [Fact]
    public void Create_ValidData_ReturnsPersonWithCorrectProperties()
    {
        // Act
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender,
            PersonTestData.ValidComment);

        // Assert
        person.Should().NotBeNull();
        person.Id.Should().NotBeEmpty();
        person.FullName.Should().Be(PersonTestData.ValidFullName);
        person.Email.Should().Be(PersonTestData.ValidEmail);
        person.Phone.Should().Be(PersonTestData.ValidPhone);
        person.DateBirth.Should().Be(PersonTestData.ValidDateBirth);
        person.Gender.Should().Be(PersonTestData.ValidGender);
        person.Comment.Should().Be(PersonTestData.ValidComment);
        person.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        person.LastModifiedDate.Should().BeNull();
    }

    [Fact]
    public void Create_WithoutComment_ReturnsPersonWithNullComment()
    {
        // Act
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);

        // Assert
        person.Comment.Should().BeNull();
    }

    [Fact]
    public void Create_WithWhitespaceComment_ReturnsPersonWithNullComment()
    {
        // Act
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender,
            "   ");

        // Assert
        person.Comment.Should().BeNull();
    }

    [Fact]
    public void Create_CommentWithWhitespace_TrimsComment()
    {
        // Act
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender,
            PersonTestData.CommentWithWhitespace);

        // Assert
        person.Comment.Should().Be(PersonTestData.TrimmedComment);
    }

    #endregion

    #region UpdatePersonalInfo Tests

    [Fact]
    public void UpdatePersonalInfo_ValidData_UpdatesSuccessfully()
    {
        // Arrange
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);

        // Act
        person.UpdatePersonalInfo(
            PersonTestData.AlternativeFullName,
            PersonTestData.AlternativeEmail,
            PersonTestData.AlternativePhone,
            PersonTestData.AlternativeDateBirth,
            PersonTestData.AlternativeGender,
            PersonTestData.AlternativeComment);

        // Assert
        person.FullName.Should().Be(PersonTestData.AlternativeFullName);
        person.Email.Should().Be(PersonTestData.AlternativeEmail);
        person.Phone.Should().Be(PersonTestData.AlternativePhone);
        person.DateBirth.Should().Be(PersonTestData.AlternativeDateBirth);
        person.Gender.Should().Be(PersonTestData.AlternativeGender);
        person.Comment.Should().Be(PersonTestData.AlternativeComment);
        person.LastModifiedDate.Should().NotBeNull();
        person.LastModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    #endregion

    #region UpdateEmail Tests

    [Fact]
    public void UpdateEmail_ValidEmail_UpdatesSuccessfully()
    {
        // Arrange
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);

        // Act
        person.UpdateEmail(PersonTestData.NewEmail);

        // Assert
        person.Email.Should().Be(PersonTestData.NewEmail);
        person.LastModifiedDate.Should().NotBeNull();
        person.LastModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    #endregion

    #region WorkExperience Tests

    [Fact]
    public void AddWorkExperience_ValidData_AddsSuccessfully()
    {
        // Arrange
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);

        // Act
        person.AddWorkExperience(
            PersonTestData.ValidPosition,
            PersonTestData.ValidOrganization,
            PersonTestData.ValidAddress,
            PersonTestData.ValidJobDescription,
            PersonTestData.ValidEmploymentDate);

        // Assert
        person.WorkExperiences.Should().HaveCount(1);
        var workExperience = person.WorkExperiences.First();
        workExperience.Position.Should().Be(PersonTestData.ValidPosition);
        workExperience.Organization.Should().Be(PersonTestData.ValidOrganization);
        workExperience.Address.Should().Be(PersonTestData.ValidAddress);
        workExperience.Description.Should().Be(PersonTestData.ValidJobDescription);
        person.LastModifiedDate.Should().NotBeNull();
    }

    [Fact]
    public void AddWorkExperience_MultipleExperiences_AddsAllSuccessfully()
    {
        // Arrange
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);

        // Act
        person.AddWorkExperience(
            PersonTestData.ValidPosition,
            "Company One",
            PersonTestData.AlternativeAddress,
            "Description one",
            PersonTestData.ValidEmploymentDate);

        person.AddWorkExperience(
            PersonTestData.AlternativePosition,
            "Company Two",
            PersonTestData.ThirdAddress,
            "Description two",
            new DateTime(2022, 1, 1));

        // Assert
        person.WorkExperiences.Should().HaveCount(2);
    }

    [Fact]
    public void RemoveWorkExperience_ExistingId_RemovesSuccessfully()
    {
        // Arrange
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);

        person.AddWorkExperience(
            PersonTestData.ValidPosition,
            PersonTestData.ValidOrganization,
            PersonTestData.FifthAddress,
            PersonTestData.ValidJobDescription,
            PersonTestData.ValidEmploymentDate);

        var workExperienceId = person.WorkExperiences.First().Id;

        // Act
        person.RemoveWorkExperience(workExperienceId);

        // Assert
        person.WorkExperiences.Should().BeEmpty();
        person.LastModifiedDate.Should().NotBeNull();
    }

    [Fact]
    public void UpdateWorkExperience_ValidData_UpdatesSuccessfully()
    {
        // Arrange
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);

        person.AddWorkExperience(
            PersonTestData.ValidPosition,
            "Old Company LLC",
            PersonTestData.ThirdAddress,
            "Old job description",
            PersonTestData.ValidEmploymentDate);

        var workExperienceId = person.WorkExperiences.First().Id;

        // Act
        person.UpdateWorkExperience(
            workExperienceId,
            PersonTestData.AlternativePosition,
            PersonTestData.AlternativeOrganization,
            PersonTestData.FourthAddress,
            PersonTestData.AlternativeJobDescription,
            PersonTestData.ValidEmploymentDate,
            PersonTestData.ValidTerminationDate);

        // Assert
        var updatedExperience = person.WorkExperiences.First();
        updatedExperience.Position.Should().Be(PersonTestData.AlternativePosition);
        updatedExperience.Organization.Should().Be(PersonTestData.AlternativeOrganization);
        updatedExperience.Address.Should().Be(PersonTestData.FourthAddress);
        updatedExperience.Description.Should().Be(PersonTestData.AlternativeJobDescription);
        updatedExperience.DateTermination.Should().Be(PersonTestData.ValidTerminationDate);
        person.LastModifiedDate.Should().NotBeNull();
    }

    #endregion

    #region Age Tests

    [Fact]
    public void Age_CalculatesCorrectly()
    {
        // Arrange
        var dateBirth = DateTime.UtcNow.Date.AddYears(-30);
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            dateBirth,
            PersonTestData.ValidGender);

        // Act
        var age = person.Age;

        // Assert
        age.Should().Be(30);
    }

    [Fact]
    public void Age_BeforeBirthdayThisYear_CalculatesCorrectly()
    {
        // Arrange
        var today = DateTime.UtcNow.Date;
        var dateBirth = new DateTime(today.Year - 30, today.Month, today.Day).AddDays(1);
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            dateBirth,
            PersonTestData.ValidGender);

        // Act
        var age = person.Age;

        // Assert
        age.Should().Be(29);
    }

    [Fact]
    public void Age_AfterBirthdayThisYear_CalculatesCorrectly()
    {
        // Arrange
        var today = DateTime.UtcNow.Date;
        var dateBirth = new DateTime(today.Year - 30, today.Month, today.Day).AddDays(-1);
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            dateBirth,
            PersonTestData.ValidGender);

        // Act
        var age = person.Age;

        // Assert
        age.Should().Be(30);
    }

    #endregion

    #region WorkExperiences Collection Tests

    [Fact]
    public void WorkExperiences_InitiallyEmpty()
    {
        // Arrange & Act
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);

        // Assert
        person.WorkExperiences.Should().BeEmpty();
    }

    [Fact]
    public void WorkExperiences_ReturnsReadOnlyCollection()
    {
        // Arrange
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);

        // Act & Assert
        person.WorkExperiences.Should().BeAssignableTo<IReadOnlyCollection<WorkExperienceEntity>>();
    }

    #endregion
}