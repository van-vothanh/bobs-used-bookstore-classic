using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace Bookstore.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent EnumDropDownListFor<TModel, TEnum>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TEnum?>> expression,
            string optionLabel = null,
            object htmlAttributes = null) where TEnum : struct, Enum
        {
            var selectList = Enum.GetValues<TEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                });

            return htmlHelper.DropDownListFor(expression, selectList, optionLabel, htmlAttributes);
        }

        public static SelectList GetSelectListForEnum<TEnum>() where TEnum : struct, Enum
        {
            var values = Enum.GetValues<TEnum>()
                .Select(e => new { Value = e.ToString(), Text = e.ToString() });

            return new SelectList(values, "Value", "Text");
        }
    }
}
