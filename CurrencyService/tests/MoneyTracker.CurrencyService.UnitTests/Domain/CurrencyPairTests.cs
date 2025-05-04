using FluentAssertions;
using MoneyTracker.CurrencyService.Domain.ExchangeRateEntity;
using MoneyTracker.CurrencyService.Domain.Services;
using MoneyTracker.SharedConstants.ErrorCodes;

namespace MoneyTracker.CurrencyService.UnitTests.Domain
{
    public class CurrencyPairTests : DomainBaseFixture
    {
        [Fact]
        public void WhenAddingExchangeRateThenItKnowsAboutPair()
        {
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var source = RateSourceFactory.CreateRateSource("some");
            var pair = pairService.CreatePair(dollar, rub);
            var rate = ExchangeRateFactory.CreateRate(10, DateTime.Now, pair, source);

            pair.ExchangeRates.Should().Contain(rate);
            rate.CurrencyPair.Should().Be(pair);
        }

        [Fact]
        public void CanNotAddTheSameRate()
        {
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var source = RateSourceFactory.CreateRateSource("some");
            var pair = pairService.CreatePair(dollar, rub);
            var rate = ExchangeRateFactory.CreateRate(10, DateTime.Now, pair, source);

            pair.AddRate(rate);
            pair.AddRate(rate);

            pair.ExchangeRates.Should().ContainSingle();
        }

        [Fact]
        public void CanNotAddRateForAnotherPair()
        {
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var euro = CurrencyFactory.Create("euro", "euro", 'e', true);
            var dollarRuble = pairService.CreatePair(dollar, rub);
            var dollarEuro = pairService.CreatePair(dollar, euro);
            var source = RateSourceFactory.CreateRateSource("some");
            var dollarRubleRate = ExchangeRateFactory.CreateRate(10, DateTime.Now, dollarRuble, source);
            var dollarEuroRate = ExchangeRateFactory.CreateRate(11, DateTime.Now, dollarEuro, source);

            var action = () => dollarEuro.AddRate(dollarRubleRate);
            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage(Errors.CurrencyPair.CanNotAddRateForAnotherPair);
        }

        [Fact]
        public void CanNotAddNullableRate()
        {
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);

            var action = () => pair.AddRate(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CanBeActivedOnlyWhenTargetAndBaseCurenciesAreActiveAndItWasArchived()
        {
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);

            pair.Archive();
            pair.IsActive.Should().BeFalse();
            pair.Activate();
            pair.IsActive.Should().BeTrue();

            dollar.Archive();
            pair.IsActive.Should().BeFalse();
            var baseIsArchived = () => pair.Activate();
            baseIsArchived.Should()
                .Throw<InvalidOperationException>()
                .WithMessage(Errors.CurrencyPair.CanNotActivatePairWhenBaseCurrencyIsArchived);
            dollar.Activate();
            pair.Activate();
            pair.IsActive.Should().BeTrue();

            rub.Archive();
            pair.IsActive.Should().BeFalse();
            var targetIsArchived = () => pair.Activate();
            targetIsArchived.Should()
                .Throw<InvalidOperationException>()
                .WithMessage(Errors.CurrencyPair.CanNotActivatePairWhenTargetCurrencyIsArchived);
            rub.Activate();
            pair.Activate();
            pair.IsActive.Should().BeTrue();
        }

        [Fact]
        public void WhenCurrencyActivatesFromArchivedStateThenPairIsStillArchived()
        {
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);

            pair.IsActive.Should().BeTrue();
            rub.Archive();
            pair.IsActive.Should().BeFalse();
            rub.Activate();
            pair.IsActive.Should().BeFalse();
        }

        [Fact]
        public void RemoveExchangeRateCorrectly()
        {
            var source = RateSourceFactory.CreateRateSource("1");
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);
            var exchangeRate = ExchangeRateFactory.CreateRate(10, DateTime.Now, pair, source);

            pair.ExchangeRates.Should().Contain(exchangeRate);
            pair.RemoveRate(exchangeRate);

            pair.ExchangeRates.Should().NotContain(exchangeRate);
            exchangeRate.CurrencyPair.Should().BeNull();
        }

        [Fact]
        public void WhenDroppingThanAllExchangeRatesAreDropping()
        {
            var source = RateSourceFactory.CreateRateSource("1");
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);
            var rates = new List<ExchangeRate>();
            for (int i = 0; i < 10; i++)
            {
                rates.Add(ExchangeRateFactory.CreateRate(10, DateTime.Now, pair, source));
            }

            pair.Drop();
            pair.ExchangeRates.Should().BeEmpty();
            foreach (var rate in rates)
            {
                rate.CurrencyPair.Should().BeNull();
            }
        }

        [Fact]
        public void WhenDroppingThanCurrensiesAreNullAndAreNotContainsIt()
        {
            var source = RateSourceFactory.CreateRateSource("1");
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);
            var rates = new List<ExchangeRate>();
            for (int i = 0; i < 10; i++)
            {
                rates.Add(ExchangeRateFactory.CreateRate(10, DateTime.Now, pair, source));
            }
            pair.Drop();

            pair.TargetCurrency.Should().BeNull();
            pair.BaseCurrency.Should().BeNull();
            dollar.CurrencyPairs.Should().NotContain(pair);
            rub.CurrencyPairs.Should().NotContain(pair);
        }

        [Fact]
        public void NoOneBusinessOperationCouldBeProcessedWhenCurrencyIsInDroppedState()
        {
            var source = RateSourceFactory.CreateRateSource("1");
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);
            var rates = new List<ExchangeRate>();
            for (int i = 0; i < 10; i++)
            {
                rates.Add(ExchangeRateFactory.CreateRate(10, DateTime.Now, pair, source));
            }
            pair.Drop();

            Action[] actions = new Action[]
            {
                () => pair.Activate(),
                () => pair.Archive(),
                () => pair.AddRate(rates[0]),
                () => pair.RemoveRate(rates[0]),
            };

            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].Should()
                    .Throw<InvalidOperationException>()
                    .WithMessage(Errors.Common.CouldNotApplyOperationForDroppedEntity);
            }
        }
    }
}
