using DistributedSystem.Contract.Abstractions.Message;
using DistributedSystem.Domain.Abstractions.Entities;

namespace DistributedSystem.Domain.Abstractions.Aggregates;

public abstract class AggregateRoot<T> : Entity<T>
{
    private readonly List<IDomainEvent> _domainEvents = new ();

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents () => _domainEvents.ToList();

    public void clearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
