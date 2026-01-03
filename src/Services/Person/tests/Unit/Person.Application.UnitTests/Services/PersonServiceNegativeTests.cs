using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using FluentAssertions;
using Moq;
using Person.Application.Interfaces.Repositories;
using Person.Application.Services;
using Person.Application.UnitTests.TestData;
using PersonEntity = Person.Domain.Entities.Person;

namespace Person.Application.UnitTests.Services;

public class PersonServiceNegativeTests
{
    private readonly Mock<IPersonRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly PersonService _personService;

    public PersonServiceNegativeTests()
    {
        _repositoryMock = new Mock<IPersonRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var mapperMock = new Mock<AutoMapper.IMapper>();

        _personService = new PersonService(_repositoryMock.Object, _unitOfWorkMock.Object, mapperMock.Object);
    }

    #region CreateAsync Tests

    [Fact]
    public async Task CreateAsync_DuplicateEmail_ThrowsDuplicateException()
    {
        // Arrange
        var request = PersonServiceTestData.ValidCreatePersonRequest;
        _repositoryMock.Setup(x => x.ExistsByEmailAsync(request.Email, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _personService.CreateAsync(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DuplicateException>()
            .WithMessage("*Email*");
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_PersonNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var personId = PersonServiceTestData.NonExistingPersonId;
        var request = PersonServiceTestData.ValidUpdatePersonRequest;

        _repositoryMock.Setup(x => x.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PersonEntity?)null);

        // Act
        var act = async () => await _personService.UpdateAsync(personId, request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task UpdateAsync_DuplicateEmail_ThrowsDuplicateException()
    {
        // Arrange
        var personId = PersonServiceTestData.ValidPersonId;
        var request = PersonServiceTestData.ValidUpdatePersonRequest;
        var existingPerson = PersonServiceTestData.CreateValidPerson();

        _repositoryMock.Setup(x => x.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPerson);
        _repositoryMock.Setup(x => x.ExistsByEmailAsync(request.Email, personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _personService.UpdateAsync(personId, request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<DuplicateException>();
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_NonExistingPerson_ThrowsNotFoundException()
    {
        // Arrange
        var personId = PersonServiceTestData.NonExistingPersonId;

        _repositoryMock.Setup(x => x.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PersonEntity?)null);

        // Act
        var act = async () => await _personService.GetByIdAsync(personId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_NonExistingPerson_ThrowsNotFoundException()
    {
        // Arrange
        var personId = PersonServiceTestData.NonExistingPersonId;

        _repositoryMock.Setup(x => x.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PersonEntity?)null);

        // Act
        var act = async () => await _personService.DeleteAsync(personId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    #endregion

    #region AddWorkExperienceAsync Tests

    [Fact]
    public async Task AddWorkExperienceAsync_PersonNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var personId = PersonServiceTestData.NonExistingPersonId;
        var request = PersonServiceTestData.ValidAddWorkExperienceRequest;

        _repositoryMock.Setup(x => x.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PersonEntity?)null);

        // Act
        var act = async () => await _personService.AddWorkExperienceAsync(personId, request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    #endregion

    #region UpdateWorkExperienceAsync Tests

    [Fact]
    public async Task UpdateWorkExperienceAsync_PersonNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var personId = PersonServiceTestData.NonExistingPersonId;
        var workExperienceId = PersonServiceTestData.ValidWorkExperienceId;
        var request = PersonServiceTestData.ValidUpdateWorkExperienceRequest;

        _repositoryMock.Setup(x => x.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PersonEntity?)null);

        // Act
        var act = async () =>
            await _personService.UpdateWorkExperienceAsync(personId, workExperienceId, request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    #endregion

    #region DeleteWorkExperienceAsync Tests

    [Fact]
    public async Task DeleteWorkExperienceAsync_PersonNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var personId = PersonServiceTestData.NonExistingPersonId;
        var workExperienceId = PersonServiceTestData.ValidWorkExperienceId;

        _repositoryMock.Setup(x => x.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PersonEntity?)null);

        // Act
        var act = async () =>
            await _personService.DeleteWorkExperienceAsync(personId, workExperienceId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    #endregion
}