using System;
using System.Web;


namespace Xyperico.Base.MongoDB
{
  public class MongoDBSessionHttpModule : IHttpModule
  {
    #region IHttpModule Members

    public void Dispose()
    {
    }

    
    public void Init(HttpApplication context)
    {
      context.EndRequest += context_EndRequest;
    }

    #endregion


    void context_EndRequest(object sender, EventArgs e)
    {
    }
  }
}
