// TODO: Migrate to ASP.NET Core - AreaRegistration not supported
// In ASP.NET Core, areas are configured using [Area] attribute on controllers
// and routes in Program.cs:
// app.MapControllerRoute(
//     name: "areas",
//     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
/*
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web.Areas
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }, namespaces: new[] { "Bookstore.Web.Areas.Admin.Controllers" }
            );
        }
    }
}
*/
