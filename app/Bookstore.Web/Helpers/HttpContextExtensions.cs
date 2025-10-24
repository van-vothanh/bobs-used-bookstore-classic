using Microsoft.AspNetCore.Http;
using System;

namespace Bookstore.Web.Helpers
{
    public static class HttpContextExtensions
    {
        private const string ShoppingCartCorrelationIdKey = "ShoppingCartCorrelationId";

        public static string GetShoppingCartCorrelationId(this HttpContext context)
        {
            if (context.Session.GetString(ShoppingCartCorrelationIdKey) is string correlationId)
            {
                return correlationId;
            }

            correlationId = Guid.NewGuid().ToString();
            context.Session.SetString(ShoppingCartCorrelationIdKey, correlationId);
            return correlationId;
        }
    }
}
