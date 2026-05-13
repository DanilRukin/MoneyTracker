using MoneyTracker.SharedKernel;

namespace MoneyTracker.Accounts.Domain.Accounts.Events
{
    /// <summary>
    /// Событие изменения имени счета
    /// </summary>
    internal class AccountNameChangedEvent : DomainEvent
    {
        /// <summary>
        /// Старое имя
        /// </summary>
        public string OldName { get; }

        /// <summary>
        /// Новое имя
        /// </summary>
        public string NewName { get; }

        public AccountNameChangedEvent(string oldName, string newName)
        {
            OldName = oldName ?? throw new ArgumentNullException(nameof(oldName));
            NewName = newName ?? throw new ArgumentNullException(nameof(newName));
        }
    }
}
