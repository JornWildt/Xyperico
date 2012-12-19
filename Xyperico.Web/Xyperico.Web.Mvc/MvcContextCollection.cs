using System.Web;
using Xyperico.Base.Collections;


namespace Xyperico.Web.Mvc
{
  public class MvcContextCollection : INameValueContextCollection
  {
    private const string SESSION_KEY = "MVCCONTEXTCOLLECTION_SESSIONS";


    #region INamedValueCollection Members

    public object GetData(string key)
    {
      return HttpContext.Current.Items[key];
    }

    public void SetData(string key, object value)
    {
      HttpContext.Current.Items[key] = value;
    }

    #endregion
  }
}
