using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MoneyTracker.CurrencyService.Domain.RateSourceEntity;
using MoneyTracker.CurrencyService.Domain.Services;

namespace MoneyTracker.CurrencyService.UnitTests.Domain
{
    /// <summary>
    /// Тесты для <see cref="RateSource"/>
    /// </summary>
    public partial class RateSourceTests : DomainBaseFixture
    {
        [Fact]
        public void WhenAddExchangeRateThanItKnowsAboutSource()
        {
            var source = RateSourceFactory.CreateRateSource("1");
            var pairService = new CurrencyPairService();
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);
            var exchangeRate = ExchangeRateFactory.CreateRate(10, DateTime.Now, pair, source);
            source.AddRate(exchangeRate);

            exchangeRate.RateSource.Should().NotBeNull();
            source.ExchangeRates.Should().Contain(exchangeRate);
        }

        [Fact]
        public void RemoveExchangeRateCorrectly()
        {
            var source = RateSourceFactory.CreateRateSource("1");
            var pairService = new CurrencyPairService();
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);
            var exchangeRate = ExchangeRateFactory.CreateRate(10, DateTime.Now, pair, source);
            source.AddRate(exchangeRate);

            var anotherRate = ExchangeRateFactory.CreateRate(10, DateTime.Now, pair, source);
            source.AddRate(anotherRate);

            source.RemoveRate(exchangeRate);
            source.ExchangeRates.Should().NotContain(exchangeRate);
            source.ExchangeRates.Should().NotBeEmpty().And.Contain(anotherRate);
        }

        [Theory]
        [MemberData(nameof(InvalidNames))]
        public void CanNotSetInvalidNameOnCreating(string name)
        {
            var action = () => RateSourceFactory.CreateRateSource(name);
            action.Should().Throw<Exception>();
        }

        [Theory]
        [MemberData(nameof(InvalidNames))]
        public void CanNotChangeValidNameOnInvalid(string name)
        {
            string validName = "valid name";
            var source = RateSourceFactory.CreateRateSource(validName);
            var action = () => source.ChangeName(name);
            action.Should().Throw<Exception>();
            source.Name.Should().Be(validName);
        }
    }
}
