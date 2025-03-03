using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate;
using MoneyTracker.CurrencyService.Domain.ExchangeRateEntity.Events;
using MoneyTracker.CurrencyService.Domain.RateSourceEntity;
using SharedKernel;

namespace MoneyTracker.CurrencyService.Domain.ExchangeRateEntity
{
    /// <summary>
    /// Курс валют
    /// </summary>
    public class ExchangeRate : EntityBase<Guid>
    {
        /// <summary>
        /// Сам обменный курс
        /// </summary>
        public decimal Rate { get; protected set; }

        /// <summary>
        /// Дата актуальности курса
        /// </summary>
        public DateTime RateDate { get; protected set; }

        /// <summary>
        /// Валютная пара, для которой актуален курс
        /// </summary>
        public CurrencyPair CurrencyPair { get; protected set; }

        /// <summary>
        /// Источник курса валют
        /// </summary>
        public RateSource RateSource { get; protected set; }

        internal ExchangeRate(ExchangeRateBuilder builder)
        {
            this.CurrencyPair = builder.Pair;
            this.RateSource = builder.RateSource;
            Update(builder.Rate);
            RateDate = builder.Date;
        }

        /// <summary>
        /// Обновляет курс валют
        /// </summary>
        /// <param name="rate">Курс</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Update(decimal rate)
        {
            if (CurrencyPair is null)
            {
                throw new InvalidOperationException("Нельзя обновить курс для неустановленной валютной пары");
            }
            if (rate < 0m)
            {
                throw new InvalidOperationException($"Курс не может быть меньше нуля. " +
                    $"Предоставленный курс = {rate}");
            }
            if (!CurrencyPair.IsActive)
            {
                throw new InvalidOperationException("Курс валют можно обновить только для активной валютной пары");
            }
            AddDomainEvent(new ExchangeRateUpdatedDomainEvent(Id, Rate, rate));
            Rate = rate;
        }

        /// <summary>
        /// Устанавливает валютную пару
        /// </summary>
        internal void SetCurrencyPair(CurrencyPair currencyPair)
        {
            if (CurrencyPair is null)
            {
                CurrencyPair = currencyPair;
            }
        }

        /// <summary>
        /// Устанавливает источник данных, из которых был взят курс
        /// </summary>
        internal void SetRateSource(RateSource source)
        {
            if (source is null)
            {
                throw new ArgumentNullException("Источник данных курса не может быть пустым");
            }
            if (RateSource is null)
            {
                RateSource = source;
            }
        }
    }
}
