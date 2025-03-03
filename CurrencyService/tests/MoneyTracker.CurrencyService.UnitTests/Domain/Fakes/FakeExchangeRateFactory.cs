using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate;
using MoneyTracker.CurrencyService.Domain.ExchangeRateEntity;
using MoneyTracker.CurrencyService.Domain.RateSourceEntity;

namespace MoneyTracker.CurrencyService.UnitTests.Domain.Fakes
{
    /// <summary>
    /// Фейковая реализация фабрики курсов валют
    /// </summary>
    public class FakeExchangeRateFactory : IExchangeRateFactory
    {
        public ExchangeRate CreateRate(decimal rate, DateTime rateDate, CurrencyPair currencyPair, RateSource rateSource)
        {
            var rateBuilder = new ExchangeRateBuilder();
            rateBuilder.WithRate(rate).OnDate(rateDate).ForPair(currencyPair).FromSource(rateSource);
            var exchangeRate = rateBuilder.Build();
            var idProperty = typeof(ExchangeRate).GetProperty(nameof(ExchangeRate.Id));
            if (idProperty is null)
            {
                throw new InvalidOperationException("У сущности обменных курсов не существует свойства Id");
            }
            idProperty.SetValue(exchangeRate, Guid.NewGuid());
            return exchangeRate;
        }
    }
}
