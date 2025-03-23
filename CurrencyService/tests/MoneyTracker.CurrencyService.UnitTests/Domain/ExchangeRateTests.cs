using FluentAssertions;
using MoneyTracker.CurrencyService.Domain.ExchangeRateEntity;
using MoneyTracker.CurrencyService.Domain.Infrastructure.ErrorMessages;
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
            var pairService = new CurrencyPairService(CurrencyPairFactory);
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
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);
            decimal rate = 5;
            var exchangeRate = ExchangeRateFactory.CreateRate(5, DateTime.Now, pair, source);
            var action = () => exchangeRate.Update(-5);
            action.Should().Throw<Exception>();
            exchangeRate.Rate.Should().Be(rate);
        }

        [Fact]
        public void CouldNotChangeRateForArchivedCurrencyPair()
        {
            var source = RateSourceFactory.CreateRateSource("1");
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);
  
            var exchangeRate = ExchangeRateFactory.CreateRate(5, DateTime.Now, pair, source);
            pair.Archive();
            var action = () => exchangeRate.Update(15);
            action
                .Should().Throw<InvalidOperationException>()
                .WithMessage(ExchangeRateErrorMessages.RateCouldBeUpdatedOnlyForActiveCurrencyPair);
        }

        [Fact]
        public void CanNotSetNullableRateSource()
        {
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);

            var builder = new ExchangeRateBuilder().WithRate(5).ForPair(pair).FromSource(null);
            var action = () => builder.Build();

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AfterCreatingRateKnowsAboutItSource()
        {
            var source = RateSourceFactory.CreateRateSource("1");
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);
            var exchangeRate = ExchangeRateFactory.CreateRate(5, DateTime.Now, pair, source);
            exchangeRate.RateSource.Should().Be(source);
            source.ExchangeRates.Contains(exchangeRate);
        }

        [Fact]
        public void CouldNotSetNullableCurrencyPair()
        {
            var builder = new ExchangeRateBuilder().WithRate(5).ForPair(null);
            var action = () => builder.Build();

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CouldNotSetArchivedCurrencyPair()
        {
            var source = RateSourceFactory.CreateRateSource("1");
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);
            pair.Archive();
            var action = () => ExchangeRateFactory.CreateRate(5, DateTime.Now, pair, source);
            action
                .Should().Throw<InvalidOperationException>()
                .WithMessage(ExchangeRateErrorMessages.CanNotSetRateForArchivedPair);
        }

        [Fact]
        public void AfterCreatingRateKnowsAboutIsPair()
        {
            var source = RateSourceFactory.CreateRateSource("1");
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);
            var exchangeRate = ExchangeRateFactory.CreateRate(5, DateTime.Now, pair, source);
            exchangeRate.CurrencyPair.Should().Be(pair);
            pair.ExchangeRates.Contains(exchangeRate);
        }
    }
}
