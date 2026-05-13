using MoneyTracker.SharedKernel;

namespace MoneyTracker.Accounts.Domain.Transfers.Events
{
    public class TransferCancelledEvent : DomainEvent
    {
        public Guid TransferId { get; }
        public Guid FromAccountId { get; }
        public Guid ToAccountId { get; }
        public decimal Amount { get; }
        public string Reason { get; }

        public TransferCancelledEvent(Guid transferId, Guid fromAccountId, Guid toAccountId, decimal amount, string reason)
        {
            TransferId = transferId;
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Amount = amount;
            Reason = reason;
        }
    }
}
