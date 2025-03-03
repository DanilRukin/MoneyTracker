namespace MoneyTracker.CurrencyService.Domain.RateSourceEntity
{
    /// <summary>
    /// Фабрика источников курсов
    /// </summary>
    public interface IRateSourceFactory
    {
        /// <summary>
        /// Создает источник курсов валют
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        RateSource CreateRateSource(string name);
    }
}
