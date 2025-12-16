namespace MoneyTracker.Accounts.Application.Eventing;

/// <summary>
/// Потребитель событий
/// </summary>
internal interface IEventSubscriber : IDisposable
{
    /// <summary>
    /// Подписывается на событие
    /// </summary>
    /// <typeparam name="TEvent">Тип события</typeparam>
    /// <param name="handler">Обработчик события</param>
    /// <param name="cancellationToken">Токен завершения задачи</param>
    Task SubscribeAsync<TEvent>(
        Func<TEvent, Task> handler,
        CancellationToken cancellationToken = default) where TEvent: class;
}
