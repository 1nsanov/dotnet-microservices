namespace BuildingBlocks.Application.Exceptions;

public class DuplicateException : Exception
{
    public DuplicateException(string entityName, string propertyName, object value)
        : base($"{entityName} with {propertyName} '{value}' already exists")
    {
        EntityName = entityName;
        PropertyName = propertyName;
        Value = value;
    }

    public string EntityName { get; }
    public string PropertyName { get; }
    public object Value { get; }
}