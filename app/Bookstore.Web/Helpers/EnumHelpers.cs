using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Bookstore.Web.Helpers
{
    public static class EnumHelpers
    {
        public static IHtmlContent EnumDropDownListFor<TModel, TEnum>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TEnum?>> expression,
            string optionLabel = null,
            object htmlAttributes = null) where TEnum : struct, Enum
        {
            var enumType = typeof(TEnum);
            var enumValues = Enum.GetValues(enumType).Cast<TEnum>();
            
            var selectList = enumValues.Select(e => new SelectListItem
            {
                Text = e.ToString(),
                Value = Convert.ToInt32(e).ToString()
            });

            return htmlHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes);
        }
    }
}
