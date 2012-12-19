using MongoDB.Driver;


namespace Xyperico.Base.MongoDB
{
  public class MongoDBRepositoryBase
  {
    public enum ErrorCodes { DuplicateKeyOnAdd = 11000, DuplicateKeyOnUpdate = 11001 };


    protected MongoDatabase _MDb;


    public MongoDBRepositoryBase()
    {
    }


    public MongoDBRepositoryBase(MongoDatabase mdb)
    {
      _MDb = mdb;
    }


    public virtual string MongoConfigEntry
    {
      get
      {
        return "Default";
      }
    }


    protected MongoDatabase MDb
    {
      get
      {
        if (_MDb != null)
          return _MDb;
        else
          return MongoDBSessionManager.Instance.GetMongoDBFor(MongoConfigEntry);
      }
    }
  }
}
