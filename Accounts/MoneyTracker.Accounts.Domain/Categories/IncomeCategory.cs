using MoneyTracker.Accounts.Domain.Transactions;

namespace MoneyTracker.Accounts.Domain.Categories
{
    internal class IncomeCategory : Category
    {
        public IncomeCategory(string name) : base(name)
        {
            Type = TransactionType.Income;
        }

        protected IncomeCategory() { }
        public override TransactionType Type { get; protected set; }
    }
}
