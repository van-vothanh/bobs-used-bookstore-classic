using Microsoft.AspNetCore.Http;
using System;

namespace Bookstore.Web.Helpers
{
    public static class HttpContextExtensions
    {
        private const string ShoppingCartCorrelationIdKey = "ShoppingCartCorrelationId";

        public static string GetShoppingCartCorrelationId(this HttpContext context)
        {
            if (context.Session.GetString(ShoppingCartCorrelationIdKey) == null)
            {
                context.Session.SetString(ShoppingCartCorrelationIdKey, Guid.NewGuid().ToString());
            }

            return context.Session.GetString(ShoppingCartCorrelationIdKey);
        }
    }
}
