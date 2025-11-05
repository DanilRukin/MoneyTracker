using MoneyTracker.Accounts.Domain.Categories;
using SharedKernel;

namespace MoneyTracker.Accounts.Domain.Transactions
{
    internal class Transaction : EntityBase<Guid>
    {
        public DateTime CreatedAt { get; protected set; }

        public MoneyValue Amount { get; protected set; } = default!;

        public virtual Category Category { get; protected set; } = default!;

        public virtual TransactionSource TransactionSource { get; protected set; } = default!;

        private Guid _accountId; // Внешний ключ для Account (только для EF)

        protected Transaction() { }

        public static Transaction Create(MoneyValue value, DateTime creationDate, Category category, TransactionSource source)
        {
            Transaction transaction = new()
            {
                Id = Guid.NewGuid(),
                Amount = value,
                Category = category,
                TransactionSource = source,
                CreatedAt = creationDate,
            };

            return transaction;
        }
    }
}
