using System.Net;
using FluentAssertions;
using Person.API.IntegrationTests.Clients;
using Person.API.IntegrationTests.Fixtures;
using Person.API.IntegrationTests.TestData;
using Person.Domain.Enums;
using Refit;

namespace Person.API.IntegrationTests.Controllers;

public class PersonControllerTests : IntegrationTestBase
{
    private readonly IPersonApiClient _client;

    public PersonControllerTests(IntegrationTestFixture fixture) : base(fixture)
    {
        _client = CreateRefitClient<IPersonApiClient>();
    }

    [Fact]
    public async Task Create_WithValidRequest_ShouldReturnCreatedPerson()
    {
        var request = PersonTestData.CreateValidPersonRequest();

        var response = await _client.CreateAsync(request);

        response.Should().NotBeNull();
        response.Id.Should().NotBeEmpty();
        response.Surname.Should().Be(request.Surname);
        response.FirstName.Should().Be(request.FirstName);
        response.Patronymic.Should().Be(request.Patronymic);
        response.Email.Should().Be(request.Email);
        response.Phone.Should().Be(request.Phone);
        response.DateBirth.Should().Be(request.DateBirth);
        response.Gender.Should().Be(request.Gender);
        response.Comment.Should().Be(request.Comment);
        response.Age.Should().BeGreaterThan(0);
        response.FullName.Should().NotBeNullOrEmpty();
        response.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        response.WorkExperiences.Should().BeEmpty();
    }

    [Fact]
    public async Task Create_WithDuplicateEmail_ShouldReturnConflict()
    {
        var request = PersonTestData.CreateValidPersonRequest();
        await _client.CreateAsync(request);

        var act = async () => await _client.CreateAsync(request);

        await act.Should().ThrowAsync<ApiException>()
            .Where(e => e.StatusCode == HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Update_WithValidRequest_ShouldReturnUpdatedPerson()
    {
        var createRequest = PersonTestData.CreateValidPersonRequest();
        var created = await _client.CreateAsync(createRequest);

        var updateRequest = PersonTestData.CreateValidUpdatePersonRequest();
        var response = await _client.UpdateAsync(created.Id, updateRequest);

        response.Should().NotBeNull();
        response.Id.Should().Be(created.Id);
        response.Email.Should().Be(updateRequest.Email);
        response.Phone.Should().Be(updateRequest.Phone);
        response.Comment.Should().Be(updateRequest.Comment);
        response.LastModifiedDate.Should().NotBeNull();
    }

    [Fact]
    public async Task Update_WithNonExistentId_ShouldReturnNotFound()
    {
        var updateRequest = PersonTestData.CreateValidUpdatePersonRequest();
        var nonExistentId = Guid.NewGuid();

        var act = async () => await _client.UpdateAsync(nonExistentId, updateRequest);

        await act.Should().ThrowAsync<ApiException>()
            .Where(e => e.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetById_WithExistingId_ShouldReturnPerson()
    {
        var createRequest = PersonTestData.CreateValidPersonRequest();
        var created = await _client.CreateAsync(createRequest);

        var response = await _client.GetByIdAsync(created.Id);

        response.Should().NotBeNull();
        response.Id.Should().Be(created.Id);
        response.Email.Should().Be(created.Email);
    }

    [Fact]
    public async Task GetById_WithNonExistentId_ShouldReturnNotFound()
    {
        var nonExistentId = Guid.NewGuid();

        var act = async () => await _client.GetByIdAsync(nonExistentId);

        await act.Should().ThrowAsync<ApiException>()
            .Where(e => e.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAll_ShouldReturnAllPersons()
    {
        var request1 = PersonTestData.CreateValidPersonRequest();
        var request2 = PersonTestData.CreateValidFemalePersonRequest();
        await _client.CreateAsync(request1);
        await _client.CreateAsync(request2);

        var response = await _client.GetAllAsync();

        response.Should().NotBeNull();
        response.Should().HaveCount(2);
    }

    [Fact]
    public async Task Delete_WithExistingId_ShouldReturnDeletedPerson()
    {
        var createRequest = PersonTestData.CreateValidPersonRequest();
        var created = await _client.CreateAsync(createRequest);

        var response = await _client.DeleteAsync(created.Id);

        response.Should().NotBeNull();
        response.Id.Should().Be(created.Id);

        var act = async () => await _client.GetByIdAsync(created.Id);
        await act.Should().ThrowAsync<ApiException>()
            .Where(e => e.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_WithNonExistentId_ShouldReturnNotFound()
    {
        var nonExistentId = Guid.NewGuid();

        var act = async () => await _client.DeleteAsync(nonExistentId);

        await act.Should().ThrowAsync<ApiException>()
            .Where(e => e.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task AddWorkExperience_WithValidRequest_ShouldAddWorkExperience()
    {
        var createRequest = PersonTestData.CreateValidPersonRequest();
        var created = await _client.CreateAsync(createRequest);

        var workExpRequest = PersonTestData.CreateValidWorkExperienceRequest();
        var response = await _client.AddWorkExperienceAsync(created.Id, workExpRequest);

        response.Should().NotBeNull();
        response.WorkExperiences.Should().HaveCount(1);
        var workExp = response.WorkExperiences.First();
        workExp.Position.Should().Be(workExpRequest.Position);
        workExp.Organization.Should().Be(workExpRequest.Organization);
        workExp.Description.Should().Be(workExpRequest.Description);
        workExp.IsCurrentJob.Should().BeTrue();
    }

    [Fact]
    public async Task AddWorkExperience_WithNonExistentPersonId_ShouldReturnNotFound()
    {
        var nonExistentId = Guid.NewGuid();
        var workExpRequest = PersonTestData.CreateValidWorkExperienceRequest();

        var act = async () => await _client.AddWorkExperienceAsync(nonExistentId, workExpRequest);

        await act.Should().ThrowAsync<ApiException>()
            .Where(e => e.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateWorkExperience_WithValidRequest_ShouldUpdateWorkExperience()
    {
        var createRequest = PersonTestData.CreateValidPersonRequest();
        var created = await _client.CreateAsync(createRequest);

        var workExpRequest = PersonTestData.CreateValidWorkExperienceRequest();
        var withWorkExp = await _client.AddWorkExperienceAsync(created.Id, workExpRequest);
        var workExpId = withWorkExp.WorkExperiences.First().Id;

        var updateRequest = PersonTestData.CreateValidUpdateWorkExperienceRequest();
        var response = await _client.UpdateWorkExperienceAsync(created.Id, workExpId, updateRequest);

        response.Should().NotBeNull();
        response.WorkExperiences.Should().HaveCount(1);
        var workExp = response.WorkExperiences.First();
        workExp.Position.Should().Be(updateRequest.Position);
        workExp.Description.Should().Be(updateRequest.Description);
        workExp.IsCurrentJob.Should().BeFalse();
        workExp.DateTermination.Should().Be(updateRequest.DateTermination);
    }

    [Fact]
    public async Task UpdateWorkExperience_WithNonExistentWorkExpId_ShouldReturnNotFound()
    {
        var createRequest = PersonTestData.CreateValidPersonRequest();
        var created = await _client.CreateAsync(createRequest);

        var nonExistentWorkExpId = Guid.NewGuid();
        var updateRequest = PersonTestData.CreateValidUpdateWorkExperienceRequest();

        var act = async () => await _client.UpdateWorkExperienceAsync(created.Id, nonExistentWorkExpId, updateRequest);

        await act.Should().ThrowAsync<ApiException>()
            .Where(e => e.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteWorkExperience_WithExistingId_ShouldRemoveWorkExperience()
    {
        var createRequest = PersonTestData.CreateValidPersonRequest();
        var created = await _client.CreateAsync(createRequest);

        var workExpRequest = PersonTestData.CreateValidWorkExperienceRequest();
        var withWorkExp = await _client.AddWorkExperienceAsync(created.Id, workExpRequest);
        var workExpId = withWorkExp.WorkExperiences.First().Id;

        var response = await _client.DeleteWorkExperienceAsync(created.Id, workExpId);

        response.Should().NotBeNull();
        response.WorkExperiences.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteWorkExperience_WithNonExistentWorkExpId_ShouldReturnNotFound()
    {
        var createRequest = PersonTestData.CreateValidPersonRequest();
        var created = await _client.CreateAsync(createRequest);

        var nonExistentWorkExpId = Guid.NewGuid();

        var act = async () => await _client.DeleteWorkExperienceAsync(created.Id, nonExistentWorkExpId);

        await act.Should().ThrowAsync<ApiException>()
            .Where(e => e.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateMultiplePersons_WithDifferentGenders_ShouldCreateSuccessfully()
    {
        var maleRequest = PersonTestData.CreateValidPersonRequest();
        var femaleRequest = PersonTestData.CreateValidFemalePersonRequest();

        var male = await _client.CreateAsync(maleRequest);
        var female = await _client.CreateAsync(femaleRequest);

        male.Gender.Should().Be(Gender.Male);
        female.Gender.Should().Be(Gender.Female);

        var all = await _client.GetAllAsync();
        all.Should().HaveCount(2);
    }

    [Fact]
    public async Task AddMultipleWorkExperiences_ShouldAddAllSuccessfully()
    {
        var createRequest = PersonTestData.CreateValidPersonRequest();
        var created = await _client.CreateAsync(createRequest);

        var workExp1 = PersonTestData.CreateValidWorkExperienceRequest();
        var workExp2 = PersonTestData.CreateValidSecondWorkExperienceRequest();

        await _client.AddWorkExperienceAsync(created.Id, workExp1);
        var response = await _client.AddWorkExperienceAsync(created.Id, workExp2);

        response.WorkExperiences.Should().HaveCount(2);
    }
}