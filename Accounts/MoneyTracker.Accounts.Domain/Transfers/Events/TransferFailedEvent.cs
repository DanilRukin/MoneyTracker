using SharedKernel;

namespace MoneyTracker.Accounts.Domain.Transfers.Events
{
    public class TransferFailedEvent : DomainEvent
    {
        public Guid TransferId { get; }
        public Guid FromAccountId { get; }
        public Guid ToAccountId { get; }
        public decimal Amount { get; }
        public string Reason { get; }

        public TransferFailedEvent(Guid transferId, Guid fromAccountId, Guid toAccountId, decimal amount, string reason)
        {
            TransferId = transferId;
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Amount = amount;
            Reason = reason;
        }
    }
}
