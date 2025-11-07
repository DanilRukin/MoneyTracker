namespace MoneyTracker.Accounts.Domain.Transfers
{
    internal interface ITransferRepository
    {
        Transfer GetById(Guid id);
    }
}
