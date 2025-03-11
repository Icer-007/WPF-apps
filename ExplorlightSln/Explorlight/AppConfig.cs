using Microsoft.Extensions.Configuration;

namespace Explorlight
{
    /// <summary>
    /// Application configuration from appsettings file
    /// </summary>
    public sealed class AppConfig
    {
        /// <summary>
        /// Initialize Configuration reader
        /// </summary>
        static AppConfig()
        {
            Configuration =
                new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            Instance = new AppConfig();
        }

        private AppConfig()
        { }

        /// <summary>
        /// <see cref="IConfiguration"/> instance for direct reading
        /// </summary>
        public static IConfiguration Configuration { get; }

        /// <summary>
        /// <see cref="AppConfig"/> singleton
        /// </summary>
        public static AppConfig Instance { get; } //##TODO: use injection instead of singleton

        /// <summary>
        /// True if safe mode is disabled, false otherwise
        /// </summary>
        public bool IsSafeModeOff => Configuration.GetValue<bool>("SecureOperations:authorize.delete.files")
                                     && Configuration.GetValue<bool>("SecureOperations:authorisation.effacer.fichiers");
    }
}
