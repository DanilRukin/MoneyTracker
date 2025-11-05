using SharedKernel;

namespace MoneyTracker.Accounts.Domain.Accounts.Events;

public class BalanceReducedEvent : DomainEvent
{
    public Guid AccountCode { get; }
    public decimal DebitAmount { get; }

    public BalanceReducedEvent(Guid accountCode, decimal debitAmount)
    {
        AccountCode = accountCode;
        DebitAmount = debitAmount;
    }
}