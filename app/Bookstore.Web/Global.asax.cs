// TODO: Migrate to ASP.NET Core - Global.asax not supported
// Application startup logic moved to Program.cs
/*
using System.Web;
using Microsoft.AspNetCore.Mvc;
using WebOptimizer;
using Microsoft.AspNetCore.Routing;
using NLog;

namespace Bookstore.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            var logger = LogManager.GetCurrentClassLogger();

            logger.Error(ex);
        }
    }
}
*/
