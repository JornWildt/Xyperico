using System.IO;

namespace Xyperico.Base.IO
{
  public static class FileUtils
  {
    /// <summary>
    /// Map relative path to app-domain base directory on both servers, background processes, standalone programs and more.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="context"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string MapRelPathToBaseDir(string path, IBaseDirContext context = null, string defaultValue = null)
    {
      if (path == null)
        path = defaultValue;
      if (path == null)
        return null;

      path = path.Trim();

      if (Path.IsPathRooted(path))
        return path;

      if (context == null)
      {
        if (System.Web.HttpContext.Current != null)
          context = new HttpBaseDirContext();
        else if (System.Web.Hosting.HostingEnvironment.IsHosted)
          context = new HttpBaseDirContextForHostingEnvironment();
        else
          context = new StandaloneBaseDirContext();
      }

      if (!path.Contains("~"))
      {
        if (path.Length > 1 && !(path[0] == '/' || path[0] == '\\'))
          path = "~/" + path;
        else
          path = "~" + path;
        // Unfortunately this does not work: path = Path.Combine("~", path);
      }

      path = context.MapPath(path);

      // Avoid troublesome double-slashes
      path = path.Replace("\\/", "\\").Replace("\\\\", "\\");

      return path;
    }


  }
}
