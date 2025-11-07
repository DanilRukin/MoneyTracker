namespace MoneyTracker.Accounts.Domain.Categories
{
    internal class TransferExpenseCategory : ExpenseCategory
    {
        protected TransferExpenseCategory() : base() { }

        public TransferExpenseCategory(string name) : base(name) { }
    }
}
