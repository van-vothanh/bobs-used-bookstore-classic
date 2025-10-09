using Microsoft.AspNetCore.Http;
using System;

namespace Bookstore.Web.Helpers
{
    public static class HttpContextExtensions
    {
        private const string ShoppingCartCorrelationIdKey = "ShoppingCartCorrelationId";

        public static string GetShoppingCartCorrelationId(this HttpContext context)
        {
            if (context.Request.Cookies.TryGetValue(ShoppingCartCorrelationIdKey, out var correlationId))
            {
                return correlationId;
            }

            correlationId = Guid.NewGuid().ToString();
            context.Response.Cookies.Append(ShoppingCartCorrelationIdKey, correlationId, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            });

            return correlationId;
        }
    }
}
