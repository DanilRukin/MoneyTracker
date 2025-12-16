namespace MoneyTracker.Accounts.Integration.IntegrationEvents.Base;

public class IntegrationEvent
{
    DateTime DateOccured { get; set; } = DateTime.UtcNow;
}
