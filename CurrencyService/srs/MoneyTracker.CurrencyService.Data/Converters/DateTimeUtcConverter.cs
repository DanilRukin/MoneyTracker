using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MoneyTracker.CurrencyService.Data.Converters
{
    public class DateTimeUtcConverter : ValueConverter<DateTime, DateTime>
    {
        public DateTimeUtcConverter() : base(dateTime => dateTime.ToUniversalTime(), utcDateTime => utcDateTime.ToLocalTime())
        {
        }
    }
}
