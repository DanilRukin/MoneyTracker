using MoneyTracker.CurrencyService.Domain.CurrencyAggregate;
using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.CurrencyService.Domain.Services
{
    /// <summary>
    /// Сервис валютных пар
    /// </summary>
    public class CurrencyPairService
    {
        private readonly ICurrencyPairFactory _currencyPairFactory;

        public CurrencyPairService(ICurrencyPairFactory currencyPairFactory)
        {
            _currencyPairFactory = currencyPairFactory ?? throw new ArgumentNullException(nameof(currencyPairFactory));
        }



        /// <summary>
        /// Создает валютную пару
        /// </summary>
        /// <param name="baseCurrency">Базовая валюта</param>
        /// <param name="targetCurrency">Целевая валюта</param>
        public CurrencyPair CreatePair(Currency baseCurrency, Currency targetCurrency)
        {
            CurrencyPairBuilder builder = new CurrencyPairBuilder();
            builder.WithBaseCurrency(baseCurrency)
                .WithTargetCurrency(targetCurrency);
            CurrencyPair currencyPair = _currencyPairFactory.CreatePair(builder);
            baseCurrency.AddCurrencyPair(currencyPair);
            targetCurrency.AddCurrencyPair(currencyPair);
            currencyPair.Activate();
            return currencyPair;
        }
    }
}
