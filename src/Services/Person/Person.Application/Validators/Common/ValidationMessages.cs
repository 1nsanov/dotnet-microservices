namespace Person.Application.Validators.Common;

public static class ValidationMessages
{
    public static class Person
    {
        public const string SurnameRequired = "Surname is required";
        public const string FirstNameRequired = "First name is required";
        public const string EmailRequired = "Email is required";
        public const string PhoneRequired = "Phone is required";
        public const string DateBirthRequired = "Date of birth is required";
        public const string GenderRequired = "Gender is required";
        
        public const string NameInvalidCharacters = "Name contains invalid characters";
        public const string EmailInvalidFormat = "Invalid email format";
        public const string PhoneInvalidFormat = "Invalid phone format";
        public const string InvalidGenderValue = "Invalid gender value";
        
        public const string NameLengthRangeFormat = "Name must be between {0} and {1} characters";
        public const string EmailMaxLengthFormat = "Email cannot exceed {0} characters";
        public const string PhoneDigitsRangeFormat = "Phone number must contain between {0} and {1} digits";
        public const string CommentMaxLengthFormat = "Comment cannot exceed {0} characters";
        public const string DateBirthAgeRangeFormat = "Date of birth must correspond to an age between {0} and {1} years";
    }

    public static class WorkExperience
    {
        public const string PositionRequired = "Position is required";
        public const string OrganizationRequired = "Organization is required";
        public const string DescriptionRequired = "Description is required";
        public const string DateEmploymentRequired = "Date of employment is required";
        
        public const string PositionLengthRangeFormat = "Position must be between {0} and {1} characters";
        public const string OrganizationLengthRangeFormat = "Organization must be between {0} and {1} characters";
        public const string DescriptionLengthRangeFormat = "Description must be between {0} and {1} characters";
        
        public const string DateEmploymentCannotBeInFuture = "Employment date cannot be in the future";
        public const string DateTerminationCannotBeInFuture = "Termination date cannot be in the future";
        public const string DateTerminationMustBeAfterEmployment = "Termination date must be after employment date";
    }

    public static class Address
    {
        public const string CountryCodeRequired = "Country code is required";
        public const string CityRequired = "City is required";
        public const string StreetRequired = "Street is required";
        public const string HouseNumberRequired = "House number is required";
        
        public const string CountryCodeInvalidFormat = "Country code must be in ISO format (2-3 uppercase letters)";
        public const string PostalCodeInvalidFormat = "Invalid postal code format";
        
        public const string CityMaxLengthFormat = "City cannot exceed {0} characters";
        public const string StreetMaxLengthFormat = "Street cannot exceed {0} characters";
        public const string HouseNumberMaxLengthFormat = "House number cannot exceed {0} characters";
        public const string ApartmentMaxLengthFormat = "Apartment cannot exceed {0} characters";
    }
}

