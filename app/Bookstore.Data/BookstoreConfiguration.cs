using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace BobsBookstoreClassic.Data
{
    public sealed class BookstoreConfiguration
    {
        private static IConfiguration _configuration;
        private static readonly Lazy<BookstoreConfiguration> Lazy = new Lazy<BookstoreConfiguration>(() => new BookstoreConfiguration());

        private static BookstoreConfiguration Instance => Lazy.Value;

        private readonly Dictionary<string, string> _appSettings = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _connectionStrings = new Dictionary<string, string>();

        private BookstoreConfiguration()
        {
            if (_configuration != null)
            {
                foreach (var setting in _configuration.AsEnumerable())
                {
                    if (!string.IsNullOrEmpty(setting.Key) && !string.IsNullOrEmpty(setting.Value))
                    {
                        _appSettings[setting.Key] = setting.Value;
                    }
                }
            }
        }

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static void AddSetting(string key, string value)
        {
            Instance._appSettings[key] = value;
        }

        public static string GetSetting(string key)
        {
            if (_configuration != null)
            {
                var value = _configuration[key];
                if (value != null) return value;
            }
            return Instance._appSettings.ContainsKey(key) ? Instance._appSettings[key] : null;
        }

        public static T GetSetting<T>(string key)
        {
            var value = GetSetting(key);
            if (value == null) return default(T);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static void AddConnectionString(string key, string value)
        {
            Instance._connectionStrings[key] = value;
        }

        public static string GetConnectionString(string key)
        {
            if (_configuration != null)
            {
                var value = _configuration.GetConnectionString(key);
                if (value != null) return value;
            }
            return Instance._connectionStrings.ContainsKey(key) ? Instance._connectionStrings[key] : null;
        }

    }
}
