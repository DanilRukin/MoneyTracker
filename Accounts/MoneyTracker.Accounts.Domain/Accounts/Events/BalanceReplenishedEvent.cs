using SharedKernel;

namespace MoneyTracker.Accounts.Domain.Accounts.Events;

public class BalanceReplenishedEvent : DomainEvent
{
    public decimal DepositAmount { get; private set; }
    public Guid AccountCode { get; private set; }

    public BalanceReplenishedEvent(decimal depositAmount, Guid accountCode)
    {
        DepositAmount = depositAmount;
        AccountCode = accountCode;
    }
}