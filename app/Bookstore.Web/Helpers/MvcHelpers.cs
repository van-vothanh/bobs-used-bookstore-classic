using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookstore.Web.Helpers
{
    public static class MvcHelpers
    {
        public static IEnumerable<SelectListItem> GetSelectListForEnum<T>()
            where T : Enum
        {
            foreach (var val in Enum.GetValues(typeof(T)))
            {
                yield return new SelectListItem()
                {
                    Text = Enum.GetName(typeof(T), val),
                    Value = val.ToString()
                };
            }
        }
    }
}
