using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace BobsBookstoreClassic.Data
{
    public sealed class BookstoreConfiguration
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static void AddSetting(string key, string value)
        {
            // Not supported in ASP.NET Core - configuration is read-only
            throw new NotSupportedException("Configuration is read-only in ASP.NET Core");
        }

        public static string GetSetting(string key)
        {
            return _configuration[key];
        }

        public static T GetSetting<T>(string key)
        {
            var value = _configuration[key];
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static void AddConnectionString(string key, string value)
        {
            // Not supported in ASP.NET Core - configuration is read-only
            throw new NotSupportedException("Configuration is read-only in ASP.NET Core");
        }

        public static string GetConnectionString(string key)
        {
            return _configuration.GetConnectionString(key);
        }
    }
}
