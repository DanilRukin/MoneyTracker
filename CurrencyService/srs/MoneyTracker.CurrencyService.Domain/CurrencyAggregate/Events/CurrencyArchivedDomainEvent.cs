using SharedKernel;

namespace MoneyTracker.CurrencyService.Domain.CurrencyAggregate.Events
{
    /// <summary>
    /// Событие архивации валюты
    /// </summary>
    public class CurrencyArchivedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Id валюты
        /// </summary>
        public int CurrencyId { get; }

        public CurrencyArchivedDomainEvent(int currencyId)
        {
            CurrencyId = currencyId;
        }
    }
}
