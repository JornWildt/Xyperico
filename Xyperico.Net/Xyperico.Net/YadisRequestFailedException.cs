using System;
using System.Net;


namespace Xyperico.Net
{
  public class YadisRequestFailedException : WebException
  {
    public YadisRequestFailedException(string msg)
      : base(msg)
    {
    }
  }
}
