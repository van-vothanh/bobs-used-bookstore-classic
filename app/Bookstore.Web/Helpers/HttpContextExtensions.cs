using Microsoft.AspNetCore.Http;
using System;

namespace Bookstore.Web.Helpers
{
    public static class HttpContextExtensions
    {
        public static string GetShoppingCartCorrelationId(this HttpContext context)
        {
            const string SessionKey = "ShoppingCartCorrelationId";
            
            var correlationId = context.Session.GetString(SessionKey);
            
            if (string.IsNullOrEmpty(correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Session.SetString(SessionKey, correlationId);
            }
            
            return correlationId;
        }
    }
}
