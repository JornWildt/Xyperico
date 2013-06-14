namespace Xyperico.Agres
{
  public interface ISerializer
  {
    byte[] Serialize(object item);
    object Deserialize(byte[] data);
  }
}
