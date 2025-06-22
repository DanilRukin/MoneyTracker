namespace MoneyTracker.CurrencyService.Data.DataProfiles.Base
{
    /// <summary>
    /// Фабрика профилей данных
    /// </summary>
    public interface IDataProfileFactory
    {
        /// <summary>
        /// Создает профиль для подключения к БД
        /// </summary>
        DataProfile CreateProfile();
    }
}
