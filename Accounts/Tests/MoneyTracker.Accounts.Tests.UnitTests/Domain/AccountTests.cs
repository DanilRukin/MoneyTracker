using FluentAssertions;
using MoneyTracker.Accounts.Domain;
using MoneyTracker.Accounts.Domain.Accounts;
using MoneyTracker.Accounts.Domain.Categories;
using MoneyTracker.Accounts.Domain.Transactions;

namespace MoneyTracker.Accounts.Tests.UnitTests.Domain
{
    public partial class AccountTests
    {
        [Theory]
        [MemberData(nameof(InvalidAccountNames))]
        public void NameShouldNotBeNullOrWhiteSpace(string invalidName)
        {
            Action method = () => Account.Create(invalidName, GetRubles(0));

            method.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CanNotExpenseMoreThanHas()
        {
            Account account = Account.Create("name", GetRubles(100));
            ExpenseCategory category = new ExpenseCategory("name");
            TransactionSource source = new TransactionSource("111");

            Action expense = () => account.Expense(GetRubles(150), category, source);

            expense.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void CanNotOperateWithAnotherCurrency()
        {
            Account account = Account.Create("name", GetRubles(100));

            IncomeCategory category = new IncomeCategory("name");
            TransactionSource source = new TransactionSource("111");

            Action income = () => account.Income(GetDollars(150), category, source);

            income.Should().Throw<InvalidOperationException>();
        }


        private static MoneyValue GetRubles(decimal amount)
        {
            return new MoneyValue(amount, new Accounts.Domain.Currencies.Currency("rub", 'r'));
        }

        private static MoneyValue GetDollars(decimal amount)
        {
            return new MoneyValue(amount, new Accounts.Domain.Currencies.Currency("dol", '$'));
        }
    }
}
