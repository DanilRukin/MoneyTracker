using MoneyTracker.CurrencyService.Domain.Base;
using MoneyTracker.CurrencyService.Domain.CurrencyAggregate;
using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate.Events;
using MoneyTracker.CurrencyService.Domain.ExchangeRateEntity;
using MoneyTracker.CurrencyService.Domain.Infrastructure.ErrorMessages;
using MoneyTracker.SharedConstants.ErrorCodes;
using SharedKernel.Interfaces;

namespace MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate
{
    /// <summary>
    /// Валютная пара
    /// </summary>
    public class CurrencyPair : CurrencyServiceBaseEntity<int>, IAgregateRoot
    {
        /// <summary>
        /// Целевая валюта (та, в которую переводят)
        /// </summary>
        public Currency TargetCurrency { get; protected set; }

        /// <summary>
        /// Основная валюта (та, из которой переводят)
        /// </summary>
        public Currency BaseCurrency { get; protected set; }

        /// <summary>
        /// Признак активности валютной пары
        /// </summary>
        public bool IsActive { get; protected set; }

        /// <summary>
        /// Связанные курсы валют для этой пары
        /// </summary>
        private List<ExchangeRate> _exchangeRates = new List<ExchangeRate>();

        /// <summary>
        /// Связанные курсы валют для этой пары
        /// </summary>
        public IReadOnlyCollection<ExchangeRate> ExchangeRates => _exchangeRates.AsReadOnly();

        /// <summary>
        /// Текущий курс валют для этой валютной пары (самый актуальный)
        /// </summary>
        public ExchangeRate? ActualRate => _exchangeRates
            .OrderByDescending(rate => rate.RateDate)
            .FirstOrDefault();

        internal CurrencyPair(CurrencyPairBuilder builder)
        {
            ValidateBaseCurrency(builder.BaseCurrency);
            BaseCurrency = builder.BaseCurrency;
            ValidateTargetCurrency(builder.TargetCurrency);
            TargetCurrency = builder.TargetCurrency;
            IsActive = true;
        }


        /// <summary>
        /// Активирует валютнуюу пару. Вызывается только <see cref="Currency"/>
        /// </summary>
        public void Activate()
        {
            ThrowIfDropped();
            if (!IsActive)
            {
                if (!BaseCurrency.IsActive)
                {
                    throw new InvalidOperationException(Errors.CurrencyPair.CanNotActivatePairWhenBaseCurrencyIsArchived);
                }
                if (!TargetCurrency.IsActive)
                {
                    throw new InvalidOperationException(Errors.CurrencyPair.CanNotActivatePairWhenTargetCurrencyIsArchived);
                }
                IsActive = true;
                AddDomainEvent(new CurrencyPairActivatedDomainEvent(Id));
            }
        }

        /// <summary>
        /// Архивирует валютную пару. Вызывается только <see cref="Currency"/>
        /// </summary>
        public void Archive()
        {
            ThrowIfDropped();
            if (IsActive)
            {
                IsActive = false;
                AddDomainEvent(new CurrencyPairArchivedDomainEvent(Id));
            }
        }

        /// <summary>
        /// Добавляет курс для этой валютной пары
        /// </summary>
        /// <param name="rate">курс валютной пары</param>
        public void AddRate(ExchangeRate rate)
        {
            ThrowIfDropped();
            ArgumentNullException.ThrowIfNull(rate);
            if (rate.CurrencyPair != this)
            {
                throw new InvalidOperationException(Errors.CurrencyPair.CanNotAddRateForAnotherPair);
            }
            if (!_exchangeRates.Contains(rate))
            {
                _exchangeRates.Add(rate);
                rate.SetCurrencyPair(this);
            }
        }

        /// <summary>
        /// Проверяет валидность состояния валюты, претендующей стать базовой
        /// </summary>
        private void ValidateBaseCurrency(Currency currency)
        {
            ValidateCurrencyCommonProperties(currency);
            if (TargetCurrency != null && TargetCurrency == currency)
            {
                throw new InvalidOperationException(Errors.CurrencyPair.CanNotSetBaseCurrencySameAsTarget);
            }
        }

        /// <summary>
        /// Проверяет валидность состояния валюты, претендующей стать целевой
        /// </summary>
        private void ValidateTargetCurrency(Currency currency)
        {
            ValidateCurrencyCommonProperties(currency);
            if (BaseCurrency != null && BaseCurrency == currency)
            {
                throw new InvalidOperationException(Errors.CurrencyPair.CanNotSetTargetCurrencySameAsBase);
            }
        }

        /// <summary>
        /// Проверяет общие свойства входящей валюты
        /// </summary>
        private void ValidateCurrencyCommonProperties(Currency currency)
        {
            ArgumentNullException.ThrowIfNull(currency);
            if (!currency.IsActive)
            {
                throw new InvalidOperationException(Errors.CurrencyPair.CanNotCreatePairForArchivedCurrency);
            }
        }

        /// <summary>
        /// Удаляет курс валют пары
        /// </summary>
        public void RemoveRate(ExchangeRate rate)
        {
            ThrowIfDropped();
            if (_exchangeRates.Contains(rate))
            {
                _exchangeRates.Remove(rate);
                rate.Drop();
            }
        }

        protected override void Invalidate()
        {
            TargetCurrency.DeleteCurrencyPair(this);
            BaseCurrency.DeleteCurrencyPair(this);
            TargetCurrency = null;
            BaseCurrency = null;
            isDropped = false;
            while (_exchangeRates.Count > 0)
            {
                _exchangeRates[0].Drop();
            }
            isDropped = true;
        }
    }
}
