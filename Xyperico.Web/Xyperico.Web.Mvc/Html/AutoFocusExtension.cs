using System;
using System.Linq.Expressions;
using System.Web.Mvc;


namespace Xyperico.Web.Mvc.Html
{
  public static class AutoFocusExtension
  {
    public static MvcHtmlString AutoFocusFor<TModel, TProperty>
      (this HtmlHelper<TModel> html,
       Expression<Func<TModel, TProperty>> expression)
      where TModel : class
    {
      ModelMetadata meta = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
      string expressionText = ExpressionHelper.GetExpressionText(expression);
      string name = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText);
      string id = name.Replace(".", "_");

      return GetAutoFocusHtml("#" + id);
    }


    public static MvcHtmlString AutoFocusFor<TModel>
      (this HtmlHelper<TModel> html,
       string elementReference)
    {
      return GetAutoFocusHtml(elementReference);
    }


    private static MvcHtmlString GetAutoFocusHtml(string elementReference)
    {
      return MvcHtmlString.Create(string.Format(@"<script type=""text/javascript"">
$(document).ready(function() {{
  $(""{0}"").focus();
}});
  </script>", elementReference));
    }
  }
}
