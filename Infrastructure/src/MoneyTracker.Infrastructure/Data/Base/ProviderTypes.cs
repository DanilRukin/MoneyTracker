using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.Infrastructure.Data.Base
{
    /// <summary>
    /// Типы СУБД
    /// </summary>
    public enum ProviderTypes
    {
        Postgres,
        SqlServer,
        Sqlite,
        InMemory
    }
}
