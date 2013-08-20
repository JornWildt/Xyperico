using System;


namespace Xyperico.Agres.Serialization
{
  public class XmlSerializer : AbstractSerializer
  {
    protected override ISerializeWorker CreateWorker(Type t)
    {
      return new XmlSerializerWorker(t);
    }


    public XmlSerializer()
    {
      Initialize();
    }
  }
}
