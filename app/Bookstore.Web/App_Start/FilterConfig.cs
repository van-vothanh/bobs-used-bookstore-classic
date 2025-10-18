// TODO: Migrate to ASP.NET Core - GlobalFilterCollection not supported
// In ASP.NET Core, register filters in Program.cs:
// builder.Services.AddControllersWithViews(options => {
//     options.Filters.Add(new AuthorizeFilter());
// });
/*
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
        }
    }
}
*/
