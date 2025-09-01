using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.Infrastructure.Data.Base
{
    /// <summary>
    /// Провайдер базы данных
    /// </summary>
    public interface IDatabaseProvider
    {
        /// <summary>
        /// Выполняет настройку подключения к БД
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="options">Опции подключения</param>
        IServiceCollection Configure(IServiceCollection services, DatabaseOptions options);

        /// <summary>
        /// Выполняет инициализацию БД
        /// </summary>
        /// <param name="services">Сервисы приложения</param>
        /// <param name="options">Опции БД</param>
        /// <param name="token">Токен отмены</param>
        Task InitializeAsync(IServiceProvider services, DatabaseOptions options, CancellationToken token);
    }
}
