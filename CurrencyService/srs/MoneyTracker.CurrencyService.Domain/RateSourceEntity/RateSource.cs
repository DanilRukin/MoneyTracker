using MoneyTracker.CurrencyService.Domain.ExchangeRateEntity;
using MoneyTracker.CurrencyService.Domain.RateSourceEntity.Events;
using SharedKernel;

namespace MoneyTracker.CurrencyService.Domain.RateSourceEntity
{
    /// <summary>
    /// Источник курса валют
    /// </summary>
    public class RateSource : EntityBase<int>
    {
        /// <summary>
        /// Наименование источника
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Обменные курсы, взятые из этого источника
        /// </summary>
        private List<ExchangeRate> _exchangeRates = new List<ExchangeRate>();

        /// <summary>
        /// Обменные курсы, взятые из этого источника
        /// </summary>
        public IReadOnlyCollection<ExchangeRate> ExchangeRates => _exchangeRates.AsReadOnly();

        public RateSource(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Изменяет наименование источника курса валют
        /// </summary>
        /// <param name="name">Новое имя источника</param>
        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Нельзя присвоить пустое имя источнику курса валют");
            }
            AddDomainEvent(new RateSourceNameChangedDomainEvent(Id, Name, name));
            Name = name;
        }
    }
}
