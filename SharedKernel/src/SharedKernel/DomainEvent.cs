using MediatR;

namespace SharedKernel
{
    public class DomainEvent : INotification
    {
        public DateTime DateOccured { get; protected set; } = DateTime.UtcNow;
    }
}
