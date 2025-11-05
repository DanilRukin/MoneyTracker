using MoneyTracker.Accounts.Domain.Accounts;
using MoneyTracker.Accounts.Domain.Categories;
using MoneyTracker.Accounts.Domain.Transactions;

namespace MoneyTracker.Accounts.Domain.Services
{
    internal class TransferService
    {
        public void Transfer(Account from, Account to, MoneyValue amount)
        {
            if (from == to)
                throw new InvalidOperationException("Нельзя переводить на тот же счет!");
            from.Expense(amount, new TransferExpenseCategory(), new TransferTransactionSource());
            to.Income(amount, new TransferIncomeCategory(), new TransferTransactionSource());
        }
    }
}
