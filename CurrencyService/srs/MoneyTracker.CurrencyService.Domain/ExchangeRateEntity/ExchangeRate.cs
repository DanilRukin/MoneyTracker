using MoneyTracker.CurrencyService.Domain.Base;
using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate;
using MoneyTracker.CurrencyService.Domain.ExchangeRateEntity.Events;
using MoneyTracker.CurrencyService.Domain.Infrastructure.ErrorMessages;
using MoneyTracker.CurrencyService.Domain.RateSourceEntity;
using MoneyTracker.SharedConstants.ErrorCodes;
using SharedKernel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace MoneyTracker.CurrencyService.Domain.ExchangeRateEntity
{
    /// <summary>
    /// Курс валют
    /// </summary>
    public class ExchangeRate : CurrencyServiceBaseEntity<Guid>
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
        /// Указывает, является ли сущность удаленной
        /// </summary>
        private bool _isDropped = false; 

        /// <summary>
        /// Источник курса валют
        /// </summary>
        public RateSource RateSource { get; protected set; }

        /// <summary>
        /// EF Core only
        /// </summary>
        [ExcludeFromCodeCoverage]
        protected ExchangeRate() { }

        internal ExchangeRate(ExchangeRateBuilder builder)
        {
            SetCurrencyPair(builder.Pair);
            SetRateSource(builder.RateSource);
            Update(builder.Rate);
            RateDate = builder.Date;
            CurrencyPair.AddRate(this);
        }

        /// <summary>
        /// Обновляет курс валют
        /// </summary>
        /// <param name="rate">Курс</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Update(decimal rate)
        {
            ThrowIfDropped();
            if (CurrencyPair is null)
            {
                throw new InvalidOperationException(Errors.ExchangeRate.CanNotUpdateRateForNullableCurrencyPair);
            }
            if (rate < 0m)
            {
                throw new InvalidOperationException(Errors.ExchangeRate.RateCanNotBeNegative);
            }
            if (!CurrencyPair.IsActive)
            {
                throw new InvalidOperationException(Errors.ExchangeRate.RateCouldBeUpdatedOnlyForActiveCurrencyPair);
            }
            AddDomainEvent(new ExchangeRateUpdatedDomainEvent(Id, Rate, rate));
            Rate = rate;
        }

        /// <summary>
        /// Устанавливает валютную пару
        /// </summary>
        internal void SetCurrencyPair(CurrencyPair currencyPair)
        {
            ArgumentNullException.ThrowIfNull(currencyPair);
            if (!currencyPair.IsActive)
            {
                throw new InvalidOperationException(Errors.ExchangeRate.CanNotSetRateForArchivedPair);
            }
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
            ArgumentNullException.ThrowIfNull(source);
            if (RateSource is null)
            {
                RateSource = source;
            }
        }

        protected override void Invalidate()
        {
            RateSource.RemoveRate(this);
            RateSource = null;
            CurrencyPair.RemoveRate(this);
            CurrencyPair = null;
        }
    }
}
