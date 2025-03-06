using SharedKernel;

namespace MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate.Events
{
    /// <summary>
    /// Событие архивации валютной пары
    /// </summary>
    public class CurrencyPairArchivedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Id валютной пары
        /// </summary>
        public int CurrencyPairId { get; }

        public CurrencyPairArchivedDomainEvent(int currencyPairId)
        {
            CurrencyPairId = currencyPairId;
        }
    }
}
