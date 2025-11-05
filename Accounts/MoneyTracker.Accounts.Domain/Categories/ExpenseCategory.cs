using MoneyTracker.Accounts.Domain.Transactions;

namespace MoneyTracker.Accounts.Domain.Categories
{
    internal class ExpenseCategory : Category
    {
        public ExpenseCategory(string name) : base(name)
        {
            Type = TransactionType.Expense;
        }

        protected ExpenseCategory() { }

        public override TransactionType Type { get; protected set; }
    }
}
