namespace Person.Domain.Common;

public abstract class ValueObjectBase : IEquatable<ValueObjectBase>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObjectBase)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public bool Equals(ValueObjectBase? other)
    {
        return Equals((object?)other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return GetEqualityComponents()
                .Select(x => x?.GetHashCode() ?? 0)
                .Aggregate(17, (current, hashCode) => current * 31 + hashCode);
        }
    }

    public static bool operator ==(ValueObjectBase? left, ValueObjectBase? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(ValueObjectBase? left, ValueObjectBase? right)
    {
        return !(left == right);
    }
}