namespace SharedKernel.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAndClearEvents(IEnumerable<IDomainObject> entities);
    }
}
