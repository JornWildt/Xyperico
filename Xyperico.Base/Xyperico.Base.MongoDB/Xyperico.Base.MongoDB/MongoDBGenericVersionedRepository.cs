using System;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MDBUpdate = MongoDB.Driver.Builders.Update;


namespace Xyperico.Base.MongoDB
{
  public abstract class MongoDBGenericVersionedRepository<TEntity, TId> : MongoDBGenericRepository<TEntity, TId>
    where TEntity : class, IHaveVersion, IHaveId<TId>
  {
    public virtual void SaveVersioned(TEntity entity)
    {
      ++entity.Version;
      try
      {
        var query = Query.And(
          Query.EQ("_id", BsonValue.Create(entity.Id)),
          Query.EQ("Version", BsonValue.Create(entity.Version-1)));
        var update = MDBUpdate.Replace(entity);
        Collection.Update(query, update);

        //LastErrorResponse response = MDb.LastError();

        //// If it fails then "updateExisting" won't be available
        //if (response.Code == (int)ErrorCodes.DuplicateKeyOnUpdate)
        //  throw new DuplicateKeyException(CollectionName, response.Error);

        //if (!(bool)response["updatedExisting"])
        //{
        //  TEntity existingEntity = Collection.FindOne(new { _id = entity.Id });
        //  if (existingEntity == null)
        //    throw new MissingResourceException(string.Format("Could not update non-existing {0} with Id {1}.", typeof(TEntity), entity.Id));
        //  else
        //    throw new VersionConflictException(entity.Id.ToString(), typeof(TEntity), entity.Version);
        //}
      }
      catch (Exception)
      {
        --entity.Version;
        throw;
      }
    }
  }
}
