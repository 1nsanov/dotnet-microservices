using Person.Domain.ValueObjects;

namespace Person.Domain.UnitTests.TestData;

public static class WorkExperienceTestData
{
    public const string ValidPosition = "Software Engineer";
    public const string AlternativePosition = "Senior Software Engineer";
    public static string LongPosition => new('x', 201);

    public const string ValidOrganization = "Tech Corporation";
    public const string AlternativeOrganization = "Innovation Labs Inc";
    public static string LongOrganization => new('x', 201);

    public static Address ValidAddress => new("US", "San Francisco", "Market Street", "100", "94105", "Suite 200");
    public static Address AlternativeAddress => new("US", "New York", "Broadway", "500", "10012", "Floor 15");

    public const string ValidDescription = "Developed and maintained enterprise applications";
    public const string AlternativeDescription = "Led development team and architected scalable solutions";
    public static string LongDescription => new('x', 2001);
    public const string DescriptionWithWhitespace = "  Description with spaces  ";
    public const string TrimmedDescription = "Description with spaces";

    public static DateTime ValidEmploymentDate => new(2020, 1, 15);
    public static DateTime AlternativeEmploymentDate => new(2018, 6, 1);
    public static DateTime ValidTerminationDate => new(2023, 12, 31);
    public static DateTime AlternativeTerminationDate => new(2022, 3, 15);

    public static DateTime FutureDate => DateTime.UtcNow.AddDays(1);
}