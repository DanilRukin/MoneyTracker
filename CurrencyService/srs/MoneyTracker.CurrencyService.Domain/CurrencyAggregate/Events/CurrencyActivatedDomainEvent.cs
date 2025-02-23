using SharedKernel;

namespace MoneyTracker.CurrencyService.Domain.CurrencyAggregate.Events
{
    /// <summary>
    /// Событие активации валюты
    /// </summary>
    public class CurrencyActivatedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Id валюты
        /// </summary>
        public int CurrencyId { get; }

        public CurrencyActivatedDomainEvent(int currencyId)
        {
            CurrencyId = currencyId;
        }
    }
}
