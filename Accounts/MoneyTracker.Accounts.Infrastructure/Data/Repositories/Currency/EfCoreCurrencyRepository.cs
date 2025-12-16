using MoneyTracker.Accounts.Domain.Currencies;

namespace MoneyTracker.Accounts.Infrastructure.Data.Repositories.Currency;

internal class EfCoreCurrencyRepository : ICurrencyRepository
{
    private AccountsDbContext _context;

    public EfCoreCurrencyRepository(AccountsDbContext context)
    {
        _context = context;
    }

    public Domain.Currencies.Currency? GetByCode(string code) => _context.Currencies
        ?.FirstOrDefault(c => c.Name == code);
}
