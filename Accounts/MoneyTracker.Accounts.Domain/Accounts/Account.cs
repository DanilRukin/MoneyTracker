using MoneyTracker.Accounts.Domain.Accounts.Events;
using SharedKernel;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyTracker.Accounts.Domain.Transactions;
using MoneyTracker.Accounts.Domain.Categories;

namespace MoneyTracker.Accounts.Domain.Accounts
{
    /// <summary>
    /// Счет
    /// </summary>
    internal class Account : EntityBase<Guid>, IAgregateRoot
    {
        /// <summary>
        /// Название счета
        /// </summary>
        public string Name { get; protected set; } = default!;

        /// <summary>
        /// Текущий баланс счета
        /// </summary>
        public MoneyValue Balance { get; protected set; } = default!;

        /// <summary>
        /// Признак активности счета
        /// </summary>
        public bool IsActive {  get; protected set; }

        /// <summary>
        /// Дата создания счета
        /// </summary>
        public DateTime CreatedAt { get; protected set; }

        /// <summary>
        /// Дата обновления счета
        /// </summary>
        public DateTime UpdatedAt { get; protected set; }

        /// <summary>
        /// Дата закрытия счета
        /// </summary>
        public DateTime? ClosedAt { get; protected set; }

        private List<Transaction> _transactions = new ();
        
        /// <summary>
        /// Транзакции
        /// </summary>
        public virtual IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        protected Account() { }

        /// <summary>
        /// Устанавливает имя счета
        /// </summary>
        /// <param name="name">Имя для счета</param>
        public void SetName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            string oldName = Name ?? string.Empty;
            Name = name;
            SetUpdateDate();
            AddDomainEvent(new AccountNameChangedEvent(oldName, Name));
        }

        /// <summary>
        /// Устанавливает баланс счета
        /// </summary>
        /// <param name="balance">баланс для установки</param>
        private void SetInitialBalance(MoneyValue balance)
        {
            if (balance.Value < 0)
                throw new InvalidOperationException("Баланс не может быть меньше нуля!");
            Balance = balance;
            AddDomainEvent(new BalanceInitializedEvent(this.Id, balance.Value));
            SetUpdateDate();
        }

        public void Income(MoneyValue value, IncomeCategory category, TransactionSource source)
        {
            IncomeTransaction transaction = IncomeTransaction.Create(DateTime.UtcNow, value, category, source, Id);
            Balance = transaction.ApplyBalance(Balance);
            _transactions.Add(transaction);
            AddDomainEvent(new BalanceReplenishedEvent(value.Value, Id));
            SetUpdateDate();
        }

        public void Expense(MoneyValue value, ExpenseCategory category, TransactionSource source)
        {
            if (Balance < value)
                throw new InvalidOperationException($"Невозможно списать больше денег ({value.Value})," +
                                                    $" чем есть на счете ({Balance})");
            ExpenseTransaction transaction = ExpenseTransaction.Create(DateTime.UtcNow, value, category, source, Id);
            Balance = transaction.ApplyBalance(Balance);
            _transactions.Add(transaction);
            AddDomainEvent(new BalanceReducedEvent(Id, value.Value));
            SetUpdateDate();
        }

        private void SetUpdateDate()
        {
            DateTime date = DateTime.UtcNow;
            UpdatedAt = date;
        }

        public static Account Create(string name, MoneyValue initialBalance)
        {
            Account account = new();
            account.Id = Guid.NewGuid();
            account.SetName(name);
            account.SetInitialBalance(initialBalance);

            DateTime date = DateTime.UtcNow;
            account.CreatedAt = date;
            account.UpdatedAt = date;
            account.IsActive = true;

            return account;
        }
    }
}
