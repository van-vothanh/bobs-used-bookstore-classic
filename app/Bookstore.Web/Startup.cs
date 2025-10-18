// TODO: OWIN is not supported in ASP.NET Core. This file has been replaced with Program.cs
// Migrate the configuration logic to Program.cs and use ASP.NET Core middleware instead of OWIN

/*
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Bookstore.Web.Startup))]

namespace Bookstore.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            LoggingSetup.ConfigureLogging();

            ConfigurationSetup.ConfigureConfiguration();

            DependencyInjectionSetup.ConfigureDependencyInjection(app);

            AuthenticationConfig.ConfigureAuthentication(app);
        }
    }
}
*/
