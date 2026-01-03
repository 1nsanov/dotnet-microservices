using AutoMapper;
using BuildingBlocks.Application.Interfaces;
using FluentAssertions;
using Moq;
using Person.Application.Contracts.Responses;
using Person.Application.Interfaces.Repositories;
using Person.Application.Services;
using Person.Application.UnitTests.TestData;
using PersonEntity = Person.Domain.Entities.Person;

namespace Person.Application.UnitTests.Services;

public class PersonServicePositiveTests
{
    private readonly Mock<IPersonRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly PersonService _personService;

    public PersonServicePositiveTests()
    {
        _repositoryMock = new Mock<IPersonRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();

        _personService = new PersonService(_repositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object);
    }

    private void SetupMapperForCreate()
    {
        _mapperMock.Setup(x => x.Map<PersonEntity>(It.IsAny<object>()))
            .Returns((object _) => PersonServiceTestData.CreateValidPerson());

        _mapperMock.Setup(x => x.Map<PersonResponse>(It.IsAny<PersonEntity>()))
            .Returns((PersonEntity src) => new PersonResponse
            {
                Id = src.Id,
                Surname = src.FullName.Surname,
                FirstName = src.FullName.FirstName,
                Patronymic = src.FullName.Patronymic,
                FullName = src.FullName.ToString(),
                Email = src.Email.Value,
                Phone = src.Phone.Value,
                DateBirth = src.DateBirth,
                Gender = src.Gender,
                Age = src.Age,
                Comment = src.Comment,
                WorkExperiences = src.WorkExperiences.Select(we => new WorkExperienceResponse
                {
                    Id = we.Id,
                    Position = we.Position,
                    Organization = we.Organization,
                    Address = new AddressResponse
                    {
                        CountryCode = we.Address.CountryCode,
                        City = we.Address.City,
                        Street = we.Address.Street,
                        HouseNumber = we.Address.HouseNumber,
                        PostalCode = we.Address.PostalCode,
                        Apartment = we.Address.Apartment,
                        FullAddress = we.Address.ToString()
                    },
                    Description = we.Description,
                    DateEmployment = we.DateEmployment,
                    DateTermination = we.DateTermination,
                    IsCurrentJob = we.IsCurrentJob,
                    CreatedDate = we.CreatedDate,
                    LastModifiedDate = we.LastModifiedDate
                }).ToList(),
                CreatedDate = src.CreatedDate,
                LastModifiedDate = src.LastModifiedDate
            });
    }

    #region CreateAsync Tests

    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsPersonResponse()
    {
        // Arrange
        var request = PersonServiceTestData.ValidCreatePersonRequest;
        SetupMapperForCreate();

        _repositoryMock.Setup(x => x.ExistsByEmailAsync(request.Email, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _personService.CreateAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Surname.Should().Be("Smith");
        result.FirstName.Should().Be("John");
        result.Email.Should().Be("john.smith@example.com");
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_CallsRepositoryAndSavesChanges()
    {
        // Arrange
        var request = PersonServiceTestData.ValidCreatePersonRequest;
        SetupMapperForCreate();

        _repositoryMock.Setup(x => x.ExistsByEmailAsync(It.IsAny<string>(), null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        await _personService.CreateAsync(request, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<PersonEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_ValidRequest_ReturnsUpdatedPersonResponse()
    {
        // Arrange
        var personId = PersonServiceTestData.ValidPersonId;
        var request = PersonServiceTestData.ValidUpdatePersonRequest;
        var existingPerson = PersonServiceTestData.CreateValidPerson();
        SetupMapperForCreate();

        _repositoryMock.Setup(x => x.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPerson);
        _repositoryMock.Setup(x => x.ExistsByEmailAsync(request.Email, personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _personService.UpdateAsync(personId, request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Surname.Should().Be("Johnson");
        result.FirstName.Should().Be("Peter");
        result.Email.Should().Be("peter.johnson@example.com");
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_ExistingPerson_ReturnsPersonResponse()
    {
        // Arrange
        var existingPerson = PersonServiceTestData.CreateValidPerson();
        var personId = existingPerson.Id;
        SetupMapperForCreate();

        _repositoryMock.Setup(x => x.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPerson);

        // Act
        var result = await _personService.GetByIdAsync(personId, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(personId);
    }

    #endregion

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_ReturnsListOfPersonResponses()
    {
        // Arrange
        var persons = new List<PersonEntity>
        {
            PersonServiceTestData.CreateValidPerson(),
            PersonServiceTestData.CreateValidPerson()
        };
        SetupMapperForCreate();

        _mapperMock.Setup(x => x.Map<List<PersonResponse>>(It.IsAny<IReadOnlyList<PersonEntity>>()))
            .Returns((IReadOnlyList<PersonEntity> src) => src.Select(p => new PersonResponse
            {
                Id = p.Id,
                Surname = p.FullName.Surname,
                FirstName = p.FullName.FirstName,
                Patronymic = p.FullName.Patronymic,
                FullName = p.FullName.ToString(),
                Email = p.Email.Value,
                Phone = p.Phone.Value,
                DateBirth = p.DateBirth,
                Gender = p.Gender,
                Age = p.Age,
                Comment = p.Comment,
                WorkExperiences = new List<WorkExperienceResponse>(),
                CreatedDate = p.CreatedDate,
                LastModifiedDate = p.LastModifiedDate
            }).ToList());

        _repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(persons);

        // Act
        var result = await _personService.GetAllAsync(CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().AllBeOfType<PersonResponse>();
    }

    [Fact]
    public async Task GetAllAsync_EmptyRepository_ReturnsEmptyList()
    {
        // Arrange
        _mapperMock.Setup(x => x.Map<List<PersonResponse>>(It.IsAny<IReadOnlyList<PersonEntity>>()))
            .Returns(new List<PersonResponse>());

        _repositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<PersonEntity>());

        // Act
        var result = await _personService.GetAllAsync(CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_ExistingPerson_ReturnsDeletedPersonResponse()
    {
        // Arrange
        var existingPerson = PersonServiceTestData.CreateValidPerson();
        var personId = existingPerson.Id;
        SetupMapperForCreate();

        _repositoryMock.Setup(x => x.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPerson);

        // Act
        var result = await _personService.DeleteAsync(personId, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(personId);
    }

    #endregion

    #region AddWorkExperienceAsync Tests

    [Fact]
    public async Task AddWorkExperienceAsync_ValidRequest_ReturnsPersonResponseWithWorkExperience()
    {
        // Arrange
        var personId = PersonServiceTestData.ValidPersonId;
        var request = PersonServiceTestData.ValidAddWorkExperienceRequest;
        var existingPerson = PersonServiceTestData.CreateValidPerson();
        SetupMapperForCreate();

        _repositoryMock.Setup(x => x.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPerson);

        // Act
        var result = await _personService.AddWorkExperienceAsync(personId, request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.WorkExperiences.Should().HaveCount(1);
        result.WorkExperiences.First().Position.Should().Be("Software Engineer");
    }

    #endregion

    #region UpdateWorkExperienceAsync Tests

    [Fact]
    public async Task UpdateWorkExperienceAsync_ValidRequest_ReturnsUpdatedPersonResponse()
    {
        // Arrange
        var personId = PersonServiceTestData.ValidPersonId;
        var request = PersonServiceTestData.ValidUpdateWorkExperienceRequest;
        var existingPerson = PersonServiceTestData.CreatePersonWithWorkExperience();
        var workExperienceId = existingPerson.WorkExperiences.First().Id;
        SetupMapperForCreate();

        _repositoryMock.Setup(x => x.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPerson);

        // Act
        var result =
            await _personService.UpdateWorkExperienceAsync(personId, workExperienceId, request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.WorkExperiences.Should().HaveCount(1);
        result.WorkExperiences.First().Position.Should().Be("Senior Software Engineer");
    }

    #endregion

    #region DeleteWorkExperienceAsync Tests

    [Fact]
    public async Task DeleteWorkExperienceAsync_ValidRequest_ReturnsPersonResponseWithoutWorkExperience()
    {
        // Arrange
        var personId = PersonServiceTestData.ValidPersonId;
        var existingPerson = PersonServiceTestData.CreatePersonWithWorkExperience();
        var workExperienceId = existingPerson.WorkExperiences.First().Id;
        SetupMapperForCreate();

        _repositoryMock.Setup(x => x.GetByIdAsync(personId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPerson);

        // Act
        var result = await _personService.DeleteWorkExperienceAsync(personId, workExperienceId, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.WorkExperiences.Should().BeEmpty();
    }

    #endregion
}