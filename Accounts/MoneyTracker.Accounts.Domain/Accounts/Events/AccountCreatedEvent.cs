using MoneyTracker.SharedKernel;

namespace MoneyTracker.Accounts.Domain.Accounts.Events
{
    /// <summary>
    /// Событие создания счета
    /// </summary>
    public class AccountCreatedEvent : DomainEvent
    {
        /// <summary>
        /// Id счета
        /// </summary>
        public required Guid Id { get; init; }

        /// <summary>
        /// Имя счета
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Начальный баланс
        /// </summary>
        public required decimal InitialBalance { get; init; }

        /// <summary>
        /// Код валюты счета
        /// </summary>
        public required string CurrencyCode { get; init; }
    }
}
