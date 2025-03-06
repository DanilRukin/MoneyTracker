using MoneyTracker.CurrencyService.Domain.CurrencyAggregate;
using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate.Events;
using MoneyTracker.CurrencyService.Domain.ExchangeRateEntity;
using SharedKernel;
using SharedKernel.Interfaces;

namespace MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate
{
    /// <summary>
    /// Валютная пара
    /// </summary>
    public class CurrencyPair : EntityBase<int>, IAgregateRoot
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
            ?.OrderByDescending(rate => rate.RateDate)
            ?.FirstOrDefault();

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
        internal void Activate()
        {
            if (!IsActive)
            {
                if (BaseCurrency.IsActive && TargetCurrency.IsActive)
                {
                    IsActive = true;
                    AddDomainEvent(new CurrencyPairActivatedDomainEvent(Id));
                }
            }
        }

        /// <summary>
        /// Архивирует валютную пару. Вызывается только <see cref="Currency"/>
        /// </summary>
        internal void Archive()
        {
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
            _exchangeRates ??= [];
            if (!_exchangeRates.Contains(rate))
            {
                _exchangeRates.Add(rate);
                rate.SetCurrencyPair(this);
            }
        }

        private void ValidateBaseCurrency(Currency currency)
        {
            ValidateCurrencyCommonProperties(currency);
            if (TargetCurrency != null && TargetCurrency == currency)
            {
                throw new InvalidOperationException("Нельзя установить базовую валюту такую же, как целевую!");
            }
        }

        private void ValidateTargetCurrency(Currency currency)
        {
            ValidateCurrencyCommonProperties(currency);
            if (BaseCurrency != null && BaseCurrency == currency)
            {
                throw new InvalidOperationException("Нельзя установить целевую валюту такую же, как базовую!");
            }
        }

        private void ValidateCurrencyCommonProperties(Currency currency)
        {
            if (currency == null)
            {
                throw new ArgumentNullException("Валюта в валютной паре не может быть null");
            }
            if (!currency.IsActive)
            {
                throw new InvalidOperationException($"Невозможно создать валютную пару для архивной валюты! " +
                    $"('{currency.Name}')");
            }
        }

    }
}
