using MoneyTracker.Accounts.Domain.Transfers.Events;
using SharedKernel;
using SharedKernel.Interfaces;

namespace MoneyTracker.Accounts.Domain.Transfers
{
    internal class Transfer : EntityBase<Guid>, IAgregateRoot
    {
        public Guid FromAccountId { get; protected set; }

        public Guid ToAccountId { get; protected set; }

        public TransferStatus Status { get; protected set; }

        public MoneyValue Amount { get; protected set; } = default!;

        public DateTime TransferDate { get; protected set; }

        public DateTime CreatedAt { get; protected set; }

        public DateTime? CompletedAt { get; protected set; }

        public string Description { get; protected set; } = default!;

        public Guid? ExpenseTransactionId { get; protected set; } = default!;

        public Guid? IncomeTransactionId { get; protected set; } = default!;

        protected Transfer() { }

        public static Transfer Create(Guid fromAccountId, Guid toAccountId, MoneyValue amount, DateTime transferDate, string description = "")
        {
            if (fromAccountId == toAccountId)
                throw new InvalidOperationException("Нельзя переводить на тот же счет");

            Transfer transfer = new()
            {
                Id = Guid.NewGuid(),
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Amount = amount,
                Description = string.IsNullOrWhiteSpace(description)
                    ? "Перевод между счетами"
                    : description,
                CreatedAt = DateTime.UtcNow,
                TransferDate = transferDate,
                Status = TransferStatus.Pending
            };
            transfer.AddDomainEvent(new TransferCreatedEvent(transfer.Id, fromAccountId, toAccountId, amount.Value, transfer.Description));
            return transfer;
        }

        public void MarkAsCompleted(Guid incomeTransactionId, Guid expenseTransactionId)
        {
            if (Status != TransferStatus.Pending)
                throw new InvalidOperationException($"Невозможно завершить перевод в статусе {Status}");

            ExpenseTransactionId = expenseTransactionId;
            IncomeTransactionId = incomeTransactionId;
            CompletedAt = DateTime.UtcNow;
            Status = TransferStatus.Completed;

            AddDomainEvent(new TransferCompletedEvent(Id, FromAccountId, ToAccountId, Amount.Value));
        }

        public void MarkAsFailed(string reason)
        {
            if (Status != TransferStatus.Pending)
                throw new InvalidOperationException(
                    $"Невозможно отменить перевод в статусе {Status}");

            Status = TransferStatus.Failed;
            AddDomainEvent(new TransferFailedEvent(Id, FromAccountId, ToAccountId, Amount.Value, reason));
        }

        public void Cancel(string reason)
        {
            if (Status != TransferStatus.Completed)
                throw new InvalidOperationException("Можно отменять только завершенные переводы");

            Status = TransferStatus.Cancelled;
            AddDomainEvent(new TransferCancelledEvent(Id, FromAccountId, ToAccountId, Amount.Value, reason));
        }


    }
}
