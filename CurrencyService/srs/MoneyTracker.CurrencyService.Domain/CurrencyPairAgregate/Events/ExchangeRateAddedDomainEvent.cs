using SharedKernel;

namespace MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate.Events
{
    /// <summary>
    /// Событие добавления курса валют для валютной пары
    /// </summary>
    public class ExchangeRateAddedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Id валютной пары
        /// </summary>
        public int CurrencyPairId { get; }

        /// <summary>
        /// Значение курса
        /// </summary>
        public decimal RateValue { get; }

        public ExchangeRateAddedDomainEvent(int currencyPairId, decimal rateValue)
        {
            CurrencyPairId = currencyPairId;
            RateValue = rateValue;
        }
    }
}
