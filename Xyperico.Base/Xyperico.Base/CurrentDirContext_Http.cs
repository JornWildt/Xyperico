using System.Web;


namespace Xyperico.Base
{
  public class CurrentDirContext_Http : ICurrentDirContext
  {
    private HttpContext Context { get; set; }


    public CurrentDirContext_Http()
    {
      Context = HttpContext.Current;
    }


    public CurrentDirContext_Http(HttpContext context)
    {
      Context = context;
    }


    #region IHttpContext Members

    public string MapPath(string path)
    {
      if (Context != null)
        return Context.Server.MapPath(path);
      else
        return path;
    }

    #endregion
  }
}
