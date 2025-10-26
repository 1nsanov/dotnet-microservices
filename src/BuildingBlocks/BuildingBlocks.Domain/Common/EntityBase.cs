namespace BuildingBlocks.Domain.Common;

public abstract class EntityBase : IEquatable<EntityBase>
{
    private int? _requestedHashCode;

    public Guid Id { get; protected init; }
    public DateTime CreatedDate { get; protected set; }
    public DateTime? LastModifiedDate { get; protected set; }

    protected void SetCreatedDate()
    {
        CreatedDate = DateTime.UtcNow;
    }

    protected void SetLastModifiedDate()
    {
        LastModifiedDate = DateTime.UtcNow;
    }

    public bool IsTransient()
    {
        return Id == Guid.Empty;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (EntityBase)obj;

        if (other.IsTransient() || IsTransient())
        {
            return ReferenceEquals(this, other);
        }

        return Id == other.Id;
    }

    public bool Equals(EntityBase? other)
    {
        return Equals((object?)other);
    }

    public override int GetHashCode()
    {
        if (IsTransient())
        {
            return base.GetHashCode();
        }

        if (!_requestedHashCode.HasValue)
        {
            _requestedHashCode = Id.GetHashCode() ^ 31;
        }

        return _requestedHashCode.Value;
    }

    public static bool operator ==(EntityBase? left, EntityBase? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(EntityBase? left, EntityBase? right)
    {
        return !(left == right);
    }
}