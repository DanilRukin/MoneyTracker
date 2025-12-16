using MediatR;
using MoneyTracker.Accounts.Application.Eventing;
using MoneyTracker.Accounts.Domain.Accounts.Events;
using MoneyTracker.Accounts.Integration.IntegrationEvents.AccountIntegrationEvents;

namespace MoneyTracker.Accounts.Application.DomainEventHandlers.AccountEventHandlers.AccountCreatedEventHandlers;

internal class PublishToMessageBusHandler : INotificationHandler<AccountCreatedEvent>
{
    private readonly IEventBus _eventBus;

    public PublishToMessageBusHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(AccountCreatedEvent notification, CancellationToken cancellationToken)
    {
        AccountCreatedInegrationEvent message = new()
        {
            Id = notification.Id
        };

        await _eventBus.PublishAsync(message, cancellationToken);
    }
}
