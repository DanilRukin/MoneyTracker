using MoneyTracker.Accounts.Domain.Transactions;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.Accounts.Domain.Categories
{
    internal abstract class Category : IdentifiedValueObject<int>
    {
        public string Name { get; private set; } = default!;
        public abstract TransactionType Type { get; protected set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Type;
        }

        protected Category(string name)
        {
            SetName(name);
        }

        protected Category() { }

        private void SetName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            Name = name;
        }
    }
}
