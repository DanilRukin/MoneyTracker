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
            ThrowWhenNameIsInvalid(name);
            Name = name;
        }

        /// <summary>
        /// Изменяет наименование источника курса валют
        /// </summary>
        /// <param name="name">Новое имя источника</param>
        public void ChangeName(string name)
        {
            ThrowWhenNameIsInvalid(name);
            AddDomainEvent(new RateSourceNameChangedDomainEvent(Id, Name, name));
            Name = name;
        }

        /// <summary>
        /// Выбрасывает исключение, если имя не соответствует требованиям
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="ArgumentException"></exception>
        private void ThrowWhenNameIsInvalid(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Нельзя присвоить пустое имя источнику курса валют");
            }
            if (name.StartsWith(' '))
            {
                throw new ArgumentException("Имя не должно начинаться с символа пробела");
            }
            if (name.EndsWith(' '))
            {
                throw new ArgumentException("Имя не может заканчиваться символом пробела");
            }
        }

        /// <summary>
        /// Добавляет курс валют, которых был установлен из этого источника
        /// </summary>
        public void AddRate(ExchangeRate rate)
        {
            _exchangeRates ??= [];
            if (!_exchangeRates.Contains(rate))
            {
                _exchangeRates.Add(rate);
                rate.SetRateSource(this);
            }
        }

        /// <summary>
        /// Удаляет обменный курс этого источника
        /// </summary>
        /// <param name="rate"></param>
        public void RemoveRate(ExchangeRate rate)
        {
            _exchangeRates.Remove(rate);
        }
    }
}
