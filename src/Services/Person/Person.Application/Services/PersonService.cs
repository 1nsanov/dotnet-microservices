using AutoMapper;
using BuildingBlocks.Application.Exceptions;
using BuildingBlocks.Application.Interfaces;
using Person.Application.Contracts.Requests;
using Person.Application.Contracts.Responses;
using Person.Application.Interfaces.Repositories;
using Person.Application.Services.Interfaces;
using Person.Domain.ValueObjects;

namespace Person.Application.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PersonService(IPersonRepository personRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PersonResponse> CreateAsync(CreatePersonRequest request, CancellationToken ct)
    {
        if (await _personRepository.ExistsByEmailAsync(request.Email, cancellationToken: ct))
        {
            throw new DuplicateException(nameof(Domain.Entities.Person), "Email", request.Email);
        }

        var person = _mapper.Map<Domain.Entities.Person>(request);

        await _personRepository.AddAsync(person, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<PersonResponse>(person);
    }

    public async Task<PersonResponse> UpdateAsync(Guid personId, UpdatePersonRequest request, CancellationToken ct)
    {
        var person = await GetPersonByIdOrThrowAsync(personId, ct);

        if (await _personRepository.ExistsByEmailAsync(request.Email, personId, ct))
        {
            throw new DuplicateException(nameof(Domain.Entities.Person), "Email", request.Email);
        }

        var fullName = new FullName(request.Surname, request.FirstName, request.Patronymic);
        var email = new Email(request.Email);
        var phone = new Phone(request.Phone);

        person.UpdatePersonalInfo(fullName, email, phone, request.DateBirth, request.Gender, request.Comment);

        await _personRepository.UpdateAsync(person, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<PersonResponse>(person);
    }

    public async Task<PersonResponse> GetByIdAsync(Guid personId, CancellationToken ct)
    {
        var person = await GetPersonByIdOrThrowAsync(personId, ct);
        return _mapper.Map<PersonResponse>(person);
    }

    public async Task<IReadOnlyList<PersonResponse>> GetAllAsync(CancellationToken ct)
    {
        var persons = await _personRepository.GetAllAsync(ct);
        return _mapper.Map<List<PersonResponse>>(persons);
    }

    public async Task<PersonResponse> DeleteAsync(Guid personId, CancellationToken ct)
    {
        var person = await GetPersonByIdOrThrowAsync(personId, ct);

        await _personRepository.DeleteAsync(person, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<PersonResponse>(person);
    }

    public async Task<PersonResponse> AddWorkExperienceAsync(Guid personId, AddWorkExperienceRequest request,
        CancellationToken ct)
    {
        var person = await GetPersonByIdOrThrowAsync(personId, ct);

        person.AddWorkExperience(request.Position, request.Organization,
            new Address(request.CountryCode, request.City, request.Street, request.HouseNumber, request.PostalCode,
                request.Apartment), request.Description, request.DateEmployment, request.DateTermination);

        await _personRepository.UpdateAsync(person, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<PersonResponse>(person);
    }

    public async Task<PersonResponse> UpdateWorkExperienceAsync(Guid personId, Guid workExperienceId,
        UpdateWorkExperienceRequest request,
        CancellationToken ct)
    {
        var person = await GetPersonByIdOrThrowAsync(personId, ct);

        person.UpdateWorkExperience(workExperienceId, request.Position, request.Organization,
            new Address(request.CountryCode, request.City, request.Street, request.HouseNumber, request.PostalCode,
                request.Apartment), request.Description, request.DateEmployment, request.DateTermination);

        await _personRepository.UpdateAsync(person, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<PersonResponse>(person);
    }

    public async Task<PersonResponse> DeleteWorkExperienceAsync(Guid personId, Guid workExperienceId,
        CancellationToken ct)
    {
        var person = await GetPersonByIdOrThrowAsync(personId, ct);

        person.RemoveWorkExperience(workExperienceId);
        await _personRepository.UpdateAsync(person, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<PersonResponse>(person);
    }

    private async Task<Domain.Entities.Person> GetPersonByIdOrThrowAsync(Guid personId, CancellationToken ct)
    {
        var person = await _personRepository.GetByIdAsync(personId, ct);
        if (person is null)
            throw new NotFoundException(nameof(Domain.Entities.Person), personId);

        return person;
    }
}