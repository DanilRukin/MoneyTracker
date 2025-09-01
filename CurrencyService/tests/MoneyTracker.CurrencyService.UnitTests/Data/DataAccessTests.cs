using FluentAssertions;
using Microsoft.Identity.Client;
using MoneyTracker.CurrencyService.Data;
using MoneyTracker.CurrencyService.Domain.CurrencyAggregate;
using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate;
using MoneyTracker.CurrencyService.Domain.ExchangeRateEntity;
using MoneyTracker.CurrencyService.Domain.RateSourceEntity;
using System.Runtime.Intrinsics.Wasm;

namespace MoneyTracker.CurrencyService.UnitTests.Data
{
    public class DataAccessTests : BaseEfTestFixture
    {
        [Fact]
        public void ShouldCreateCurrency()
        {
            CurrencyServiceContext context = GetClearContext();
            Currency currency = context.Create("rub", "rub", 'р', true);

            currency.Should().NotBeNull();
            currency.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public void ShouldCreateRateSource()
        {
            CurrencyServiceContext context = GetClearContext();
            RateSource source = context.CreateRateSource("Sber");
            source.Should().NotBeNull();
            source.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public void ShouldCreateCurrensyPair()
        {
            CurrencyServiceContext context = GetClearContext();
            Currency dollar = context.Create("dol", "USA dollar", '$', true);
            Currency ruble = context.Create("rub", "Russian ruble", 'r', true);
            CurrencyPair pair = context.CreatePair(new CurrencyPairBuilder()
                .WithBaseCurrency(dollar)
                .WithTargetCurrency(ruble));

            pair.Should().NotBeNull();
            pair.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public void ShouldCreateExchangeRate()
        {
            CurrencyServiceContext context = GetClearContext();
            Currency dollar = context.Create("dol", "USA dollar", '$', true);
            Currency ruble = context.Create("rub", "Russian ruble", 'r', true);
            CurrencyPair pair = context.CreatePair(new CurrencyPairBuilder()
                .WithBaseCurrency(dollar)
                .WithTargetCurrency(ruble));
            RateSource source = context.CreateRateSource("Sber");
            ExchangeRate rate = context.CreateRate(10, DateTime.Now, pair, source);

            rate.Should().NotBeNull();
            rate.Id.Should().NotBeEmpty().And.NotBe(Guid.Empty);
        }

        [Fact]
        public void CurrencyPairChangesTrakesAutomaticallyWhenCurrencyChanging()
        {
            CurrencyServiceContext context = GetClearContext();
            Currency dollar = context.Create("dol", "USA dollar", '$', true);
            Currency ruble = context.Create("rub", "Russian ruble", 'r', true);
            CurrencyPair pair = context.CreatePair(new CurrencyPairBuilder()
                .WithBaseCurrency(dollar)
                .WithTargetCurrency(ruble));

            pair.IsActive.Should().BeTrue();
            dollar.IsActive.Should().BeTrue();
            ruble.IsActive.Should().BeTrue();

            dollar.Archive();

            pair.IsActive.Should().BeFalse();
            ruble.IsActive.Should().BeTrue();
            dollar.IsActive.Should().BeFalse();

            context.SaveChanges();

            context.Entry(pair).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            CurrencyPair newPair = context.CurrencyPairs.First(p => p.Id == pair.Id);

            newPair.Should().NotBeSameAs(pair);
            newPair.IsActive.Should().BeFalse();
        }

        [Fact]
        public void ExchangeRateRemovesFromDatabaseWhenRemovesFromCurrencyPair()
        {
            CurrencyServiceContext context = GetClearContext();
            Currency dollar = context.Create("dol", "USA dollar", '$', true);
            Currency ruble = context.Create("rub", "Russian ruble", 'r', true);
            CurrencyPair pair = context.CreatePair(new CurrencyPairBuilder()
                .WithBaseCurrency(dollar)
                .WithTargetCurrency(ruble));
            RateSource source = context.CreateRateSource("Sber");
            ExchangeRate rate = context.CreateRate(10, DateTime.Now, pair, source);

            pair.ExchangeRates.Should().NotBeNull().And.Contain(rate);

            pair.RemoveRate(rate);
            pair.ExchangeRates.Should().NotBeNull().And.BeEmpty();
            context.SaveChanges();

            context.Entry(pair).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            CurrencyPair newPair = context.CurrencyPairs.First(p => p.Id == pair.Id);
            newPair.Should().NotBeNull().And.NotBeSameAs(pair);
            newPair.ExchangeRates.Should().NotBeNull().And.BeEmpty();
            context.ExchangeRates.Should().BeEmpty();
        }

        [Fact]
        public void AllRelatedExchangeRatesShouldBeDeletedWhenRateSourceWasDeleted()
        {
            CurrencyServiceContext context = GetClearContext();
            Currency dollar = context.Create("dol", "USA dollar", '$', true);
            Currency ruble = context.Create("rub", "Russian ruble", 'r', true);
            CurrencyPair pair = context.CreatePair(new CurrencyPairBuilder()
                .WithBaseCurrency(dollar)
                .WithTargetCurrency(ruble));
            RateSource sber = context.CreateRateSource("Sber");
            RateSource alpha = context.CreateRateSource("Alpha");
            ExchangeRate sberRate = context.CreateRate(10, DateTime.Now, pair, sber);
            ExchangeRate alphaRate = context.CreateRate(11, DateTime.Now, pair, alpha);

            context.RateSources.Should().Contain(sber).And.Contain(alpha);

            context.RateSources.Remove(sber);
            context.SaveChanges();

            context.RateSources.Should().ContainSingle().And.NotContain(sber);
            context.ExchangeRates.Should().NotContain(sberRate).And.Contain(alphaRate);
        }

        [Fact]
        public void AllRelatedExchangeRatesShouldBeDeletedWhenCurrencyPairWasDeleted()
        {
            CurrencyServiceContext context = GetClearContext();
            Currency dollar = context.Create("dol", "USA dollar", '$', true);
            Currency ruble = context.Create("rub", "Russian ruble", 'r', true);
            CurrencyPair pair = context.CreatePair(new CurrencyPairBuilder()
                .WithBaseCurrency(dollar)
                .WithTargetCurrency(ruble));
            RateSource sber = context.CreateRateSource("Sber");
            RateSource alpha = context.CreateRateSource("Alpha");
            ExchangeRate sberRate = context.CreateRate(10, DateTime.Now, pair, sber);
            ExchangeRate alphaRate = context.CreateRate(11, DateTime.Now, pair, alpha);

            context.ExchangeRates.Should().Contain(sberRate).And.Contain(alphaRate);

            context.CurrencyPairs.Remove(pair);
            context.SaveChanges();

            context.CurrencyPairs.Should().BeEmpty();
            context.ExchangeRates.Should().BeEmpty();
            context.Currencies.Should().Contain(dollar).And.Contain(ruble);
            context.RateSources.Should().Contain(sber).And.Contain(alpha);
        }

        [Fact]
        public void AllCurrencyPairsAndExchnageRatesShouldBeDeletedWhenCurrencyWasDeleted()
        {
            CurrencyServiceContext context = GetClearContext();
            Currency dollar = context.Create("dol", "USA dollar", '$', true);
            Currency ruble = context.Create("rub", "Russian ruble", 'r', true);
            CurrencyPair pair = context.CreatePair(new CurrencyPairBuilder()
                .WithBaseCurrency(dollar)
                .WithTargetCurrency(ruble));
            RateSource sber = context.CreateRateSource("Sber");
            RateSource alpha = context.CreateRateSource("Alpha");
            ExchangeRate sberRate = context.CreateRate(10, DateTime.Now, pair, sber);
            ExchangeRate alphaRate = context.CreateRate(11, DateTime.Now, pair, alpha);

            context.ExchangeRates.Should().Contain(sberRate).And.Contain(alphaRate);
            context.CurrencyPairs.Should().Contain(pair);

            context.Currencies.Remove(dollar);
            context.SaveChanges();

            context.CurrencyPairs.Should().BeEmpty();
            context.ExchangeRates.Should().BeEmpty();
            context.Currencies.Should().Contain(ruble).And.NotContain(dollar);
            context.RateSources.Should().Contain(sber).And.Contain(alpha);
        }
    }
}
