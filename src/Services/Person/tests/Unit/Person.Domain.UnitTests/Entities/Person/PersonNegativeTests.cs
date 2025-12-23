using BuildingBlocks.Domain.Exceptions;
using FluentAssertions;
using Person.Domain.Enums;
using Person.Domain.UnitTests.TestData;
using PersonEntity = Person.Domain.Entities.Person;

namespace Person.Domain.UnitTests.Entities.Person;

public class PersonNegativeTests
{
    #region Create Tests - Null Validation

    [Fact]
    public void Create_NullFullName_ThrowsInvalidEntityException()
    {
        // Act
        var act = () => PersonEntity.Create(
            null!,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*FullName cannot be null*");
    }

    [Fact]
    public void Create_NullEmail_ThrowsInvalidEntityException()
    {
        // Act
        var act = () => PersonEntity.Create(
            PersonTestData.ValidFullName,
            null!,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*Email cannot be null*");
    }

    [Fact]
    public void Create_NullPhone_ThrowsInvalidEntityException()
    {
        // Act
        var act = () => PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            null!,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*Phone cannot be null*");
    }

    #endregion

    #region Create Tests - DateBirth Validation

    [Fact]
    public void Create_FutureDateBirth_ThrowsInvalidEntityException()
    {
        // Arrange
        var futureDateBirth = DateTime.UtcNow.AddDays(1);

        // Act
        var act = () => PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            futureDateBirth,
            PersonTestData.ValidGender);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*date of birth cannot be in the future*");
    }

    [Fact]
    public void Create_AgeTooOld_ThrowsInvalidEntityException()
    {
        // Arrange
        var tooOldDateBirth = DateTime.UtcNow.AddYears(-151);

        // Act
        var act = () => PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            tooOldDateBirth,
            PersonTestData.ValidGender);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*age cannot exceed 150 years*");
    }

    #endregion

    #region Create Tests - Gender Validation

    [Fact]
    public void Create_GenderNone_ThrowsInvalidEntityException()
    {
        // Act
        var act = () => PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            Gender.None);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*gender cannot be None*");
    }

    [Fact]
    public void Create_InvalidGenderValue_ThrowsInvalidEntityException()
    {
        // Arrange
        var invalidGender = (Gender)99;

        // Act
        var act = () => PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            invalidGender);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*invalid gender value*");
    }

    #endregion

    #region Create Tests - Comment Validation

    [Fact]
    public void Create_CommentTooLong_ThrowsInvalidEntityException()
    {
        // Arrange
        var longComment = new string('a', 1001);

        // Act
        var act = () => PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender,
            longComment);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*comment cannot exceed 1000 characters*");
    }

    #endregion

    #region UpdatePersonalInfo Tests - Null Validation

    [Fact]
    public void UpdatePersonalInfo_NullFullName_ThrowsInvalidEntityException()
    {
        // Arrange
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);

        // Act
        var act = () => person.UpdatePersonalInfo(
            null!,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender,
            null);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*FullName cannot be null*");
    }

    [Fact]
    public void UpdatePersonalInfo_NullEmail_ThrowsInvalidEntityException()
    {
        // Arrange
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);

        // Act
        var act = () => person.UpdatePersonalInfo(
            PersonTestData.ValidFullName,
            null!,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender,
            null);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*Email cannot be null*");
    }

    [Fact]
    public void UpdatePersonalInfo_NullPhone_ThrowsInvalidEntityException()
    {
        // Arrange
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);

        // Act
        var act = () => person.UpdatePersonalInfo(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            null!,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender,
            null);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*Phone cannot be null*");
    }

    #endregion

    #region UpdateEmail Tests - Null Validation

    [Fact]
    public void UpdateEmail_NullEmail_ThrowsInvalidEntityException()
    {
        // Arrange
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);

        // Act
        var act = () => person.UpdateEmail(null!);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage("*Email cannot be null*");
    }

    #endregion

    #region WorkExperience Tests - Exceptions

    [Fact]
    public void RemoveWorkExperience_NonExistingId_ThrowsInvalidEntityException()
    {
        // Arrange
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);
        var nonExistingId = Guid.NewGuid();

        // Act
        var act = () => person.RemoveWorkExperience(nonExistingId);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage($"*WorkExperience with Id {nonExistingId} not found*");
    }

    [Fact]
    public void UpdateWorkExperience_NonExistingId_ThrowsInvalidEntityException()
    {
        // Arrange
        var person = PersonEntity.Create(
            PersonTestData.ValidFullName,
            PersonTestData.ValidEmail,
            PersonTestData.ValidPhone,
            PersonTestData.ValidDateBirth,
            PersonTestData.ValidGender);
        var nonExistingId = Guid.NewGuid();

        // Act
        var act = () => person.UpdateWorkExperience(
            nonExistingId,
            PersonTestData.ValidPosition,
            PersonTestData.ValidOrganization,
            PersonTestData.SixthAddress,
            PersonTestData.ValidJobDescription,
            PersonTestData.ValidEmploymentDate);

        // Assert
        act.Should().Throw<InvalidEntityException>()
            .WithMessage($"*WorkExperience with Id {nonExistingId} not found*");
    }

    #endregion
}