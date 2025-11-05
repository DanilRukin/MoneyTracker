using SharedKernel;

namespace MoneyTracker.Accounts.Domain.Accounts.Events
{
    public class BalanceInitializedEvent : DomainEvent
    {
        public decimal BalanceValue { get; }
        public Guid AccountCode { get; }

        public BalanceInitializedEvent(Guid accountCode, decimal balance)
        {
            AccountCode = accountCode;
            BalanceValue = balance;
        }
    }
}
