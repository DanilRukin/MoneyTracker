namespace MoneyTracker.Infrastructure.Data.Base
{
    /// <summary>
    /// Наполнитель БД
    /// </summary>
    public interface IDataSeeder
    {
        /// <summary>
        /// Выполняет наполнение БД
        /// </summary>
        Task SeedDataAsync(CancellationToken token);
    }
}
