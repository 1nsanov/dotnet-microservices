namespace Person.Application.Validators.Common;

public static class ValidationConstants
{
    public static class Name
    {
        public const int MinLength = 1;
        public const int MaxLength = 100;
    }

    public static class Email
    {
        public const int MaxLength = 254;
    }

    public static class Phone
    {
        public const int MinDigits = 10;
        public const int MaxDigits = 15;
    }

    public static class Person
    {
        public const int MaxCommentLength = 1000;
        public const int MinAge = 0;
        public const int MaxAge = 150;
    }

    public static class WorkExperience
    {
        public const int MinFieldLength = 1;
        public const int MaxPositionLength = 200;
        public const int MaxOrganizationLength = 200;
        public const int MaxDescriptionLength = 2000;
    }

    public static class Address
    {
        public const int MaxCityLength = 100;
        public const int MaxStreetLength = 200;
        public const int MaxHouseNumberLength = 20;
        public const int MaxApartmentLength = 20;
    }
}

