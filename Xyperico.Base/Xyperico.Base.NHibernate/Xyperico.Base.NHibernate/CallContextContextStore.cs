using System.Runtime.Remoting.Messaging;


namespace Xyperico.Base.NHibernate
{
  public class CallContextContextStore : INHibernateContextStore
  {
    #region INHibernateContextStore Members

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
