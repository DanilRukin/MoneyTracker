using MoneyTracker.Accounts.Domain.Categories;
using SharedKernel;

namespace MoneyTracker.Accounts.Domain.Transactions
{
    internal abstract class Transaction : EntityBase<Guid>
    {
        public DateTime CreatedAt { get; protected set; }

        public MoneyValue Amount { get; protected set; } = default!;

        public virtual Category Category { get; protected set; } = default!;

        public virtual TransactionSource TransactionSource { get; protected set; } = default!;

        public Guid AccountId { get; protected set; }

        protected Transaction() { }

        public abstract MoneyValue ApplyBalance(MoneyValue balance);

        protected Transaction(DateTime createdAt, MoneyValue amount, Category category, TransactionSource transactionSource, Guid accountId)
        {
            Id = Guid.NewGuid();
            CreatedAt = createdAt;
            Amount = amount;
            Category = category;
            TransactionSource = transactionSource;
            AccountId = accountId;
        }
    }
}
