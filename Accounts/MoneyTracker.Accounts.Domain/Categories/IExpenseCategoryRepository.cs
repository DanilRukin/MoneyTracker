namespace MoneyTracker.Accounts.Domain.Categories
{
    internal interface IExpenseCategoryRepository
    {
        ExpenseCategory? GetByName(string name);
    }
}
