using Person.Application.Contracts.Requests;
using Person.Application.Contracts.Responses;
using Refit;

namespace Person.API.IntegrationTests.Clients;

public interface IPersonApiClient
{
    [Post("/api/person")]
    Task<PersonResponse> CreateAsync([Body] CreatePersonRequest request, CancellationToken ct = default);

    [Put("/api/person/{id}")]
    Task<PersonResponse> UpdateAsync(Guid id, [Body] UpdatePersonRequest request, CancellationToken ct = default);

    [Get("/api/person/{id}")]
    Task<PersonResponse> GetByIdAsync(Guid id, CancellationToken ct = default);

    [Get("/api/person")]
    Task<List<PersonResponse>> GetAllAsync(CancellationToken ct = default);

    [Delete("/api/person/{id}")]
    Task<PersonResponse> DeleteAsync(Guid id, CancellationToken ct = default);

    [Post("/api/person/{id}/work-experience")]
    Task<PersonResponse> AddWorkExperienceAsync(Guid id, [Body] AddWorkExperienceRequest request,
        CancellationToken ct = default);

    [Put("/api/person/{id}/work-experience/{workExperienceId}")]
    Task<PersonResponse> UpdateWorkExperienceAsync(Guid id, Guid workExperienceId,
        [Body] UpdateWorkExperienceRequest request, CancellationToken ct = default);

    [Delete("/api/person/{id}/work-experience/{workExperienceId}")]
    Task<PersonResponse> DeleteWorkExperienceAsync(Guid id, Guid workExperienceId, CancellationToken ct = default);
}