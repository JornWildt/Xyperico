using CuttingEdge.Conditions;


namespace Xyperico.Base.IO
{
  class StandaloneBaseDirContext : IBaseDirContext
  {
    public string MapPath(string path)
    {
      Condition.Requires(path, "path").IsNotNullOrEmpty();
      string appPath = System.AppDomain.CurrentDomain.BaseDirectory;
      path = path.Replace("~", appPath);
      return path;
    }
  }
}
