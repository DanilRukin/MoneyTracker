using MoneyTracker.CurrencyService.Domain.CurrencyAggregate;
using MoneyTracker.CurrencyService.Domain.ExchangeRateEntity;
using MoneyTracker.CurrencyService.Domain.RateSourceEntity;
using MoneyTracker.CurrencyService.UnitTests.Domain.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        protected IExchangeRateFactory ExchangeRateFactory =>
            _exchangeRateFactory ??= new FakeExchangeRateFactory();
    }
}
