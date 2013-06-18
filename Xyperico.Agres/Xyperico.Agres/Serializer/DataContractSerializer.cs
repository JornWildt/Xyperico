using System;


namespace Xyperico.Agres.Serializer
{
  public class DataContractSerializer : AbstractSerializer
  {
    protected override ISerializeWorker CreateWorker(Type t)
    {
      return new DataContractSerializerWorker(t);
    }


    public DataContractSerializer()
    {
      Initialize();
    }
  }
}
