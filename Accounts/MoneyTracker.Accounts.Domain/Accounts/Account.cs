using MoneyTracker.Accounts.Domain.Accounts.Events;
using SharedKernel;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Name { get; protected set; }

        /// <summary>
        /// Текущий баланс счета
        /// </summary>
        public decimal Balance { get; protected set; }

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

        /// <summary>
        /// Сумма ежемесячного обслуживания счета
        /// </summary>
        public decimal MonthlyMaintenanceFee { get; protected set; }

        protected Account() { }

        /// <summary>
        /// Устанавливает имя счета
        /// </summary>
        /// <param name="name">Имя для счета</param>
        public void SetName(string name)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace("Названи счета не может быть пустым!");
            string oldName = Name ?? string.Empty;
            Name = name;
            SetUpdateDate();
            AddDomainEvent(new AccountNameChangedEvent(oldName, Name));
        }

        /// <summary>
        /// Устанавливает баланс счета
        /// </summary>
        /// <param name="balance">баланс для установки</param>
        public void SetBalance(decimal balance)
        {
            if (balance < 0)
                throw new InvalidOperationException("Баланс не может быть меньше нуля!");
            decimal oldBalance = Balance;
            Balance = balance;
            AddDomainEvent(new BalanceChangedEvent(oldBalance, Balance));
            SetUpdateDate();
        }

        /// <summary>
        /// Устанавливает ежемесячное обслуживание счета
        /// </summary>
        /// <param name="value">Стоимость обслуживания</param>
        public void SetMonthlyMaintenanceFee(decimal value)
        {
            if (value < 0)
                throw new InvalidOperationException("Ежемесячное обслуживание не может быть меньше нуля!");
            decimal oldMonthlyMaintenanceFee = MonthlyMaintenanceFee;
            MonthlyMaintenanceFee = value;
            AddDomainEvent(new MonthlyMaintenanceFeeChangedEvent(oldMonthlyMaintenanceFee, MonthlyMaintenanceFee));
            SetUpdateDate();
        }

        private void SetUpdateDate()
        {
            DateTime date = DateTime.UtcNow;
            UpdatedAt = date;
        }

        public static Account Create(string name, decimal initialBalance = 0, decimal monthlyMaintenanceFee = 0)
        {
            Account account = new();
            account.Id = Guid.NewGuid();
            account.SetName(name);
            account.SetBalance(initialBalance);
            account.SetMonthlyMaintenanceFee(monthlyMaintenanceFee);

            DateTime date = DateTime.UtcNow;
            account.CreatedAt = date;
            account.UpdatedAt = date;
            account.IsActive = true;
            account.Balance = 0;


            return account;
        }
    }
}
