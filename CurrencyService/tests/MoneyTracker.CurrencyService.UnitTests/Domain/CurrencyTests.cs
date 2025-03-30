using FluentAssertions;
using MoneyTracker.CurrencyService.Domain.CurrencyAggregate.Events;
using MoneyTracker.CurrencyService.Domain.Infrastructure.ErrorMessages;
using MoneyTracker.CurrencyService.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.CurrencyService.UnitTests.Domain
{
    public class CurrencyTests : DomainBaseFixture
    {
        [Fact]
        public void WhenArchivedMustArchiveAllCurrencyPairs()
        {
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var euro = CurrencyFactory.Create("euro", "euro", 'e', true);
            var dollarRuble = pairService.CreatePair(dollar, rub);
            var dollarEuro = pairService.CreatePair(dollar, euro);
            var rubleDollar = pairService.CreatePair(rub, dollar);
            var rubleEuro = pairService.CreatePair(rub, euro);
            var euroDollar = pairService.CreatePair(euro, dollar);
            var euroRuble = pairService.CreatePair(euro, rub);

            rub.Archive();

            rub.IsActive.Should().BeFalse();
            rubleDollar.IsActive.Should().BeFalse();
            rubleEuro.IsActive.Should().BeFalse();
            dollarRuble.IsActive.Should().BeFalse();
            euroRuble.IsActive.Should().BeFalse();

            dollarEuro.IsActive.Should().BeTrue();
            euroDollar.IsActive.Should().BeTrue();
        }

        [Fact]
        public void CurrencyKnowAboutPairsWhereItIsTarget()
        {
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var euro = CurrencyFactory.Create("euro", "euro", 'e', true);

            var dollarRuble = pairService.CreatePair(dollar, rub);
            var dollarEuro = pairService.CreatePair(dollar, euro);
            var rubleDollar = pairService.CreatePair(rub, dollar);
            var rubleEuro = pairService.CreatePair(rub, euro);
            var euroDollar = pairService.CreatePair(euro, dollar);
            var euroRuble = pairService.CreatePair(euro, rub);

            rub.OnThesePairsIsTarget
                .Should().NotBeNullOrEmpty()
                .And.Contain(euroRuble)
                .And.Contain(dollarRuble)
                .And.NotContain(rubleEuro)
                .And.NotContain(rubleDollar);

            dollar.OnThesePairsIsTarget
                .Should().NotBeNullOrEmpty()
                .And.Contain(rubleDollar)
                .And.Contain(euroDollar)
                .And.NotContain(dollarEuro)
                .And.NotContain(dollarRuble);

            euro.OnThesePairsIsTarget
                .Should().NotBeNullOrEmpty()
                .And.Contain(rubleEuro)
                .And.Contain(dollarEuro)
                .And.NotContain(euroRuble)
                .And.NotContain(euroDollar);
        }

        [Fact]
        public void CurrencyKnowAboutPairsWhereItIsBase()
        {
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var euro = CurrencyFactory.Create("euro", "euro", 'e', true);

            var dollarRuble = pairService.CreatePair(dollar, rub);
            var dollarEuro = pairService.CreatePair(dollar, euro);
            var rubleDollar = pairService.CreatePair(rub, dollar);
            var rubleEuro = pairService.CreatePair(rub, euro);
            var euroDollar = pairService.CreatePair(euro, dollar);
            var euroRuble = pairService.CreatePair(euro, rub);

            rub.OnThesePairsIsBase
                .Should().NotBeNullOrEmpty()
                .And.Contain(rubleDollar)
                .And.Contain(rubleEuro)
                .And.NotContain(dollarRuble)
                .And.NotContain(euroRuble);

            dollar.OnThesePairsIsBase
                .Should().NotBeNullOrEmpty()
                .And.Contain(dollarRuble)
                .And.Contain(dollarEuro)
                .And.NotContain(rubleDollar)
                .And.NotContain(rubleEuro);

            euro.OnThesePairsIsBase
                .Should().NotBeNullOrEmpty()
                .And.Contain(euroRuble)
                .And.Contain(euroDollar)
                .And.NotContain(rubleDollar)
                .And.NotContain(rubleEuro);
        }

        [Fact]
        public void WhenActivatingDomainEventAboutItIsCreating()
        {
            var rub = CurrencyFactory.Create("rub", "Российский рубль", 'Р', false);
            rub.Activate();
            rub.IsActive.Should().BeTrue();
            rub.DomainEvents
                .Should().NotBeNullOrEmpty()
                .And.ContainItemsAssignableTo<CurrencyActivatedDomainEvent>();
        }

        [Fact]
        public void WhenArchivingDomainEventAboutItIsCreating()
        {
            var rub = CurrencyFactory.Create("rub", "Российский рубль", 'Р', true);
            rub.Archive();
            rub.IsActive.Should().BeFalse();
            rub.DomainEvents
                .Should().NotBeNullOrEmpty()
                .And.ContainItemsAssignableTo<CurrencyArchivedDomainEvent>();
        }

        [Fact]
        public void CanNotAddNullableCurrencyPair()
        {
            var rub = CurrencyFactory.Create("rub", "Российский рубль", 'Р', true);
            var action = () => rub.AddCurrencyPair(null);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CanNotAddPairOfOtherCurrencies()
        {
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var euro = CurrencyFactory.Create("euro", "euro", 'e', true);

            var dollarRuble = pairService.CreatePair(dollar, rub);

            var action = () => euro.AddCurrencyPair(dollarRuble);

            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage(CurrencyErrorMessages.ThisCurrencyPairDoesNotBelongToCurrency);
        }

        [Fact]
        public void RemoveCurrencyPairCorrectly()
        {
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);

            var dollarRuble = pairService.CreatePair(dollar, rub);
            var rubleDollar = pairService.CreatePair(rub, dollar);
            rub.DeleteCurrencyPair(dollarRuble);

            rub.CurrencyPairs.Should().NotContain(dollarRuble);
            dollarRuble.TargetCurrency.Should().BeNull();
            dollarRuble.BaseCurrency.Should().BeNull();
            dollar.CurrencyPairs.Should().NotContain(dollarRuble);

            rubleDollar.BaseCurrency.Should().Be(rub);
            rubleDollar.TargetCurrency.Should().Be(dollar);
            rub.CurrencyPairs.Should().Contain(rubleDollar);
            dollar.CurrencyPairs.Should().Contain(rubleDollar);
        }

        [Fact]
        public void WhenDroppingCurrencyThanAllDependentCurrencyPairsDropping()
        {
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);
            var euro = CurrencyFactory.Create("euro", "euro", 'e', true);

            var dollarRuble = pairService.CreatePair(dollar, rub);
            var rubleDollar = pairService.CreatePair(rub, dollar);
            var euroDollar = pairService.CreatePair(euro, dollar);
            rub.Drop();

            rub.CurrencyPairs.Should().BeEmpty();
            dollar.CurrencyPairs
                .Should().NotContain(dollarRuble)
                .And.NotContain(rubleDollar)
                .And.Contain(euroDollar);
            dollarRuble.TargetCurrency.Should().BeNull();
            dollarRuble.BaseCurrency.Should().BeNull();
            rubleDollar.BaseCurrency.Should().BeNull();
            rubleDollar.TargetCurrency.Should().BeNull();
            euroDollar.BaseCurrency.Should().Be(euro);
            euroDollar.TargetCurrency.Should().Be(dollar);
        }

        [Fact]
        public void NoOneBusinessOperationCouldBeProcessedWhenCurrencyIsInDroppedState()
        {
            var pairService = new CurrencyPairService(CurrencyPairFactory);
            var dollar = CurrencyFactory.Create("dol", "usa dollar", '$', true);
            var rub = CurrencyFactory.Create("rub", "russian ruble", 'Р', true);

            var dollarRuble = pairService.CreatePair(dollar, rub);
            var rubleDollar = pairService.CreatePair(rub, dollar);
            rub.Drop();

            var actions = new Action[]
            {
                () => rub.DeleteCurrencyPair(rubleDollar),
                () => rub.Activate(),
                () => rub.Archive(),
                () => rub.AddCurrencyPair(rubleDollar),
            };

            foreach (var action in actions)
            {
                action.Should()
                    .Throw<InvalidOperationException>()
                    .WithMessage(CommonErrorMessages.CouldNotApplyOperationForDroppedEntity);
            } 
        }
    }
}
