using MoneyTracker.Accounts.Integration.IntegrationEvents.Base;

namespace MoneyTracker.Accounts.Integration.IntegrationEvents.AccountIntegrationEvents;

public class AccountCreatedInegrationEvent : IntegrationEvent
{
    public required Guid Id { get; init; }
}
