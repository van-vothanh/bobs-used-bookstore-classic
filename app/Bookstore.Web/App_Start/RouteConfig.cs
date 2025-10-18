// TODO: Migrate to ASP.NET Core - RouteCollection not supported
// In ASP.NET Core, configure routes in Program.cs:
// app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
/*
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Bookstore.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Bookstore.Web.Controllers" }
            );
        }
    }
}
*/
