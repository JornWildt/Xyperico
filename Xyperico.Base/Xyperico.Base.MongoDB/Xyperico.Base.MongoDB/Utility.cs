using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;


namespace Xyperico.Base.MongoDB
{
  public static class Utility
  {
    private static bool FirstTime = true;

    public static void Initialize()
    {
      if (FirstTime)
      {
        BsonTypeConverters.Utility.RegisterAllSerializers();

        var conventions = ConventionProfile.GetDefault();
        conventions.SetMemberFinderConvention(new ProtectedMemberFinderConvention());
        BsonClassMap.RegisterConventions(conventions, t => true);

        FirstTime = false;
      }
    }
  }
}
