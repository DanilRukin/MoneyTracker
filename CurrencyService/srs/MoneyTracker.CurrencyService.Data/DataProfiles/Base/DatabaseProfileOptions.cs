namespace MoneyTracker.CurrencyService.Data.DataProfiles.Base
{
    /// <summary>
    /// Общие настройки БД
    /// </summary>
    public record DatabaseProfileOptions
    {
        /// <summary>
        /// Указывает, следует ли пытаться повторить операцию при неудаче
        /// </summary>
        public bool? EnableRetryOnFailure { get; init; }

        /// <summary>
        /// Тайм-аут для команд
        /// </summary>
        public int? CommandTimeout { get; init; }
    }
}
