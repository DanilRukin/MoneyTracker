namespace MoneyTracker.CurrencyService.Data.DataProfiles.Base
{
    /// <summary>
    /// Конфиг для PostgeSQL
    /// </summary>
    public record PostgresConfig : DatabaseProfileConfig;

    /// <summary>
    /// Конфиг для SQLServer
    /// </summary>
    public record SqlServerConfig : DatabaseProfileConfig;

    /// <summary>
    /// Конфиг для SQLLite
    /// </summary>
    public record SQLiteConfig : DatabaseProfileConfig;

    /// <summary>
    /// Конфиг для InMemoryDatabase
    /// </summary>
    public record InMemoryConfig : DatabaseProfileConfig;
}
