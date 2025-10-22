using SharedKernel;

namespace MoneyTracker.Accounts.Domain.Accounts.Events
{
    /// <summary>
    /// Событие изменения баланса
    /// </summary>
    internal class BalanceChangedEvent : DomainEvent
    {
        /// <summary>
        /// Предыдущее значение
        /// </summary>
        public decimal OldValue { get; }

        /// <summary>
        /// Текущее значение
        /// </summary>
        public decimal NewValue { get; }

        public BalanceChangedEvent(decimal oldValue, decimal newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
