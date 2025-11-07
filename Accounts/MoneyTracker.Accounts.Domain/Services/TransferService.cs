using MoneyTracker.Accounts.Domain.Accounts;
using MoneyTracker.Accounts.Domain.Categories;
using MoneyTracker.Accounts.Domain.Transactions;
using MoneyTracker.Accounts.Domain.Transfers;

namespace MoneyTracker.Accounts.Domain.Services
{
    internal class TransferService
    {
        private readonly IAccountsRepository _accountsRepository;
        private readonly ITransferRepository _transfersRepository;

        public TransferService(IAccountsRepository accountsRepository, ITransferRepository transfersRepository)
        {
            _accountsRepository = accountsRepository;
            _transfersRepository = transfersRepository;
        }

        public void ExecuteTransfer(Guid transferId)
        {
            Transfer transfer = _transfersRepository.GetById(transferId);
            if (transfer == null)
                throw new ArgumentException("Перевод не найден");

            Account from = _accountsRepository.GetById(transfer.FromAccountId);
            Account to = _accountsRepository.GetById(transfer.ToAccountId);
            if (from == null || to == null)
                throw new ArgumentException("Один из счетов для перевода не найден");

            try
            {
                from.Expense(
                    transfer.Amount,
                    new TransferExpenseCategory("Исходящий перевод между счетами"),
                    new TransactionSource("Внутренний перевод"));
                to.Income(
                    transfer.Amount,
                    new TransferIncomeCategory("Входящий перевод между счетами"),
                    new TransactionSource("Внутренний перевод"));

                transfer.MarkAsCompleted(to.Transactions.Last().Id, from.Transactions.Last().Id);
            }
            catch (Exception ex)
            {
                transfer.MarkAsFailed($"Ошибка при переводе: {ex.Message}");
            }
        }
    }
}
