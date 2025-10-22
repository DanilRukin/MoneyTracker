namespace MoneyTracker.Accounts.Tests.UnitTests.Domain
{
    public partial class AccountTests
    {
        public static IEnumerable<object[]> InvalidAccountNames =
            new List<object[]>
            {
                new object[] { string.Empty },
                new object[] { null },
                new object[] { "    " }
            };
    }
}
