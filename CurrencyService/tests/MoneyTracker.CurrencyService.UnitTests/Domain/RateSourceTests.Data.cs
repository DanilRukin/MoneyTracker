using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.CurrencyService.UnitTests.Domain
{
    public partial class RateSourceTests
    {
        public static IEnumerable<object?[]> InvalidNames =>
            new List<object?[]>()
            {
                new object?[] { "" },
                new object?[] { string.Empty },
                new object?[] { "   " },
                new object?[] { " " },
                new object?[] { "    some name" },
                new object?[] { "somename   " },
                new object?[] { null }
            };
    }
}
