using MoneyTracker.Accounts.Domain.Categories;

namespace MoneyTracker.Accounts.Domain.Transactions
{
    internal class IncomeTransaction : Transaction
    {
        protected IncomeTransaction() : base() { }

        protected IncomeTransaction(DateTime createdAt, MoneyValue amount, Category category, TransactionSource transactionSource, Guid accountId) : base(createdAt, amount, category, transactionSource, accountId)
        {
        }

        public override MoneyValue ApplyBalance(MoneyValue balance)
        {
            return balance + Amount;
        }

        public static IncomeTransaction Create(DateTime createdAt, MoneyValue amount, 
            IncomeCategory category, TransactionSource source, Guid accountId)
        {
            return new IncomeTransaction(createdAt, amount, category, source, accountId);
        }
    }
}
