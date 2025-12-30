using Person.Application.Contracts.Requests;
using Person.Domain.Enums;

namespace Person.API.IntegrationTests.TestData;

public static class PersonTestData
{
    public static CreatePersonRequest CreateValidPersonRequest() => new()
    {
        Surname = "Smith",
        FirstName = "John",
        Patronymic = "Michael",
        Email = "john.smith@example.com",
        Phone = "+1234567890",
        DateBirth = new DateTime(1990, 5, 15, 0, 0, 0, DateTimeKind.Utc),
        Gender = Gender.Male,
        Comment = "Test person"
    };

    public static CreatePersonRequest CreateValidFemalePersonRequest() => new()
    {
        Surname = "Johnson",
        FirstName = "Emma",
        Patronymic = null,
        Email = "emma.johnson@example.com",
        Phone = "+9876543210",
        DateBirth = new DateTime(1995, 8, 20, 0, 0, 0, DateTimeKind.Utc),
        Gender = Gender.Female,
        Comment = null
    };

    public static UpdatePersonRequest CreateValidUpdatePersonRequest() => new()
    {
        Surname = "Smith",
        FirstName = "John",
        Patronymic = "Michael",
        Email = "john.smith.updated@example.com",
        Phone = "+1111111111",
        DateBirth = new DateTime(1990, 5, 15, 0, 0, 0, DateTimeKind.Utc),
        Gender = Gender.Male,
        Comment = "Updated comment"
    };

    public static AddWorkExperienceRequest CreateValidWorkExperienceRequest() => new()
    {
        Position = "Software Engineer",
        Organization = "Tech Corp",
        CountryCode = "US",
        City = "New York",
        Street = "Broadway",
        HouseNumber = "123",
        PostalCode = "10001",
        Apartment = "5A",
        Description = "Developed web applications",
        DateEmployment = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        DateTermination = null
    };

    public static AddWorkExperienceRequest CreateValidSecondWorkExperienceRequest() => new()
    {
        Position = "Senior Developer",
        Organization = "Innovation Labs",
        CountryCode = "US",
        City = "San Francisco",
        Street = "Market Street",
        HouseNumber = "456",
        PostalCode = "94102",
        Apartment = null,
        Description = "Led development team",
        DateEmployment = new DateTime(2022, 6, 1, 0, 0, 0, DateTimeKind.Utc),
        DateTermination = null
    };

    public static UpdateWorkExperienceRequest CreateValidUpdateWorkExperienceRequest() => new()
    {
        Position = "Lead Software Engineer",
        Organization = "Tech Corp",
        CountryCode = "US",
        City = "New York",
        Street = "Broadway",
        HouseNumber = "123",
        PostalCode = "10001",
        Apartment = "5A",
        Description = "Updated description",
        DateEmployment = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        DateTermination = new DateTime(2023, 12, 31, 0, 0, 0, DateTimeKind.Utc)
    };
}