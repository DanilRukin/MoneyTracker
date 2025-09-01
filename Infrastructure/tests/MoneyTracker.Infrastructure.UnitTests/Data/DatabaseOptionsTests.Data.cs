using MoneyTracker.Infrastructure.Data.Base;

namespace MoneyTracker.Infrastructure.UnitTests.Data
{
    public partial class DatabaseOptionsTests
    {
        public static IEnumerable<object[]> ValidJsonData =>
            new List<object[]>
            {
                new object[]
                {
                    """
                     {
                        "OrmType": "EntityFrameworkCore",
                        "Provider": "Postgres",
                        "ConnectionString": "Host=localhost; ",
                        "AutoMigrate": true,
                        "SeedTestData": true
                     }
                    """,
                     new DatabaseOptions()
                     {
                         OrmType = OrmTypes.EntityFrameworkCore,
                         Provider = ProviderTypes.Postgres,
                         ConnectionString = "Host=localhost; ",
                         AutoMigrate = true,
                         SeedTestData = true
                     }
                },
                new object[]
                {
                    """
                     {
                        "OrmType": "NHibernate",
                        "Provider": "Postgres",
                        "ConnectionString": "Host=localhost; ",
                        "AutoMigrate": true,
                        "SeedTestData": true
                     }
                    """,
                     new DatabaseOptions()
                     {
                         OrmType = OrmTypes.NHibernate,
                         Provider = ProviderTypes.Postgres,
                         ConnectionString = "Host=localhost; ",
                         AutoMigrate = true,
                         SeedTestData = true
                     }
                },
                new object[]
                {
                    """
                     {
                        "OrmType": "Dapper",
                        "Provider": "Postgres",
                        "ConnectionString": "Host=localhost; ",
                        "AutoMigrate": true,
                        "SeedTestData": true
                     }
                    """,
                     new DatabaseOptions()
                     {
                         OrmType = OrmTypes.Dapper,
                         Provider = ProviderTypes.Postgres,
                         ConnectionString = "Host=localhost; ",
                         AutoMigrate = true,
                         SeedTestData = true
                     }
                },
                new object[]
                {
                    """
                     {
                        "OrmType": "Dapper",
                        "Provider": "Sqlite",
                        "ConnectionString": "Host=localhost; ",
                        "AutoMigrate": true,
                        "SeedTestData": true
                     }
                    """,
                     new DatabaseOptions()
                     {
                         OrmType = OrmTypes.Dapper,
                         Provider = ProviderTypes.Sqlite,
                         ConnectionString = "Host=localhost; ",
                         AutoMigrate = true,
                         SeedTestData = true
                     }
                },
                new object[]
                {
                    """
                     {
                        "OrmType": "Dapper",
                        "Provider": "SqlServer",
                        "ConnectionString": "Host=localhost; ",
                        "AutoMigrate": true,
                        "SeedTestData": true
                     }
                    """,
                     new DatabaseOptions()
                     {
                         OrmType = OrmTypes.Dapper,
                         Provider = ProviderTypes.SqlServer,
                         ConnectionString = "Host=localhost; ",
                         AutoMigrate = true,
                         SeedTestData = true
                     }
                },
                new object[]
                {
                    """
                     {
                        "OrmType": "Dapper",
                        "Provider": "InMemory",
                        "ConnectionString": "Host=localhost; ",
                        "AutoMigrate": true,
                        "SeedTestData": true
                     }
                    """,
                     new DatabaseOptions()
                     {
                         OrmType = OrmTypes.Dapper,
                         Provider = ProviderTypes.InMemory,
                         ConnectionString = "Host=localhost; ",
                         AutoMigrate = true,
                         SeedTestData = true
                     }
                }
            };

        public static IEnumerable<object[]> InvalidJsonData =>
            new List<object[]>
            {
                new object[]
                {
                    """
                    {
                        "OrmType": "NO",
                        "Provider": "NO",
                        "ConnectionString": "Host=localhost; ",
                        "AutoMigrate": true,
                        "SeedTestData": true
                    }
                    """
                },
                new object[]
                {
                    """
                    {
                        "OrmType": "NO",
                        "Provider": "Postgres",
                        "ConnectionString": "Host=localhost; ",
                        "AutoMigrate": true,
                        "SeedTestData": true
                    }
                    """
                },
                new object[]
                {
                    """
                    {
                        "OrmType": "EntityFrameworkCore",
                        "Provider": "NO",
                        "ConnectionString": "Host=localhost; ",
                        "AutoMigrate": true,
                        "SeedTestData": true
                    }
                    """
                }
            };
    }
}
