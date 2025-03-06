using MoneyTracker.CurrencyService.Domain.CurrencyAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate
{
    public class CurrencyPairBuilder
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
        /// Устанавливает базовую валюту
        /// </summary>
        /// <param name="baseCurrency"></param>
        /// <returns></returns>
        public CurrencyPairBuilder WithBaseCurrency(Currency baseCurrency)
        {
            BaseCurrency = baseCurrency;
            return this;
        }

        /// <summary>
        /// Устанавливает целевую валюту
        /// </summary>
        /// <param name="targetCurrency"></param>
        /// <returns></returns>
        public CurrencyPairBuilder WithTargetCurrency(Currency targetCurrency)
        {
            TargetCurrency = targetCurrency;
            return this;
        }

        /// <summary>
        /// Создает валютную пару (заготовку, без id)
        /// </summary>
        /// <returns></returns>
        public CurrencyPair Build()
        {
            CurrencyPair currencyPair = new CurrencyPair(this);
            return currencyPair;
        }
    }
}
