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
        /// <summary>
        /// PostgreSQL
        /// </summary>
        Postgres,

        /// <summary>
        /// SQLServer
        /// </summary>
        SqlServer,

        /// <summary>
        /// SQLite
        /// </summary>
        Sqlite,

        /// <summary>
        /// InMemoryDatabase
        /// </summary>
        InMemory
    }
}
