using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions;
using MoneyTracker.CurrencyService.Domain.ExchangeRateEntity;
using MoneyTracker.CurrencyService.Domain.Infrastructure.ErrorMessages;
using MoneyTracker.CurrencyService.Domain.RateSourceEntity;
using MoneyTracker.CurrencyService.Domain.RateSourceEntity.Events;
using MoneyTracker.CurrencyService.Domain.Services;
using MoneyTracker.SharedConstants.ErrorCodes;

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
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);
            var exchangeRate = ExchangeRateFactory.CreateRate(10, DateTime.Now, pair, source);
            source.AddRate(exchangeRate);

            exchangeRate.RateSource.Should().NotBeNull();
            source.ExchangeRates.Should().Contain(exchangeRate);
            exchangeRate.RateSource.Should().Be(source);
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
            source.AddRate(exchangeRate);

            var anotherRate = ExchangeRateFactory.CreateRate(10, DateTime.Now, pair, source);
            source.AddRate(anotherRate);

            source.RemoveRate(exchangeRate);
            source.ExchangeRates.Should().NotContain(exchangeRate);
            source.ExchangeRates.Should().NotBeEmpty().And.Contain(anotherRate);

            exchangeRate.RateSource.Should().BeNull();
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

        [Fact]
        public void NameIsChangingAndDomainEventAddingWhenNameIsValid()
        {
            string validName = "valid name";
            string anotherValidName = "another valid name";
            var source = RateSourceFactory.CreateRateSource(validName);
            source.ChangeName(anotherValidName);

            source.Name.Should().Be(anotherValidName);
            source.DomainEvents
                .Should().NotBeNullOrEmpty()
                .And.ContainItemsAssignableTo<RateSourceNameChangedDomainEvent>();
        }

        [Fact]
        public void WhenDroppingThanAllDependentExchangeRatesAreDropping()
        {
            var source = RateSourceFactory.CreateRateSource("1");
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);
            var rates = new List<ExchangeRate>();
            int count = 10;
            for (int i = 0; i < count; i++)
            {
                var rate = ExchangeRateFactory.CreateRate(10, DateTime.Now, pair, source);
                rates.Add(rate);
                source.AddRate(rate);
            }
            source.ExchangeRates.Should().HaveCount(count);

            source.Drop();

            source.ExchangeRates.Should().BeEmpty();
            foreach (var rate in rates)
            {
                rate.RateSource.Should().BeNull();
            }
        }

        [Fact]
        public void NoOneBusinessOperationCouldBeProcessedWhenCurrencyIsInDroppedState()
        {
            var source = RateSourceFactory.CreateRateSource("1");
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var pair = pairService.CreatePair(dollar, rub);
            var exchangeRate = ExchangeRateFactory.CreateRate(10, DateTime.Now, pair, source);
            var rates = new List<ExchangeRate>();
            int count = 10;
            for (int i = 0; i < count; i++)
            {
                rates.Add(ExchangeRateFactory.CreateRate(10, DateTime.Now, pair, source));
            }

            source.Drop();

            Action[] actions = new Action[]
            {
                () => source.AddRate(rates[0]),
                () => source.ChangeName("name"),
                () => source.RemoveRate(rates[0]),
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
