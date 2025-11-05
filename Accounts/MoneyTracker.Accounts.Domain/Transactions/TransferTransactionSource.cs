namespace MoneyTracker.Accounts.Domain.Transactions
{
    internal class TransferTransactionSource : TransactionSource
    {
        public TransferTransactionSource() : base(nameof(TransferTransactionSource)) { }
    }
}
