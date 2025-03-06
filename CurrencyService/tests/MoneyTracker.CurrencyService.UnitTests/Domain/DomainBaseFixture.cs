using MoneyTracker.CurrencyService.Domain.CurrencyAggregate;
using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate;
using MoneyTracker.CurrencyService.Domain.ExchangeRateEntity;
using MoneyTracker.CurrencyService.Domain.RateSourceEntity;
using MoneyTracker.CurrencyService.UnitTests.Domain.Fakes;

namespace MoneyTracker.CurrencyService.UnitTests.Domain
{
    /// <summary>
    /// Базовые возможности классов тестов доменной области
    /// </summary>
    public abstract class DomainBaseFixture
    {
        private ICurrencyFactory _currencyFactory;
        /// <summary>
        /// Фабрика создания валют
        /// </summary>
        protected ICurrencyFactory CurrencyFactory =>
            _currencyFactory ??= new FakeCurrencyFactory();

        private IRateSourceFactory _rateSourceFactory;
        /// <summary>
        /// Фабрика создания источников курсов валют
        /// </summary>
        protected IRateSourceFactory RateSourceFactory =>
            _rateSourceFactory ??= new FakeRateSourceFactory();

        private IExchangeRateFactory _exchangeRateFactory;
        /// <summary>
        /// Фабрика создания курсов валют
        /// </summary>
        protected IExchangeRateFactory ExchangeRateFactory =>
            _exchangeRateFactory ??= new FakeExchangeRateFactory();

        private ICurrencyPairFactory _currencyPairFactory;
        /// <summary>
        /// Фабрика создания валютных пар
        /// </summary>
        protected ICurrencyPairFactory CurrencyPairFactory =>
            _currencyPairFactory ??= new FakeCurrencyPairFactory();
    }
}
