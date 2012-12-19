using System;


namespace Xyperico.Base.Exceptions
{
  public class UnauthorizedException : Exception
  {
    public UnauthorizedException(string msg)
      : base(msg)
    {
    }
  }
}
