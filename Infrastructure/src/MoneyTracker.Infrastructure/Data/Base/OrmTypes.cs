namespace MoneyTracker.Infrastructure.Data.Base
{
    /// <summary>
    /// Типы поддерживаемых ОРМ
    /// </summary>
    public enum OrmTypes
    {
        /// <summary>
        /// EntityFrameworkCore
        /// </summary>
        EntityFrameworkCore,

        /// <summary>
        /// NHibernate
        /// </summary>
        NHibernate,

        /// <summary>
        /// Dapper
        /// </summary>
        Dapper
    }
}
