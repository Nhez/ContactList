using CL.Shared.Abstractions.Events;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CL.Shared.Abstractions.Domain;

public interface IAggregateRoot
{
    IReadOnlyList<IEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
public abstract class AggregateRoot : AggregateRoot<Guid>
{
    public AggregateRoot()
    {
        Id = Guid.NewGuid();
    }
}

public abstract class AggregateRoot<TId> : BaseEntity<TId>, IAggregateRoot
{
    private readonly HashSet<IEvent> _domainEvents = new();

    [NotMapped]
    [JsonIgnore]
    public IReadOnlyList<IEvent> DomainEvents => _domainEvents?.ToImmutableList();

    protected void AddDomainEvent(IEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}

public abstract class BaseEntity { }

public abstract class BaseEntity<T> : BaseEntity, IIdentifiable<T>
{
    public T Id { get; protected set; }
}

public interface IIdentifiable<T>
{
    T Id { get; }
}