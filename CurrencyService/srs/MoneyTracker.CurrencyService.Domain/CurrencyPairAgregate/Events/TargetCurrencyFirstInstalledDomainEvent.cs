using SharedKernel;

namespace MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate.Events
{
    /// <summary>
    /// Событие установки целевой валюты в валютной паре впервые
    /// </summary>
    public class TargetCurrencyFirstInstalledDomainEvent : DomainEvent
    {
        /// <summary>
        /// Id валютной пары
        /// </summary>
        public int CurrencyPairId { get; }

        /// <summary>
        /// Id валюты
        /// </summary>
        public int CurrencyId { get; }

        public TargetCurrencyFirstInstalledDomainEvent(int currencyPairId, int currencyId)
        {
            CurrencyPairId = currencyPairId;
            CurrencyId = currencyId;
        }
    }
}
