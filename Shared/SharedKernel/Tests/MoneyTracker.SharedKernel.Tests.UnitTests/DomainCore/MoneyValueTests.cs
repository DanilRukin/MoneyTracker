using FluentAssertions;
using MoneyTracker.SharedKernel.DomainCore;

namespace MoneyTracker.Accounts.Tests.UnitTests.Domain
{
    public class MoneyValueTests
    {
        [Fact]
        public void ComparingShouldWorkCorrectly()
        {
            MoneyValue first = GetRubles(100);
            MoneyValue second = GetRubles(200);
            MoneyValue third = GetRubles(100);

            bool isMore = first > second;
            bool isLess = first < second;
            bool isEqual = first == third;

            isMore.Should().BeFalse();
            isLess.Should().BeTrue();
            isEqual.Should().BeTrue();
        }

        [Fact]
        public void ShouldAddCorrectly()
        {
            MoneyValue first = GetRubles(100);
            MoneyValue second = GetRubles(200);

            MoneyValue expected = GetRubles(300);
            MoneyValue result = first + second;

            result.Should().Be(expected);
        }

        [Fact]
        public void ShouldSubtractCorrectly()
        {
            MoneyValue first = GetRubles(200);
            MoneyValue second = GetRubles(100);

            MoneyValue expected = GetRubles(100);
            MoneyValue result = first - second;

            result.Should().Be(expected);
        }

        [Fact]
        public void CanNotCreateNegative()
        {
            Action create = () => GetRubles(-100);
            create.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void CanCreateZero()
        {
            Func<MoneyValue> create = () => GetRubles(0);
            create.Should().NotThrow<InvalidOperationException>();
            decimal value = create.Invoke().Value;
            value.Should().Be(0m);
        }

        [Fact]
        public void CanNotSubtractMoreThanHas()
        {
            MoneyValue first = GetRubles(200);
            MoneyValue second = GetRubles(300);

            Func<MoneyValue> sub = () => first - second;

            sub.Should().Throw<InvalidOperationException>();
        }

        private MoneyValue GetRubles(decimal amount)
        {
            return new MoneyValue(amount, "rub");
        }
    }
}
