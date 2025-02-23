using SharedKernel;

namespace MoneyTracker.CurrencyService.Domain.ExchangeRateEntity.Events
{
    /// <summary>
    /// Событие обновления курса валют
    /// </summary>
    public class ExchangeRateUpdatedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Id курса валют
        /// </summary>
        public Guid ExchangeRateId { get; }

        /// <summary>
        /// Старое значение курса
        /// </summary>
        public decimal OldValue { get; }

        /// <summary>
        /// Новое значение курса
        /// </summary>
        public decimal NewValue { get; }

        public ExchangeRateUpdatedDomainEvent(Guid exchangeRateId, decimal oldValue, decimal newValue)
        {
            ExchangeRateId = exchangeRateId;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
