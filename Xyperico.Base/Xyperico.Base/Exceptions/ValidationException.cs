using System;


namespace Xyperico.Base.Exceptions
{
  public class ValidationException : Exception
  {
    public ValidationException(string message)
      : base(message)
    {
    }
  }
}
