using MoneyTracker.Accounts.Domain.Currencies;
using SharedKernel;
using System.Numerics;

namespace MoneyTracker.Accounts.Domain
{
    internal class MoneyValue : ValueObject
    {
        protected MoneyValue() { }
        public decimal Value { get; private set; }
        public Currency Currency { get; private set; } = null!;

        public MoneyValue(decimal value, Currency currency)
        {
            SetValue(value);
            Currency = currency;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return Currency;
        }

        private void SetValue(decimal value)
        {
            if (value < 0)
            {
                throw new InvalidOperationException("Денежное значение не может быть отрицательным!");
            }
            Value = value;
        }

        private MoneyValue Add(MoneyValue value)
        {
            ValidateCurrency(value);
            return new MoneyValue(value.Value + Value, (Currency)Currency.GetCopy());
        }

        private MoneyValue Subtract(MoneyValue value)
        {
            ValidateCurrency(value);
            return new MoneyValue(Value - value.Value, (Currency)Currency.GetCopy());
        }

        private bool IsMore(MoneyValue value)
        {
            ValidateCurrency(value);
            return Value > value.Value;
        }

        public static MoneyValue operator +(MoneyValue left, MoneyValue right) => left.Add(right);

        public static MoneyValue operator -(MoneyValue left, MoneyValue right) => left.Subtract(right);

        public static bool operator <(MoneyValue left, MoneyValue right) => !left.IsMore(right);

        public static bool operator >(MoneyValue left, MoneyValue right) => left.IsMore(right);

        private void ValidateCurrency(MoneyValue value)
        {
            if (value.Currency != Currency)
                throw new InvalidOperationException("Невозможно выполнить операцию с указанной суммой, т.к. она в другой валюте");
        }
    }
}
