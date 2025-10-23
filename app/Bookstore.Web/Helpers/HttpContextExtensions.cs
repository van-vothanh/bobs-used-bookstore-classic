using Microsoft.AspNetCore.Http;
using System;

namespace Bookstore.Web.Helpers
{
    public static class HttpContextExtensions
    {
        private const string ShoppingCartCorrelationIdKey = "ShoppingCartCorrelationId";

        public static Guid GetShoppingCartCorrelationId(this HttpContext context)
        {
            if (context.Session.TryGetValue(ShoppingCartCorrelationIdKey, out var bytes))
            {
                return new Guid(bytes);
            }

            var correlationId = Guid.NewGuid();
            context.Session.Set(ShoppingCartCorrelationIdKey, correlationId.ToByteArray());
            return correlationId;
        }
    }
}
