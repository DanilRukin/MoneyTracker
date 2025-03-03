using FluentAssertions;
using MoneyTracker.CurrencyService.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.CurrencyService.UnitTests.Domain
{
    public class ExchangeRateTests : DomainBaseFixture
    {
        [Fact]
        public void RateCanNotBeNegativeWhenCreating()
        {
            var source = RateSourceFactory.CreateRateSource("1");
            var pairService = new CurrencyPairService();
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);
            var action = () => ExchangeRateFactory.CreateRate(-5, DateTime.Now, pair, source);
            action.Should().Throw<Exception>();
        }

        [Fact]
        public void CouldNotChangeRateOnNegative()
        {
            var source = RateSourceFactory.CreateRateSource("1");
            var pairService = new CurrencyPairService();
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);
            decimal rate = 5;
            var exchangeRate = ExchangeRateFactory.CreateRate(5, DateTime.Now, pair, source);
            var action = () => exchangeRate.Update(-5);
            action.Should().Throw<Exception>();
            exchangeRate.Rate.Should().Be(rate);
        }
    }
}
