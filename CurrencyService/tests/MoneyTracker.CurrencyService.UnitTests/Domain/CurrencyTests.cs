using FluentAssertions;
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
            var pairService = new CurrencyPairService();
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
        }
    }
}
