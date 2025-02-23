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

        public CurrencyPair() { }


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
    }
}
