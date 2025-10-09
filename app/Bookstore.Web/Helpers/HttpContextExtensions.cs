using System;
using Microsoft.AspNetCore.Http;

namespace Bookstore.Web.Helpers
{
    public static class HttpContextExtensions
    {
        public static string GetShoppingCartCorrelationId(this HttpContext context)
        {
            const string cookieName = "ShoppingCartCorrelationId";
            
            if (context.Request.Cookies.TryGetValue(cookieName, out var correlationId))
            {
                return correlationId;
            }

            correlationId = Guid.NewGuid().ToString();
            context.Response.Cookies.Append(cookieName, correlationId, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                HttpOnly = true,
                IsEssential = true
            });

            return correlationId;
        }
    }
}
