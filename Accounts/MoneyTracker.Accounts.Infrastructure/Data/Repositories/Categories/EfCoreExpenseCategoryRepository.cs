using MoneyTracker.Accounts.Domain.Categories;

namespace MoneyTracker.Accounts.Infrastructure.Data.Repositories.Categories;

internal class EfCoreExpenseCategoryRepository : IExpenseCategoryRepository
{
    private AccountsDbContext _context;

    public EfCoreExpenseCategoryRepository(AccountsDbContext context)
    {
        _context = context;
    }

    public ExpenseCategory? GetByName(string name) => _context.Categories
        ?.OfType<ExpenseCategory>()
        ?.FirstOrDefault(c => c.Name == name);
}
