using Microsoft.Extensions.DependencyInjection;

namespace MoneyTracker.Accounts.Application;

public static class AccountsModule
{
    /// <summary>
    /// Добавляет модуль Accounts
    /// </summary>
    /// <param name="services">Коллекция сервисов приложения</param>
    public static IServiceCollection AddAccountsModule(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(AccountsModule).Assembly);
        });
        return services;
    }
}
