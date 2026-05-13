using MediatR;
using MoneyTracker.SharedKernel.Interfaces;

namespace MoneyTracker.SharedKernel
{
    /// <summary>
    /// <inheritdoc cref="IDomainEventDispatcher"/>
    /// </summary>
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private IMediator _mediator;

        public DomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DispatchAndClearEvents(IEnumerable<IDomainObject> entities)
        {
            foreach (var entity in entities)
            {
                foreach (var domainEvent in entity.DomainEvents)
                {
                    await _mediator.Publish(domainEvent).ConfigureAwait(false);
                }
                entity.ClearDomainEvents();
            }
        }
    }
}
