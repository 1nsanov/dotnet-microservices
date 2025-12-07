namespace BuildingBlocks.Domain.Exceptions;

public class InvalidValueObjectException : DomainException
{
    public InvalidValueObjectException(string valueObjectName, string reason)
        : base($"Invalid {valueObjectName}: {reason}")
    {
        ValueObjectName = valueObjectName;
        Reason = reason;
    }

    public string ValueObjectName { get; }
    public string Reason { get; }
}