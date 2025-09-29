using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookstore.Web.Helpers
{
    public static class MvcHelpers
    {
        public static IEnumerable<SelectListItem> GetSelectListForEnum<T>(string emptyItem = null)
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
                    Text = Enum.GetName(typeof(T), val)
                };
            }
        }

        public static IHtmlContent EnumDropDownListFor<TModel, TProperty>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            string optionLabel = null,
            object htmlAttributes = null)
        {
            var propertyType = typeof(TProperty);
            var enumType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
            
            if (!enumType.IsEnum)
                throw new ArgumentException("TProperty must be an enum or nullable enum type");
                
            var selectList = GetSelectListForEnum(enumType, optionLabel);
            return htmlHelper.DropDownListFor(expression, selectList, htmlAttributes);
        }

        public static IEnumerable<SelectListItem> GetSelectListForEnum(Type enumType, string emptyItem = null)
        {
            if (!string.IsNullOrEmpty(emptyItem))
            {
                yield return new SelectListItem()
                {
                    Text = emptyItem,
                    Value = ""
                };
            }
            foreach (var val in Enum.GetValues(enumType))
            {
                yield return new SelectListItem()
                {
                    Text = Enum.GetName(enumType, val),
                    Value = val.ToString()
                };
            }
        }

        public static IEnumerable<SelectListItem> GetSelectListForEnum<T>(this IHtmlHelper htmlHelper, string emptyItem = null)
            where T : Enum
        {
            return GetSelectListForEnum(typeof(T), emptyItem);
        }
    }
}
