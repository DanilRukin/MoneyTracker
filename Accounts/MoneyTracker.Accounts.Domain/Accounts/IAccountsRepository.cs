namespace MoneyTracker.Accounts.Domain.Accounts
{
    internal interface IAccountsRepository
    {
        Account GetById(Guid id);
    }
}
