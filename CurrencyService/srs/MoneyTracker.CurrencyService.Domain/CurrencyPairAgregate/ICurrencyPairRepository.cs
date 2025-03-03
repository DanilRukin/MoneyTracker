namespace MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate
{
    /// <summary>
    /// Репозиторий валютных пар
    /// </summary>
    public interface ICurrencyPairRepository
    {
        /// <summary>
        /// Сохраняет ваютную пару
        /// </summary>
        /// <param name="pair">ваютная пара для сохранения (без id)</param>
        /// <returns>Возвращает ту же валютную пару, но с уже установленным id</returns>
        CurrencyPair Save(CurrencyPair pair);

        /// <summary>
        /// Получает валютную пару по id
        /// </summary>
        CurrencyPair Get(int id);
        
        /// <summary>
        /// Обновляет валютную пару
        /// </summary>
        bool Update(CurrencyPair pair);

        /// <summary>
        /// Удаляет валютную пару
        /// </summary>
        bool Delete(int id);
    }
}
