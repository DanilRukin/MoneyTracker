namespace MoneyTracker.CurrencyService.Data.SeedData.Base
{
    /// <summary>
    /// Наполнитель БД
    /// </summary>
    public interface IDataSeeder
    {
        /// <summary>
        /// Выполняет наполнение БД
        /// </summary>
        Task SeedData(bool clearExisting = true);
    }
}
