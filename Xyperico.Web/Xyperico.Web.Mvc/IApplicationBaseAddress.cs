using System;


namespace Xyperico.Web.Mvc
{
  public interface IApplicationBaseAddress
  {
    string Url { get; }
    Uri Uri { get; }
  }
}
