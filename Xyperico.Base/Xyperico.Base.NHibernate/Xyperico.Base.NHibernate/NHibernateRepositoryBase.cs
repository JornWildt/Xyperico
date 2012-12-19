using System;
using NHibernate;

namespace Xyperico.Base.NHibernate
{
  public class NHibernateRepositoryBase
  {
    protected ISession _session;


    public NHibernateRepositoryBase()
    {
    }


    public NHibernateRepositoryBase(ISession session)
    {
      _session = session;
    }


    private string _NHibernateConfigEntry = null;
    public string NHibernateConfigEntry
    {
      get
      {
        return _NHibernateConfigEntry ?? "Default";
      }
      set
      {
        _NHibernateConfigEntry = value;
      }
    }


    public virtual ISession Session
    {
      get
      {
        if (_session != null)
          return _session;
        else
          return NHibernateSessionManager.Instance.GetSessionFrom(NHibernateSessionManager.GetNHibernateConfigPath(NHibernateConfigEntry));
      }
    }


    protected virtual void ExecuteWithErrorHandling(Action code)
    {
      try
      {
        code();
      }
      catch (Exception ex)
      {
        Session.Clear();
        throw new ApplicationException(string.Format("NHibernate SQL execution failed with the error message '{0}'", ex.Message), ex);
      }
    }
  }
}
