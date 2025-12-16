using MoneyTracker.Accounts.Domain.Accounts;

namespace MoneyTracker.Accounts.Infrastructure.Data.Repositories.Accounts;

internal class EfCoreAccountsRepository : IAccountsRepository
{
    private AccountsDbContext _context;

    public EfCoreAccountsRepository(AccountsDbContext context)
    {
        _context = context;
    }

    public Account GetById(Guid id) => _context.Accounts.First(a => a.Id == id);

    public void Save(Account account) => _context.Accounts.Add(account);
}
