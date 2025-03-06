using SharedKernel;

namespace MoneyTracker.CurrencyService.Domain.RateSourceEntity.Events
{
    /// <summary>
    /// Событие изменения наименования источника курса валют
    /// </summary>
    public class RateSourceNameChangedDomainEvent : DomainEvent
    {
        /// <summary>
        /// Id источника
        /// </summary>
        public int RateSourceId { get; }

        /// <summary>
        /// Старое значение
        /// </summary>
        public string OldName { get; }

        /// <summary>
        /// Новое значение
        /// </summary>
        public string NewName { get; }

        public RateSourceNameChangedDomainEvent(int rateSourceId, string oldName, string newName)
        {
            RateSourceId = rateSourceId;
            OldName = oldName ?? throw new ArgumentNullException(nameof(oldName));
            NewName = newName ?? throw new ArgumentNullException(nameof(newName));
        }
    }
}
