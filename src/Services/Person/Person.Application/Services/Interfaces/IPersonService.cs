using Person.Application.Contracts.Requests;
using Person.Application.Contracts.Responses;

namespace Person.Application.Services.Interfaces;

public interface IPersonService
{
    // Person operations
    Task<PersonResponse> CreateAsync(CreatePersonRequest request, CancellationToken ct);
    Task<PersonResponse> UpdateAsync(Guid personId, UpdatePersonRequest request, CancellationToken ct);
    Task<PersonResponse> GetByIdAsync(Guid personId, CancellationToken ct);
    Task<PersonResponse> GetAllAsync(CancellationToken ct);
    Task<PersonResponse> DeleteAsync(Guid personId, CancellationToken ct);

    // Work experience operations
    Task<PersonResponse> AddWorkExperienceAsync(Guid personId, AddWorkExperienceRequest request, CancellationToken ct);

    Task<PersonResponse> UpdateWorkExperienceAsync(Guid personId, Guid workExperienceId,
        UpdateWorkExperienceRequest request, CancellationToken ct);

    Task<PersonResponse> DeleteWorkExperienceAsync(Guid personId, Guid workExperienceId, CancellationToken ct);
}