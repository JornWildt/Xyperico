using System.IO;


namespace Xyperico.Agres.DocumentStore
{
  public interface IStreamSerializer
  {
    void Serialize(Stream s, object item);
    object Deserialize(Stream s);
  }
}
