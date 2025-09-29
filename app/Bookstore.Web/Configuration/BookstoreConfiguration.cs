using Microsoft.Extensions.Configuration;

namespace Bookstore.Web.Configuration
{
    public static class BookstoreConfiguration
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetSetting(string key)
        {
            return _configuration[key];
        }
    }
}
