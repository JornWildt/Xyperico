using CuttingEdge.Conditions;


namespace Xyperico.Base.IO
{
  class HttpBaseDirContextForHostingEnvironment : IBaseDirContext
  {
    public string MapPath(string path)
    {
      Condition.Requires(path, "path").IsNotNullOrEmpty();
      return System.Web.Hosting.HostingEnvironment.MapPath(path);
    }
  }
}
