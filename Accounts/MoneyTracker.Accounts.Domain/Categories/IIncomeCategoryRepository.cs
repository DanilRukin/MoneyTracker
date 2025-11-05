namespace MoneyTracker.Accounts.Domain.Categories
{
    internal interface IIncomeCategoryRepository
    {
        IncomeCategory GetByName(string name);
    }
}
