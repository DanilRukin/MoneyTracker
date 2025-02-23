using SharedKernel;

namespace MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate.Events
{
    /// <summary>
    /// Событие активации валютной пары
    /// </summary>
    public class CurrencyPairActivatedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Id валютной пары
        /// </summary>
        public int CurrencyPairId { get; }

        public CurrencyPairActivatedDomainEvent(int currencyPairId)
        {
            CurrencyPairId = currencyPairId;
        }
    }
}
