using System;
using Xyperico.Base.CommonDomainTypes;
using MongoDB.Bson.Serialization;


namespace Xyperico.Base.MongoDB.BsonTypeConverters
{
  public static class Utility
  {
    private static bool FirstTime = true;

    internal static void RegisterAllSerializers()
    {
      if (FirstTime)
      {
        BsonSerializer.RegisterSerializer(typeof(EMail), new EMailBsonSerializer());
        BsonSerializer.RegisterSerializer(typeof(Password), new PasswordBsonSerializer());
        BsonSerializer.RegisterSerializer(typeof(Uri), new UriBsonSerializer());
        FirstTime = false;
      }
    }
  }
}
