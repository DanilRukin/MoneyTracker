using FluentAssertions;
using MoneyTracker.Accounts.Domain.Accounts;

namespace MoneyTracker.Accounts.Tests.UnitTests.Domain
{
    public partial class AccountTests
    {
        [Theory]
        [MemberData(nameof(InvalidAccountNames))]
        public void NameShouldNotBeNullOrWhiteSpace(string invalidName)
        {
            Action method = () => Account.Create(invalidName);

            method.Should().Throw<ArgumentNullException>();
        }
    }
}
