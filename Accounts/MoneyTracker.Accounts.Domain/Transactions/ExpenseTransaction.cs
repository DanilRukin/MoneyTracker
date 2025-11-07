using MoneyTracker.Accounts.Domain.Categories;

namespace MoneyTracker.Accounts.Domain.Transactions
{
    internal class ExpenseTransaction : Transaction
    {
        protected ExpenseTransaction(DateTime createdAt, MoneyValue amount, Category category, TransactionSource transactionSource, Guid accountId) : base(createdAt, amount, category, transactionSource, accountId)
        {
        }

        protected ExpenseTransaction() { }

        public override MoneyValue ApplyBalance(MoneyValue balance)
        {
            if (balance < Amount)
                throw new InvalidOperationException("Недостаточно средств на счете");
            return balance - Amount;
        }

        public static ExpenseTransaction Create(DateTime createdAt, MoneyValue amount,
            ExpenseCategory category, TransactionSource source, Guid accountId)
        {
            return new ExpenseTransaction(createdAt, amount, category, source, accountId);
        }
    }
}
