using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate;
using MoneyTracker.CurrencyService.Domain.RateSourceEntity;

namespace MoneyTracker.CurrencyService.Domain.ExchangeRateEntity
{
    /// <summary>
    /// Фабрика курсов валют
    /// </summary>
    public interface IExchangeRateFactory
    {
        /// <summary>
        /// Создает курс валют для валютной пары с установленным id
        /// </summary>
        /// <param name="rate">значение курса валют</param>
        /// <param name="rateDate">дата курса валют</param>
        /// <param name="currencyPair">валютная пара, для которой создается курс</param>
        /// <param name="rateSource">источник курса</param>
        /// <returns></returns>
        ExchangeRate CreateRate(decimal rate, DateTime rateDate, CurrencyPair currencyPair, RateSource rateSource);
    }
}
