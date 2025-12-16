namespace MoneyTracker.Accounts.Application.Eventing;

/// <summary>
/// Шина событий
/// </summary>
internal interface IEventBus : IEventPublisher, IEventSubscriber
{
}
