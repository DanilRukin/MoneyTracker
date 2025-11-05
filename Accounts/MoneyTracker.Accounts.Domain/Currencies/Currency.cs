using SharedKernel;

namespace MoneyTracker.Accounts.Domain.Currencies
{
    internal class Currency : IdentifiedValueObject<int>
    {
        public string Name { get; private set; } = default!;
        public char Symbol { get; private set; } = default!;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Symbol;
        }

        public Currency(string name, char symbol)
        {
            SetName(name);
            SetSymbol(symbol);
        }

        protected Currency() { }

        private void SetName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            Name = name;
        }

        private void SetSymbol(char symbol)
        {
            if (char.IsWhiteSpace(symbol))
                throw new ArgumentException("Символ валюты не может быть пробелом!");
            Symbol = symbol;
        }
    }
}
