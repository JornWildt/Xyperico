using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xyperico.Base
{
  public static class FileUtility
  {
    /// <summary>
    /// Map relative path to app-domain base directory on both servers, background processes, standalone programs and more.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="context"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string MapPathToBaseDir(string path, ICurrentDirContext context = null, string defaultValue = null)
    {
      if (path == null)
        path = defaultValue;
      if (path == null)
        return null;

      if (context == null)
        context = GetCurrentDirContext();

      path = path.Trim();

      if (IsAbsolutePath(path))
        return path;

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


    public static ICurrentDirContext GetCurrentDirContext()
    {
      if (System.Web.HttpContext.Current != null)
        return new CurrentDirContext_Http(System.Web.HttpContext.Current);
      else if (System.Web.Hosting.HostingEnvironment.IsHosted)
        return new CurrentDirContext_HostingEnv();
      else
        return new CurrentDirContext_Empty();
    }
    
    
    private static bool IsAbsolutePath(string path)
    {
      return path.Length >= 2 && path[1] == ':' && (path[2] == '/' || path[2] == '\\')
             || path.Length >= 1 && (path[0] == '/' || path[0] == '\\');
    }
  }
}
