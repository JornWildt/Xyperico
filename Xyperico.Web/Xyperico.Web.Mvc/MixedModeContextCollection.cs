using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xyperico.Base.Collections;
using System.Web;
using System.Runtime.Remoting.Messaging;

namespace Xyperico.Web.Mvc
{
  public class MixedModeContextCollection : INameValueContextCollection
  {
    private const string SESSION_KEY = "MVCCONTEXTCOLLECTION_SESSIONS";


    #region INamedValueCollection Members

    public object GetData(string key)
    {
      if (HttpContext.Current == null)
        return CallContext.GetData(key);
      else
        return HttpContext.Current.Items[key];
    }

    public void SetData(string key, object value)
    {
      if (HttpContext.Current == null)
        CallContext.SetData(key, value);
      else
        HttpContext.Current.Items[key] = value;
    }

    #endregion
  }
}
