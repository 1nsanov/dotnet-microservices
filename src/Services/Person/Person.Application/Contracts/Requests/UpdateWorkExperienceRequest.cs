namespace Person.Application.Contracts.Requests;

public class UpdateWorkExperienceRequest
{
    public required string Position { get; init; }
    public required string Organization { get; init; }
    public required string CountryCode { get; init; }
    public required string City { get; init; }
    public required string Street { get; init; }
    public required string HouseNumber { get; init; }
    public string? PostalCode { get; init; }
    public string? Apartment { get; init; }
    public required string Description { get; init; }
    public required DateTime DateEmployment { get; init; }
    public DateTime? DateTermination { get; init; }
}