using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.CurrencyService.Data.DataProfiles.Base
{
    /// <summary>
    /// Профиль настроек БД
    /// </summary>
    public record DatabaseProfileConfig
    {
        /// <summary>
        /// Строка подключения
        /// </summary>
        public string ConnectionString { get; init; }

        /// <summary>
        /// Общие настройки
        /// </summary>
        public DatabaseProfileOptions Options { get; init; }

        /// <summary>
        /// Сборка, в которую производить миграцию
        /// </summary>
        public string MigrationAssembly { get; init; }
    }
}
