using FluentAssertions;
using MoneyTracker.CurrencyService.Domain.CurrencyAggregate;
using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate;
using MoneyTracker.CurrencyService.Domain.Infrastructure.ErrorMessages;
using MoneyTracker.CurrencyService.Domain.Services;
using MoneyTracker.SharedConstants.ErrorCodes;

namespace MoneyTracker.CurrencyService.UnitTests.Domain
{
    public class CurrencyPairServiceTests : DomainBaseFixture
    {
        private CurrencyPairService _currencyPairService;
        public CurrencyPairServiceTests() 
        {
            _currencyPairService = new CurrencyPairService(CurrencyPairFactory);
        }
        [Fact]
        public void CanNotCreateCurrencyPairWhenCurrenciesAreSame()
        {
            Currency baseCurrency = CurrencyFactory.Create("1", "1", '1', true);
            Currency targetCurrency = baseCurrency;
            var action = () => _currencyPairService.CreatePair(baseCurrency, targetCurrency);
            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage(Errors.CurrencyPair.CanNotSetTargetCurrencySameAsBase);
        }

        [Fact]
        public void CanNotCreatePairWhenTargetOrBaseIsNullOrBothAreNull()
        {
            Currency baseCurrency = CurrencyFactory.Create("1", "1", '1', true);
            Currency targetCurrency = CurrencyFactory.Create("2", "2", '2', true);
            var targetIsNull = () => _currencyPairService.CreatePair(baseCurrency, null);
            var baseIsNull = () => _currencyPairService.CreatePair(null, targetCurrency);
            var bothAreNull = () => _currencyPairService.CreatePair(null, null);
            targetIsNull.Should().Throw<ArgumentException>();
            baseIsNull.Should().Throw<ArgumentException>();
            bothAreNull.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CurrenciesAreKnowsAboutPairAfterItsCreation()
        {
            Currency baseCurrency = CurrencyFactory.Create("1", "1", '1', true);
            Currency targetCurrency = CurrencyFactory.Create("2", "2", '2', true);
            CurrencyPair pair = _currencyPairService.CreatePair(baseCurrency, targetCurrency);
            baseCurrency.CurrencyPairs.Should().Contain(pair);
            targetCurrency.CurrencyPairs.Should().Contain(pair);
        }

        [Fact]
        public void CreatedCurrencyPairIsNotNull()
        {
            Currency baseCurrency = CurrencyFactory.Create("1", "1", '1', true);
            Currency targetCurrency = CurrencyFactory.Create("2", "2", '2', true);
            CurrencyPair pair = _currencyPairService.CreatePair(baseCurrency, targetCurrency);
            pair.Should().NotBeNull();
        }

        [Fact]
        public void CreatedCurrencyPairHasPositiveId()
        {
            Currency baseCurrency = CurrencyFactory.Create("1", "1", '1', true);
            Currency targetCurrency = CurrencyFactory.Create("2", "2", '2', true);
            CurrencyPair pair = _currencyPairService.CreatePair(baseCurrency, targetCurrency);
            pair.Id.Should().BePositive();
        }

        [Fact]
        public void SourceCurrenciesAreSameIntoThePair()
        {
            Currency baseCurrency = CurrencyFactory.Create("1", "1", '1', true);
            Currency targetCurrency = CurrencyFactory.Create("2", "2", '2', true);
            CurrencyPair pair = _currencyPairService.CreatePair(baseCurrency, targetCurrency);
            pair.BaseCurrency.Should().Be(baseCurrency);
            pair.TargetCurrency.Should().Be(targetCurrency);
        }

        [Fact]
        public void CanNotCreateServiceInstanceWithoutFactory()
        {
            CurrencyPairService service;
            var action = () => service = new CurrencyPairService(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CanNotCreatePairForArchivedCurrency()
        {
            Currency baseCurrency = CurrencyFactory.Create("1", "1", '1', false);
            Currency targetCurrency = CurrencyFactory.Create("2", "2", '2', true);
            var action = () => _currencyPairService.CreatePair(baseCurrency, targetCurrency);
            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage(Errors.CurrencyPair.CanNotCreatePairForArchivedCurrency);
        }
    }
}
