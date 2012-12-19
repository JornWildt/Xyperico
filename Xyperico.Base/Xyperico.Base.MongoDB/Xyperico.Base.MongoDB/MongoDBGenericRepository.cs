using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using Xyperico.Base.Exceptions;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Wrappers;
using MongoDB.Bson;
using MDBUpdate = MongoDB.Driver.Builders.Update;


namespace Xyperico.Base.MongoDB
{
  public class MongoDBGenericRepository<TEntity, TId> : MongoDBRepositoryBase
    where TEntity : class, IHaveId<TId>
  {
    private static bool SetupHasExecuted = false;

    public virtual string CollectionName { get { return typeof(TEntity).Name; } }

    public virtual MongoCollection<TEntity> Collection
    {
      get
      {
        return MDb.GetCollection<TEntity>(CollectionName);
      }
    }


    public MongoDBGenericRepository()
    {
      if (!SetupHasExecuted)
      {
        Setup();
        SetupHasExecuted = true;
      }
    }


    public virtual void Setup()
    {
    }


    /// <summary>
    /// Map from MondoDB error message to the name of the duplicated key (not the value!).
    /// </summary>
    /// <remarks>Used to map internal MondoDB error messages to key names that makes sense in the
    /// upper layers.</remarks>
    /// <param name="error">MongoDB error message.</param>
    /// <returns>Name of duplicate key.</returns>
    protected virtual string MapDuplicateKeyErrorToKeyName(string error)
    {
      return null;
    }


    public virtual void Add(TEntity entity)
    {
      try
      {
        SafeModeResult result = Collection.Insert(entity);
      }
      catch (MongoCommandException ex)
      {
        if (ex.Message.Contains("E11000"))
          throw new DuplicateKeyException(CollectionName, MapDuplicateKeyErrorToKeyName(ex.Message), ex.Message);
        else
          throw;
      }
    }


    public virtual bool Exists(TId id)
    {
      var query = Query.EQ("_id", BsonValue.Create(id));
      TEntity entity = Collection.FindOne(query);
      return entity != null;
    }


    public virtual TEntity GetNullable(TId id)
    {
      var query = Query.EQ("_id", BsonValue.Create(id));
      return Collection.FindOne(query);
    }


    public virtual TEntity Get(TId id)
    {
      var query = Query.EQ("_id", BsonValue.Create(id));
      TEntity entity = Collection.FindOne(query);
      if (entity == null)
        throw new MissingResourceException(string.Format("Could not find data for ID {0} of type {1}.", id, typeof(TEntity)));
      return entity;
    }


    public virtual IEnumerable<TEntity> Search()
    {
      return Collection.FindAll();
    }


    public virtual TEntity FindSingle(object template)
    {
      var query = new QueryWrapper(template);
      TEntity entity = Collection.FindOne(query);
      if (entity == null)
        throw new MissingResourceException(string.Format("Could not find data of type {0}.", typeof(TEntity)));
      return entity;
    }


    public virtual void Update(TEntity entity)
    {
      Condition.Requires(entity, "entity").IsNotNull();

      var query = Query.EQ("_id", BsonValue.Create(entity.Id));
      var update = global::MongoDB.Driver.Builders.Update.Replace(entity);

      try
      {
        Collection.Update(query, update);
      }
      catch (MongoCommandException ex)
      {
        if (ex.Message.Contains("E11000"))
          throw new DuplicateKeyException(CollectionName, MapDuplicateKeyErrorToKeyName(ex.Message), ex.Message);
        else
          throw;
      }
    }


    // Add new or update existing
    public virtual void Put(TEntity entity)
    {
      Condition.Requires(entity, "entity").IsNotNull();

      var query = Query.EQ("_id", BsonValue.Create(entity.Id));
      var update = MDBUpdate.Replace(entity);

      Collection.Update(query, update);
        //(new { _id = entity.Id }, entity, false, true);
      //LastErrorResponse response = MDb.LastError();
      //if (response.Code == (int)ErrorCodes.DuplicateKeyOnAdd || response.Code == (int)ErrorCodes.DuplicateKeyOnUpdate)
      //  throw new DuplicateKeyException(CollectionName, response.Error);
    }


    public virtual void Remove(TId id)
    {
      var query = Query.EQ("_id", BsonValue.Create(id));
      Collection.Remove(query);
    }


    public virtual void DeleteAll()
    {
      Collection.RemoveAll();
    }
  }
}
