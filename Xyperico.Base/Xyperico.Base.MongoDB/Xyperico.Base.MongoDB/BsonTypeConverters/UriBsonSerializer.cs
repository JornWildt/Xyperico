using System;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;


namespace Xyperico.Base.MongoDB.BsonTypeConverters
{
  public class UriBsonSerializer : IBsonSerializer
  {
    #region IBsonTypeConverter Members

    public object ConvertFromBson(object data)
    {
      string uri = (string)data;
      if (uri == null)
        return null;
      return new Uri(uri);
    }

    public object ConvertToBson(object data)
    {
      Uri uri = (Uri)data;
      if (uri == null)
        return null;
      return uri.ToString();
    }

    public Type SerializedType
    {
      get { return typeof(string); }
    }

    #endregion

    #region IBsonSerializer Members

    public object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
    {
      throw new NotImplementedException();
    }

    public object Deserialize(BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
    {
      throw new NotImplementedException();
    }

    public IBsonSerializationOptions GetDefaultSerializationOptions()
    {
      throw new NotImplementedException();
    }

    public bool GetDocumentId(object document, out object id, out Type idNominalType, out IIdGenerator idGenerator)
    {
      throw new NotImplementedException("Do not make EMails root elements");
    }

    public BsonSerializationInfo GetItemSerializationInfo()
    {
      throw new NotImplementedException();
    }

    public BsonSerializationInfo GetMemberSerializationInfo(string memberName)
    {
      throw new NotImplementedException();
    }

    public void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
    {
      throw new NotImplementedException();
    }

    public void SetDocumentId(object document, object id)
    {
      throw new NotImplementedException("Do not make EMails root elements");
    }

    #endregion
  }
}
