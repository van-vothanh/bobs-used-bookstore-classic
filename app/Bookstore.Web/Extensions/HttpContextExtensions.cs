using Microsoft.AspNetCore.Http;

namespace Bookstore.Web.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetShoppingCartCorrelationId(this HttpContext context)
        {
            return context.Session.GetString("ShoppingCartCorrelationId") ?? Guid.NewGuid().ToString();
        }
    }
}
