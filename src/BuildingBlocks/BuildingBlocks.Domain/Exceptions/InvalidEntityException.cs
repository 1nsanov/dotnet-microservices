namespace BuildingBlocks.Domain.Exceptions;

public class InvalidEntityException : DomainException
{
    public InvalidEntityException(string entityName, string reason)
        : base($"Invalid {entityName}: {reason}")
    {
        EntityName = entityName;
        Reason = reason;
    }

    public string EntityName { get; }
    public string Reason { get; }
}