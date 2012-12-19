using CuttingEdge.Conditions;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using System;
using MDBUpdate = MongoDB.Driver.Builders.Update;


namespace Xyperico.Base.MongoDB
{
  public class CounterCollection : MongoDBGenericRepository<CounterCollection.Counter, string>
  {
    public class Counter : IHaveId<string>
    {
      public string Id { get; protected set; }
      public long Count { get; protected set; }

      public Counter(string id, long count)
      {
        Id = id;
        Count = count;
      }
    }


    private string _mongoConfigEntry;
    public override string MongoConfigEntry
    {
      get { return _mongoConfigEntry; }
    }


    public string CounterName { get; protected set; }


    public CounterCollection(string mongoConfigEntry, string counterName)
    {
      Condition.Requires(mongoConfigEntry, "mongoConfigEntry").IsNotNullOrEmpty();
      Condition.Requires(counterName, "counterName").IsNotNullOrEmpty();

      _mongoConfigEntry = mongoConfigEntry;
      CounterName = counterName;
    }


    public long Next()
    {
      var query = Query.EQ("_id", BsonValue.Create(CounterName));
      var sort = SortBy.Ascending("_id");
      var update = MDBUpdate.Inc("Count", 1);
      FindAndModifyResult result = Collection.FindAndModify(query, sort, update, true);
      if (result.ModifiedDocument == null)
      {
        AddInitialEntry();
        result = Collection.FindAndModify(query, sort, update, true);
      }
      if (result.ModifiedDocument == null)
        throw new InvalidOperationException(string.Format("Failed to update counter '{0}'.", CounterName));
      return result.ModifiedDocument.GetValue("Count").ToInt64();
    }


    protected void AddInitialEntry()
    {
      Add(new Counter(CounterName, 0));
    }
  }
}
