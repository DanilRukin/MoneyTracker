using SharedKernel;

namespace MoneyTracker.Accounts.Domain.Transfers.Events
{
    public class TransferCompletedEvent : DomainEvent
    {
        public Guid TransferId { get; }
        public Guid FromAccountId { get; }
        public Guid ToAccountId { get; }
        public decimal Amount { get; }

        public TransferCompletedEvent(Guid transferId, Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            TransferId = transferId;
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Amount = amount;
        }
    }
}
