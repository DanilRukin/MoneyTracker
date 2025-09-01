using FluentAssertions;
using MoneyTracker.Infrastructure.Data.Base;
using System.Text.Json;

namespace MoneyTracker.Infrastructure.UnitTests.Data
{
    public partial class DatabaseOptionsTests
    {
        [Theory]
        [MemberData(nameof(ValidJsonData))]
        public void ShouldDeserializeFromJsonCorrectly(string json, DatabaseOptions realOptions)
        {
            DatabaseOptions options = JsonSerializer.Deserialize<DatabaseOptions>(json);

            options.Should().NotBeNull();

            options.OrmType.Should().Be(realOptions.OrmType);
            options.Provider.Should().Be(realOptions.Provider);
            options.ConnectionString.Should().Be(realOptions.ConnectionString);
            options.AutoMigrate.Should().Be(realOptions.AutoMigrate);
            options.SeedTestData.Should().Be(realOptions.SeedTestData);
        }

        [Theory]
        [MemberData(nameof(InvalidJsonData))]
        public void ShouldThrowOnInvalidEnumValue(string json)
        {
            Assert.Throws<JsonException>(() =>
                JsonSerializer.Deserialize<DatabaseOptions>(json));
        }
    }
}
