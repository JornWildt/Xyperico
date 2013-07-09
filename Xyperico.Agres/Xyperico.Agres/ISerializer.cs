using System.IO;


namespace Xyperico.Agres
{
  public interface ISerializer
  {
    byte[] Serialize(object item);
    void Serialize(Stream s, object item);
    object Deserialize(byte[] data);
    object Deserialize(Stream s);
  }
}
