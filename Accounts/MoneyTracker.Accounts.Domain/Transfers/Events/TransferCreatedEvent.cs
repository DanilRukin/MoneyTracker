using SharedKernel;

namespace MoneyTracker.Accounts.Domain.Transfers.Events
{
    public class TransferCreatedEvent : DomainEvent
    {
        public Guid TransferId { get; }
        public Guid FromAccountId { get; }
        public Guid ToAccountId { get; }
        public decimal Amount { get; }
        public string Description { get; }

        public TransferCreatedEvent(Guid transferId, Guid fromAccountId, Guid toAccountId, decimal amount, string description)
        {
            TransferId = transferId;
            FromAccountId = fromAccountId;
            ToAccountId = toAccountId;
            Amount = amount;
            Description = description ?? string.Empty;
        }
    }
}
