using MoneyTracker.Accounts.Domain.Transfers;

namespace MoneyTracker.Accounts.Infrastructure.Data.Repositories.Transfers
{
    internal class EfCoreTransferRepository : ITransferRepository
    {
        private AccountsDbContext _context;

        public EfCoreTransferRepository(AccountsDbContext context)
        {
            _context = context;
        }

        public Transfer GetById(Guid id) => _context.Transfers
            .FirstOrDefault(t => t.Id == id);
    }
}
