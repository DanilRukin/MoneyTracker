namespace MoneyTracker.Accounts.Application.Eventing;

/// <summary>
/// Издатель событий
/// </summary>
internal interface IEventPublisher : IDisposable
{
    /// <summary>
    /// Публикует события
    /// </summary>
    /// <typeparam name="TEvent">Тип события</typeparam>
    /// <param name="event">Объект события</param>
    /// <param name="cancellationToken">Токен завершения задачи</param>
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent: class;
}
