using SharedKernel;

namespace MoneyTracker.Accounts.Domain.Transactions
{
    /// <summary>
    /// Источник появления транзакции (импорт операций из банка, перевод между счетами, простое зачисление...ё)
    /// </summary>
    internal class TransactionSource : IdentifiedValueObject<int>
    {
        public string Name { get; private set; } = default!;
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }

        public TransactionSource(string name)
        {
            SetName(name);
        }

        private void SetName(string name)
        {
            ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
            Name = name;
        }

        protected TransactionSource() { }
    }
}
