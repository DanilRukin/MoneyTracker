namespace MoneyTracker.Infrastructure.Data.Base
{
    /// <summary>
    /// Типы поддерживаемых ОРМ
    /// </summary>
    public enum OrmTypes
    {
        EntityFrameworkCore,
        NHibernate,
        Dapper
    }
}
