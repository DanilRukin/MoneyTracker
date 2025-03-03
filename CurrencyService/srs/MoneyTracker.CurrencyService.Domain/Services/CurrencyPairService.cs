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
        /// <summary>
        /// Создает валютную пару
        /// </summary>
        /// <param name="baseCurrency">Базовая валюта</param>
        /// <param name="targetCurrency">Целевая валюта</param>
        /// <returns></returns>
        public CurrencyPair CreatePair(Currency baseCurrency, Currency targetCurrency)
        {
            CurrencyPair currencyPair = new CurrencyPair();
            currencyPair.SetBaseCurrency(baseCurrency);
            baseCurrency.AddCurrencyPair(currencyPair);
            currencyPair.SetTargetCurrency(targetCurrency);
            targetCurrency.AddCurrencyPair(currencyPair);
            currencyPair.Activate();
            return currencyPair;
        }
    }
}
