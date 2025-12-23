using Person.Application.Contracts.Requests;
using Person.Domain.Enums;
using Person.Domain.ValueObjects;

namespace Person.Application.UnitTests.TestData;

public static class PersonServiceTestData
{
    public static Guid ValidPersonId => Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static Guid NonExistingPersonId => Guid.Parse("99999999-9999-9999-9999-999999999999");
    public static Guid ValidWorkExperienceId => Guid.Parse("22222222-2222-2222-2222-222222222222");

    public static CreatePersonRequest ValidCreatePersonRequest => new()
    {
        Surname = "Smith",
        FirstName = "John",
        Patronymic = "Michael",
        Email = "john.smith@example.com",
        Phone = "+12025551234",
        DateBirth = new DateTime(1990, 1, 1),
        Gender = Gender.Male,
        Comment = "Test comment"
    };

    public static UpdatePersonRequest ValidUpdatePersonRequest => new()
    {
        Surname = "Johnson",
        FirstName = "Peter",
        Patronymic = "David",
        Email = "peter.johnson@example.com",
        Phone = "+12025559876",
        DateBirth = new DateTime(1985, 5, 15),
        Gender = Gender.Male,
        Comment = "Updated comment"
    };

    public static AddWorkExperienceRequest ValidAddWorkExperienceRequest => new()
    {
        Position = "Software Engineer",
        Organization = "Tech Corporation",
        CountryCode = "US",
        City = "San Francisco",
        Street = "Market Street",
        HouseNumber = "100",
        PostalCode = "94105",
        Apartment = "Suite 200",
        Description = "Developed enterprise applications",
        DateEmployment = new DateTime(2020, 1, 15),
        DateTermination = null
    };

    public static UpdateWorkExperienceRequest ValidUpdateWorkExperienceRequest => new()
    {
        Position = "Senior Software Engineer",
        Organization = "Innovation Labs",
        CountryCode = "US",
        City = "New York",
        Street = "Broadway",
        HouseNumber = "500",
        PostalCode = "10012",
        Apartment = "Floor 15",
        Description = "Led development team",
        DateEmployment = new DateTime(2018, 6, 1),
        DateTermination = new DateTime(2023, 12, 31)
    };

    public static Person.Domain.Entities.Person CreateValidPerson()
    {
        var fullName = new FullName("Smith", "John", "Michael");
        var email = new Email("john.smith@example.com");
        var phone = new Phone("+12025551234");
        var dateBirth = new DateTime(1990, 1, 1);
        const Gender gender = Gender.Male;
        const string comment = "Test comment";

        return Domain.Entities.Person.Create(fullName, email, phone, dateBirth, gender, comment);
    }

    public static Person.Domain.Entities.Person CreatePersonWithWorkExperience()
    {
        var person = CreateValidPerson();
        var address = new Address("US", "San Francisco", "Market Street", "100", "94105", "Suite 200");
        person.AddWorkExperience("Software Engineer", "Tech Corporation", address, "Job description",
            new DateTime(2020, 1, 15));
        return person;
    }
}