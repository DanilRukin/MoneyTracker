using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.Infrastructure.Data.Configuration
{
    public static class ConfigKeys
    {
        private static string _delimeter = ":";
        public static class Database
        {
            public static string Get() => nameof(Database);
            public static class Provider
            {
                public static string Get() => $"{Database.Get()}{_delimeter}{nameof(Provider)}";
            }
            public static class OrmType
            {
                public static string Get() => $"{Database.Get()}{_delimeter}{nameof(OrmType)}";
            }
            public static class ConnectionString
            {
                public static string Get() => $"{Database.Get()}{_delimeter}{nameof(ConnectionString)}";
            }
            public static class AutoMigrate
            {
                public static string Get() => $"{Database.Get()}{_delimeter}{nameof(AutoMigrate)}";
            }
            public static class SeedTestData
            {
                public static string Get() => $"{Database.Get()}{_delimeter}{nameof(SeedTestData)}";
            }
        }
    }
}
