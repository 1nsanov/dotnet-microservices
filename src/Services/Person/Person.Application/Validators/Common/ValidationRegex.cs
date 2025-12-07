using System.Text.RegularExpressions;

namespace Person.Application.Validators.Common;

public static class ValidationRegex
{
    public static readonly Regex Name = new(@"^[\p{L}\s\-']+$", RegexOptions.Compiled);
    public static readonly Regex Phone = new(@"^\+?[0-9\s\-\(\)]+$", RegexOptions.Compiled);
    public static readonly Regex DigitsOnly = new(@"[^\d]", RegexOptions.Compiled);
    public static readonly Regex CountryCode = new(@"^[A-Z]{2,3}$", RegexOptions.Compiled);
    public static readonly Regex PostalCode = new(@"^[\d\s\-]{3,10}$", RegexOptions.Compiled);
}

