using SharedKernel;

namespace MoneyTracker.Accounts.Domain.Accounts.Events
{
    /// <summary>
    /// Событие изменения ежемесячного обслуживания
    /// </summary>
    internal class MonthlyMaintenanceFeeChangedEvent: DomainEvent
    {
        /// <summary>
        /// Предыдущее значение
        /// </summary>
        public decimal OldValue { get; }

        /// <summary>
        /// Текущее значение
        /// </summary>
        public decimal NewValue { get; }

        public MonthlyMaintenanceFeeChangedEvent(decimal oldValue, decimal newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
