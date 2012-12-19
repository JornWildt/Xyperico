using CuttingEdge.Conditions;


namespace Xyperico.Base.IO
{
  class HttpBaseDirContext : IBaseDirContext
  {
    public string MapPath(string path)
    {
      Condition.Requires(path, "path").IsNotNullOrEmpty();
      return System.Web.HttpContext.Current.Server.MapPath(path);
    }
  }
}
