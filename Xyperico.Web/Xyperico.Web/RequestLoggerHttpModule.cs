using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace Xyperico.Web
{
  public class RequestLoggerHttpModule : IHttpModule
  {
    #region IHttpModule Members

    public void Dispose()
    {
    }

    public void Init(HttpApplication context)
    {
      context.BeginRequest += Context_BeginRequest;
    }

    #endregion

    
    static object FileLock = new object();

    void Context_BeginRequest(object sender, EventArgs e)
    {
      //using (StreamWriter w = new StreamWriter("c:/tmp/httplog.txt", true))
      using (StringWriter w = new StringWriter())
      {
        HttpContext context = HttpContext.Current;
        HttpRequest request = context.Request;

        w.WriteLine("{0} {1}", request.HttpMethod, context.Request.RawUrl);
        foreach (string key in request.Headers.Keys)
        {
          w.WriteLine("  {0}: {1}", key, request.Headers[key]);
        }

        lock (FileLock)
        {
          File.AppendAllText("c:/tmp/httplog.txt", w.ToString());
        }
      }
    }
  }
}
