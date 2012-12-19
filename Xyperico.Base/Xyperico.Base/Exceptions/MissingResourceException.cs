using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xyperico.Base.Exceptions
{
  public class MissingResourceException : Exception
  {
    public MissingResourceException(string message)
      : base(message)
    {
    }
  }
}
