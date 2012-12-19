using System;
using System.Collections.Specialized;


namespace Xyperico.Base.Web
{
  public static class UriTemplateExtensions
  {
    public static Uri BindByName(this UriTemplate template, Uri baseAddress, object parameters)
    {
      NameValueCollection parameterCollection = Utility.GetNameValueCollection(parameters);
      return template.BindByName(baseAddress, parameterCollection);
    }
  }
}
