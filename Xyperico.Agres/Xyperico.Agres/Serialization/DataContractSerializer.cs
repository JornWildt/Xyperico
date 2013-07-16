using System;


namespace Xyperico.Agres.Serialization
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
