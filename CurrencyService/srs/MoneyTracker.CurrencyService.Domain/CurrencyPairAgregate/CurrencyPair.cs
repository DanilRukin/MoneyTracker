using MoneyTracker.CurrencyService.Domain.CurrencyAggregate;
using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate.Events;
using MoneyTracker.CurrencyService.Domain.ExchangeRateEntity;
using SharedKernel;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        internal CurrencyPair() { }


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
        /// Устанавливает целевую валюту
        /// </summary>
        internal void SetTargetCurrency(Currency currency)
        {
            if (BaseCurrency != null && BaseCurrency == currency)
            {
                throw new InvalidOperationException("Нельзя установить целевую валюту такую же, как базовую!");
            }
            if (TargetCurrency == null)
            {
                AddDomainEvent(new TargetCurrencyFirstInstalledDomainEvent(Id, currency.Id));
            }
            else
            {
                AddDomainEvent(new TargetCurrencyChangedDomainEvent(Id, TargetCurrency.Id, currency.Id));
            }
            TargetCurrency = currency;
        }

        /// <summary>
        /// Устанавливает базовую валюту
        /// </summary>
        internal void SetBaseCurrency(Currency currency)
        {

            if (TargetCurrency != null && TargetCurrency == currency)
            {
                throw new InvalidOperationException("Нельзя установить базовую валюту такую же, как целевую!");
            }
            if (BaseCurrency == null)
            {
                AddDomainEvent(new BaseCurrencyFirstInstalledDomainEvent(Id, currency.Id));
            }
            else
            {
                AddDomainEvent(new BaseCurrencyChangedDomainEvent(Id, BaseCurrency.Id, currency.Id));
            }
            BaseCurrency = currency;
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
    }
}
