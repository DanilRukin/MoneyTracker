using MediatR;

namespace MoneyTracker.SharedKernel
{
    /// <summary>
    /// Доменное событие
    /// </summary>
    public class DomainEvent : INotification
    {
        /// <summary>
        /// Время наступления события
        /// </summary>
        public DateTime DateOccured { get; protected set; } = DateTime.UtcNow;
    }
}
