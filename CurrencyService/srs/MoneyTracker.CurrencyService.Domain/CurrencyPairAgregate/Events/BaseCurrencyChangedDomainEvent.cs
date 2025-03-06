using SharedKernel;

namespace MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate.Events
{
    /// <summary>
    /// Событие смены основной валюты в валютной паре
    /// </summary>
    public class BaseCurrencyChangedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Id валютной пары
        /// </summary>
        public int CurrencyPairId { get; }

        /// <summary>
        /// Id предыдущей валюты
        /// </summary>
        public int OldCurrencyId { get; }

        /// <summary>
        /// Id новой валюты
        /// </summary>
        public int NewCurrencyId { get; }

        public BaseCurrencyChangedDomainEvent(int currencyPairId, int oldCurrencyId, int newCurrencyId)
        {
            CurrencyPairId = currencyPairId;
            OldCurrencyId = oldCurrencyId;
            NewCurrencyId = newCurrencyId;
        }
    }
}
