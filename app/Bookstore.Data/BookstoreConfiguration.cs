using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace BobsBookstoreClassic.Data
{
    public sealed class BookstoreConfiguration
    {
        private static IConfiguration? _configuration;
        private readonly Dictionary<string, string?> _appSettings = new Dictionary<string, string?>();
        private readonly Dictionary<string, string?> _connectionStrings = new Dictionary<string, string?>();

        private BookstoreConfiguration()
        {
            if (_configuration != null)
            {
                foreach (var setting in _configuration.AsEnumerable())
                {
                    if (setting.Value != null)
                    {
                        _appSettings[setting.Key] = setting.Value;
                    }
                }

                var connectionStrings = _configuration.GetSection("ConnectionStrings");
                foreach (var conn in connectionStrings.GetChildren())
                {
                    if (conn.Value != null)
                    {
                        _connectionStrings[conn.Key] = conn.Value;
                    }
                }
            }

            // Override with environment variables
            foreach (var key in _appSettings.Keys)
            {
                var envValue = Environment.GetEnvironmentVariable(key);
                if (envValue != null)
                {
                    _appSettings[key] = envValue;
                }
            }
        }

        private static readonly Lazy<BookstoreConfiguration> Lazy = new Lazy<BookstoreConfiguration>(() => new BookstoreConfiguration());
        private static BookstoreConfiguration Instance => Lazy.Value;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static void AddSetting(string key, string value)
        {
            Instance._appSettings[key] = value;
        }

        public static string? GetSetting(string key)
        {
            return Instance._appSettings.TryGetValue(key, out var value) ? value : null;
        }

        public static T? GetSetting<T>(string key)
        {
            var value = GetSetting(key);
            if (value == null) return default;
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static void AddConnectionString(string key, string value)
        {
            Instance._connectionStrings[key] = value;
        }

        public static string? GetConnectionString(string key)
        {
            return Instance._connectionStrings.TryGetValue(key, out var value) ? value : null;
        }
    }
}
