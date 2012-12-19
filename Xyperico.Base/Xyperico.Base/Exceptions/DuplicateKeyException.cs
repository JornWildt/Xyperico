using System;


namespace Xyperico.Base.Exceptions
{
  public class DuplicateKeyException : Exception
  {
    public string Collection { get; private set; }

    public string Key { get; private set; }


    public DuplicateKeyException(string collection, string key, string message)
      : base(message)
    {
      Collection = collection;
      Key = key;
    }
  }
}
