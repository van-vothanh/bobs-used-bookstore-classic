using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bookstore.Web.Helpers
{
    public static class MvcHelpers
    {
        public static IEnumerable<SelectListItem> GetSelectListForEnum<T>(this IHtmlHelper html, string? emptyItem = null)
            where T : Enum
        {
            if (!string.IsNullOrEmpty(emptyItem))
            {
                yield return new SelectListItem()
                {
                    Text = emptyItem
                };
            }
            foreach (var val in Enum.GetValues(typeof(T)))
            {
                yield return new SelectListItem()
                {
                    Text = Enum.GetName(typeof(T), val),
                    Value = val.ToString()
                };
            }
        }

        public static IHtmlContent EnumDropDownListFor<TModel, TEnum>(
            this IHtmlHelper<TModel> html,
            Expression<Func<TModel, TEnum?>> expression,
            string? optionLabel = null,
            object? htmlAttributes = null) where TEnum : struct, Enum
        {
            var items = new List<SelectListItem>();
            
            if (!string.IsNullOrEmpty(optionLabel))
            {
                items.Add(new SelectListItem
                {
                    Text = optionLabel,
                    Value = ""
                });
            }
            
            foreach (var val in Enum.GetValues(typeof(TEnum)))
            {
                items.Add(new SelectListItem
                {
                    Text = Enum.GetName(typeof(TEnum), val),
                    Value = val.ToString()
                });
            }
            return html.DropDownListFor(expression, items, htmlAttributes);
        }
    }
}
