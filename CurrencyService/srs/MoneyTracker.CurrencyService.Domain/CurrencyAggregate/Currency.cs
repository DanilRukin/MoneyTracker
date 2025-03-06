using MoneyTracker.CurrencyService.Domain.CurrencyAggregate.Events;
using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate;
using SharedKernel;
using SharedKernel.Interfaces;

namespace MoneyTracker.CurrencyService.Domain.CurrencyAggregate
{
    /// <summary>
    /// Сущность валюты
    /// </summary>
    public class Currency : EntityBase<int>, IAgregateRoot
    {
        /// <summary>
        /// Код валюты
        /// </summary>
        public string Code { get; protected set; }

        /// <summary>
        /// Наименование валюты
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Символ валюты
        /// </summary>
        public char Symbol { get; protected set; }

        /// <summary>
        /// Признак активности валюты, если <see langword="false"/>, то валюта больше не поддерживается
        /// </summary>
        public bool IsActive { get; protected set; }

        /// <summary>
        /// Список связанных валют
        /// </summary>
        private List<CurrencyPair> _currencyPairs = new List<CurrencyPair>();

        /// <summary>
        /// Список связанных валют
        /// </summary>
        public IReadOnlyCollection<CurrencyPair>? CurrencyPairs => _currencyPairs.AsReadOnly();

        /// <summary>
        /// Список валютных пар, где данная валюта является основной
        /// </summary>
        public IReadOnlyCollection<CurrencyPair>? OnThesePairsIsBase => _currencyPairs
            ?.Where(pair => pair.BaseCurrency?.Id == Id)
            ?.ToList()
            ?.AsReadOnly();

        /// <summary>
        /// Список валютных пар, где данная валюта является целевой
        /// </summary>
        public IReadOnlyCollection<CurrencyPair>? OnThesePairsIsTarget => _currencyPairs
            ?.Where(pair => pair.TargetCurrency?.Id == Id)
            ?.ToList()
            ?.AsReadOnly();

        protected Currency() { }

        public Currency(string code, string name, char symbol, bool isActive)
        {
            Code = code;
            Name = name;
            Symbol = symbol;
            IsActive = isActive;
        }

        /// <summary>
        /// Активирует валюту
        /// </summary>
        public void Activate()
        {
            if (!IsActive)
            {
                IsActive = true;
                AddDomainEvent(new CurrencyActivatedDomainEvent(Id));
                _currencyPairs.ForEach(pair => pair.Activate());
            }
        }

        /// <summary>
        /// Архивирует валюту
        /// </summary>
        public void Archive()
        {
            if (IsActive)
            {
                IsActive = false;
                AddDomainEvent(new CurrencyArchivedDomainEvent(Id));
                _currencyPairs.ForEach(pair => pair.Archive());
            }
        }

        /// <summary>
        /// Добавляет валютную пару
        /// </summary>
        /// <param name="currencyPair"></param>
        public void AddCurrencyPair(CurrencyPair currencyPair)
        {
            _currencyPairs ??= [];
            ValidateCurrencyPair(currencyPair);
            if (!_currencyPairs.Contains(currencyPair))
            {
                _currencyPairs.Add(currencyPair);
            }
        }

        private void ValidateCurrencyPair(CurrencyPair currencyPair)
        {
            if (currencyPair is null)
            {
                throw new ArgumentNullException("Нельзя добавить несуществующую валютную пару");
            }
            if (currencyPair.BaseCurrency != this && currencyPair.TargetCurrency != this)
            {
                throw new InvalidOperationException($"Данная валютная пара не принадлежит этой валюте ('{Name}')");
            }
        }

        /// <summary>
        /// Удаляет валютную пару
        /// </summary>
        /// <param name="currencyPair"></param>
        internal void DeleteCurrencyPair(CurrencyPair currencyPair) =>
            _currencyPairs?.Remove(currencyPair);
    }
}
