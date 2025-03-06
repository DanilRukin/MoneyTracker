using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.CurrencyService.Domain.CurrencyAggregate.Events
{
    public class CurrencyPairAddedDomainEvent : DomainEvent
    {
        public int CurrencyId { get; }
        public int CurrencyPairId { get; }

        public CurrencyPairAddedDomainEvent(int currencyId, int currencyPairId)
        {
            CurrencyId = currencyId;
            CurrencyPairId = currencyPairId;
        }
    }
}
