using SharedKernel;

namespace MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate.Events
{
    /// <summary>
    /// Событие смены целевой валюты в валютной паре
    /// </summary>
    public class TargetCurrencyChangedDomainEvent : DomainEvent
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

        public TargetCurrencyChangedDomainEvent(int currencyPairId, int oldCurrencyId, int newCurrencyId)
        {
            CurrencyPairId = currencyPairId;
            OldCurrencyId = oldCurrencyId;
            NewCurrencyId = newCurrencyId;
        }
    }
}
