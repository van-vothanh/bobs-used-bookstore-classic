using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace BobsBookstoreClassic.Data
{
    public sealed class BookstoreConfiguration
    {
        private static readonly Lazy<BookstoreConfiguration> Lazy = new Lazy<BookstoreConfiguration>(() => new BookstoreConfiguration());

        private static BookstoreConfiguration Instance => Lazy.Value;

        private readonly Dictionary<string, string> _appSettings = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _connectionStrings = new Dictionary<string, string>();
        private static IConfiguration _configuration;

        private BookstoreConfiguration()
        {
        }

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
            
            foreach (var setting in configuration.AsEnumerable())
            {
                if (setting.Value != null)
                {
                    Instance._appSettings[setting.Key] = setting.Value;
                }
            }
        }

        public static void AddSetting(string key, string value)
        {
            Instance._appSettings[key] = value;
        }

        public static string GetSetting(string key)
        {
            return Instance._appSettings.ContainsKey(key) ? Instance._appSettings[key] : _configuration?[key];
        }

        public static T GetSetting<T>(string key)
        {
            var value = GetSetting(key);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static void AddConnectionString(string key, string value)
        {
            Instance._connectionStrings[key] = value;
        }

        public static string GetConnectionString(string key)
        {
            if (Instance._connectionStrings.ContainsKey(key))
                return Instance._connectionStrings[key];
            
            return _configuration?.GetConnectionString(key);
        }
    }
}
