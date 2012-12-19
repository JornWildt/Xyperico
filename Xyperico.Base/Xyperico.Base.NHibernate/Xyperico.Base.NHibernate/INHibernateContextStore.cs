
namespace Xyperico.Base.NHibernate
{
  /// <summary>
  /// Since multiple databases may be in use, there may be one session per database 
  /// persisted at any one time.  The easiest way to store them is via a mapping
  /// with the key being tied to session factory.  If within a web context, this uses
  /// <see cref="HttpContext" /> instead of the WinForms specific <see cref="CallContext" />.  
  /// Discussion concerning this found at http://forum.springframework.net/showthread.php?t=572
  /// </summary>
  public interface INHibernateContextStore
  {
    object GetData(string key);
    void SetData(string key, object value);
  }
}
