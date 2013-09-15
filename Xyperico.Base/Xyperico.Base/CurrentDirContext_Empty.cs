using System;


namespace Xyperico.Base
{
  public class CurrentDirContext_Empty : ICurrentDirContext
  {
    public string MapPath(string path)
    {
      if (path == null)
        throw new ArgumentNullException("path");
      string appPath = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\', '/');
      path = path.Replace("~", appPath);
      return path;
    }
  }
}
