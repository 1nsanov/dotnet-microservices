using Person.Domain.Enums;

namespace Person.Application.Contracts.Requests;

public class UpdatePersonRequest
{
    public required string Surname { get; init; }
    public required string FirstName { get; init; }
    public string? Patronymic { get; init; }
    public required string Email { get; init; }
    public required string Phone { get; init; }
    public required DateTime DateBirth { get; init; }
    public required Gender Gender { get; init; }
    public string? Comment { get; init; }
}