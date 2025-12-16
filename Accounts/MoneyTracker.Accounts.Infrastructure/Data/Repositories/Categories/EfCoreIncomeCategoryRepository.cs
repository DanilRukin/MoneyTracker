using MoneyTracker.Accounts.Domain.Categories;

namespace MoneyTracker.Accounts.Infrastructure.Data.Repositories.Categories;

internal class EfCoreIncomeCategoryRepository : IIncomeCategoryRepository
{
    private AccountsDbContext _context;

    public EfCoreIncomeCategoryRepository(AccountsDbContext context)
    {
        _context = context;
    }

    public IncomeCategory? GetByName(string name) => _context.Categories
        ?.OfType<IncomeCategory>()
        ?.FirstOrDefault(c => c.Name == name);
}
