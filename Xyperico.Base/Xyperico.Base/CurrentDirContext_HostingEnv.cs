namespace Xyperico.Base
{
  public class CurrentDirContext_HostingEnv : ICurrentDirContext
  {
    public string MapPath(string path)
    {
      return System.Web.Hosting.HostingEnvironment.MapPath(path);
    }
  }
}
