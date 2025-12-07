using Person.Domain.Enums;

namespace Person.Application.Contracts.Responses;

public class PersonResponse
{
    public required Guid Id { get; init; }
    public required string Surname { get; init; }
    public required string FirstName { get; init; }
    public string? Patronymic { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string Phone { get; init; }
    public required DateTime DateBirth { get; init; }
    public required int Age { get; init; }
    public required Gender Gender { get; init; }
    public string? Comment { get; init; }
    public required DateTime CreatedDate { get; init; }
    public DateTime? LastModifiedDate { get; init; }
    public IReadOnlyCollection<WorkExperienceResponse> WorkExperiences { get; init; } = [];
}

public class WorkExperienceResponse
{
    public required Guid Id { get; init; }
    public required string Position { get; init; }
    public required string Organization { get; init; }
    public required AddressResponse Address { get; init; }
    public required string Description { get; init; }
    public required DateTime DateEmployment { get; init; }
    public DateTime? DateTermination { get; init; }
    public required bool IsCurrentJob { get; init; }
    public required DateTime CreatedDate { get; init; }
    public DateTime? LastModifiedDate { get; init; }
}

public class AddressResponse
{
    public required string CountryCode { get; init; }
    public required string City { get; init; }
    public required string Street { get; init; }
    public required string HouseNumber { get; init; }
    public string? PostalCode { get; init; }
    public string? Apartment { get; init; }
    public required string FullAddress { get; init; }
}