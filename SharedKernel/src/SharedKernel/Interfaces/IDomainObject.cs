namespace SharedKernel.Interfaces
{
    public interface IDomainObject
    {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }

        void AddDomainEvent(DomainEvent domainEvent);

        void RemoveDomainEvent(DomainEvent domainEvent);

        void ClearDomainEvents();
    }
}
