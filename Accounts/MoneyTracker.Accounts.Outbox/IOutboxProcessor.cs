namespace MoneyTracker.Accounts.Outbox;

/// <summary>
/// Фоновая задача-обработчик записей в outbox
/// </summary>
internal interface IOutboxProcessor
{
    /// <summary>
    /// Выполняет обработку outbox-записей
    /// </summary>
    /// <param name="cancellationToken">Токен завершения задачи</param>
    Task ProcessPendingMessagesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Получает количество outbox-записей
    /// </summary>
    /// <param name="cancellationToken">Токен завершения задачи</param>
    Task<int> GetPendingMessagesCountAsync(CancellationToken cancellationToken);
}
