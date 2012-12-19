namespace Xyperico.Base.Collections
{
  public interface INameValueContextCollection
  {
    object GetData(string key);
    void SetData(string key, object value);
  }
}
