namespace MoneyTracker.CurrencyService.Data.Infrastructure
{
    internal static class ConfigKeys
    {
        private static string _delimeter = ":";
        internal static class Database
        { 
            internal static string Get() => nameof(Database);
            internal static class ActiveProfile
            {
                internal static string Get() => $"{Database.Get()}{_delimeter}{nameof(ActiveProfile)}";
            }

            internal static class Profiles
            {
                internal static string Get() => $"{Database.Get()}{_delimeter}{nameof(Profiles)}";
                internal static string Get(string profileName) => $"{Get()}{_delimeter}{profileName}";
            }
        }
    }
}
