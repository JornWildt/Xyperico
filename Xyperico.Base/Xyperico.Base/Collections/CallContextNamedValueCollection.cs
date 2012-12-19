using System.Runtime.Remoting.Messaging;
using Xyperico.Base.Collections;


namespace Xyperico.Base.Collections
{
  public class CallContextNamedValueCollection : INameValueContextCollection
  {
    #region INamedValueCollection Members

    public object GetData(string key)
    {
      return CallContext.GetData(key);
    }

    public void SetData(string key, object value)
    {
      CallContext.SetData(key, value);
    }

    #endregion
  }
}
