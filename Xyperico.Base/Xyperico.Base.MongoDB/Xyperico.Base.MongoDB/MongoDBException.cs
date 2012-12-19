using System;


namespace Xyperico.Base.MongoDB
{
  public class MongoDBException : Exception
  {
    public MongoDBException(string message)
      : base(message)
    {
    }
  }
}
