using Person.Application.Contracts.Requests;
using Person.Application.Contracts.Responses;
using Person.Application.Services.Interfaces;

namespace Person.Application.Services;

public class PersonService : IPersonService
{
    public Task<PersonResponse> CreateAsync(CreatePersonRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<PersonResponse> UpdateAsync(Guid personId, UpdatePersonRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<PersonResponse> GetByIdAsync(Guid personId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<PersonResponse> GetAllAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<PersonResponse> DeleteAsync(Guid personId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<PersonResponse> AddWorkExperienceAsync(Guid personId, AddWorkExperienceRequest request,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<PersonResponse> UpdateWorkExperienceAsync(Guid personId, Guid workExperienceId,
        UpdateWorkExperienceRequest request,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<PersonResponse> DeleteWorkExperienceAsync(Guid personId, Guid workExperienceId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}