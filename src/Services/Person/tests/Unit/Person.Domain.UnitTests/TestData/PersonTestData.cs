using Person.Domain.Enums;
using Person.Domain.ValueObjects;

namespace Person.Domain.UnitTests.TestData;

public static class PersonTestData
{
    public static FullName ValidFullName => new("Smith", "John", "Michael");
    public static FullName AlternativeFullName => new("Johnson", "Peter", "David");

    public static Email ValidEmail => new("john.smith@example.com");
    public static Email AlternativeEmail => new("peter.johnson@example.com");
    public static Email NewEmail => new("new.email@example.com");

    public static Phone ValidPhone => new("+12025551234");
    public static Phone AlternativePhone => new("+12025559876");

    public static DateTime ValidDateBirth => new(1990, 1, 1);
    public static DateTime AlternativeDateBirth => new(1985, 5, 15);

    public const Gender ValidGender = Gender.Male;
    public const Gender AlternativeGender = Gender.Female;

    public const string ValidComment = "Test comment";
    public const string AlternativeComment = "Updated comment";
    public const string CommentWithWhitespace = "  Test comment with spaces  ";
    public const string TrimmedComment = "Test comment with spaces";

    public static Address ValidAddress => new("US", "New York", "Broadway", "123", "10001", "5A");
    public static Address AlternativeAddress => new("US", "San Francisco", "Market Street", "200", "94102", null);
    public static Address ThirdAddress => new("US", "Chicago", "Michigan Avenue", "300", "60601", null);
    public static Address FourthAddress => new("US", "Seattle", "Pike Street", "400", "98101", null);
    public static Address FifthAddress => new("US", "Boston", "Main Street", "50", "02101", null);
    public static Address SixthAddress => new("US", "Austin", "Congress Avenue", "500", "78701", null);

    public const string ValidPosition = "Software Developer";
    public const string AlternativePosition = "Senior Software Developer";

    public const string ValidOrganization = "Tech Company LLC";
    public const string AlternativeOrganization = "New Company Inc";

    public const string ValidJobDescription = "Job description";
    public const string AlternativeJobDescription = "New job description";

    public static DateTime ValidEmploymentDate => new(2020, 1, 1);
    public static DateTime ValidTerminationDate => new(2023, 1, 1);
}