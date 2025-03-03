using SharedKernel;

namespace MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate.Events
{
    /// <summary>
    /// Событие установки базовой валюты в валютной паре впервые
    /// </summary>
    public class BaseCurrencyFirstInstalledDomainEvent : DomainEvent
    {
        /// <summary>
        /// Id валютной пары
        /// </summary>
        public int CurrencyPairId { get; }

        /// <summary>
        /// Id валюты
        /// </summary>
        public int CurrencyId { get; }

        public BaseCurrencyFirstInstalledDomainEvent(int currencyPairId, int currencyId)
        {
            CurrencyPairId = currencyPairId;
            CurrencyId = currencyId;
        }
    }
}
