using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bookstore.Web.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static IEnumerable<SelectListItem> GetSelectListForEnum<TEnum>(this IHtmlHelper html, string emptyItem = null)
            where TEnum : Enum
        {
            if (!string.IsNullOrEmpty(emptyItem))
            {
                yield return new SelectListItem()
                {
                    Text = emptyItem,
                    Value = ""
                };
            }
            foreach (var val in Enum.GetValues(typeof(TEnum)))
            {
                yield return new SelectListItem()
                {
                    Text = Enum.GetName(typeof(TEnum), val),
                    Value = val.ToString()
                };
            }
        }

        public static SelectList EnumDropDownListFor<TModel, TEnum>(
            this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TEnum>> expression,
            string optionLabel = null)
            where TEnum : Enum
        {
            var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Select(e => new SelectListItem
                {
                    Text = e.ToString(),
                    Value = e.ToString()
                });

            return new SelectList(values, "Value", "Text");
        }
    }
}
