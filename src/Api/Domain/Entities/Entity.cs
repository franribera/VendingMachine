namespace Api.Domain.Entities;

public abstract class Entity
{
    public long Id { get; protected set; }

    public bool IsTransient()
    {
        return Id == default;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity entity)
            return false;

        if (ReferenceEquals(this, entity))
            return true;

        if (GetType() != entity.GetType())
            return false;

        if (entity.IsTransient() || IsTransient())
            return false;

        return entity.Id == Id;
    }

    public override int GetHashCode()
    {
        if (!IsTransient())
        {
            return Id.GetHashCode() ^ 31;
        }

        return base.GetHashCode();

    }
    public static bool operator ==(Entity? left, Entity? right)
    {
        return left?.Equals(right) ?? Equals(right, null);
    }

    public static bool operator !=(Entity? left, Entity? right)
    {
        return !(left == right);
    }
}