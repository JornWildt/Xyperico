using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;


namespace Xyperico.Base.NHibernate
{
  public class NHibernateSessionManager
  {
    #region Thread-safe, lazy Singleton

    /// <summary>
    /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
    /// for more details about its implementation.
    /// </summary>
    public static NHibernateSessionManager Instance
    {
      get
      {
        return Nested.NHibernateSessionManager;
      }
    }

    /// <summary>
    /// Private constructor to enforce singleton
    /// </summary>
    private NHibernateSessionManager() { }

    /// <summary>
    /// Assists with ensuring thread-safe, lazy singleton
    /// </summary>
    private class Nested
    {
      static Nested() { }
      internal static readonly NHibernateSessionManager NHibernateSessionManager = new NHibernateSessionManager();
    }

    #endregion


    public static string DefaultNHibernateConfigPath
    {
      get
      {
        return GetNHibernateConfigPath("Default");
      }
    }


    public static string GetNHibernateConfigPath(string configEntry)
    {
      if (Xyperico.Base.NHibernate.ConfigurationSettings.Settings != null)
      {
        // Try "<MachineName>_<entry>"
        string localConfigName = System.Environment.MachineName + "_" + configEntry;
        if (Xyperico.Base.NHibernate.ConfigurationSettings.Settings.ConfigurationFiles[localConfigName] != null)
          return Xyperico.Base.NHibernate.ConfigurationSettings.Settings.ConfigurationFiles[localConfigName].Filename;

        // Try "<entry>"
        if (Xyperico.Base.NHibernate.ConfigurationSettings.Settings.ConfigurationFiles[configEntry] != null)
          return Xyperico.Base.NHibernate.ConfigurationSettings.Settings.ConfigurationFiles[configEntry].Filename;

        // Try "<MachineName>_Default"
        localConfigName = System.Environment.MachineName + "_Default";
        if (Xyperico.Base.NHibernate.ConfigurationSettings.Settings.ConfigurationFiles[localConfigName] != null)
          return Xyperico.Base.NHibernate.ConfigurationSettings.Settings.ConfigurationFiles[localConfigName].Filename;

        // Try "Default"
        if (Xyperico.Base.NHibernate.ConfigurationSettings.Settings.ConfigurationFiles["Default"] != null)
          return Xyperico.Base.NHibernate.ConfigurationSettings.Settings.ConfigurationFiles["Default"].Filename;
      }

      throw new ArgumentException("NHibernate configuration entry '" + configEntry + "' (and it's alternatives) does not exist.");
    }


    /// <summary>
    /// This method attempts to find a session factory stored in <see cref="sessionFactories" />
    /// via its name; if it can't be found it creates a new one and adds it the hashtable.
    /// </summary>
    /// <param name="sessionFactoryConfigPath">Path location of the factory config</param>
    private ISessionFactory GetSessionFactoryFor(string sessionFactoryConfigPath)
    {
      // FIXME: CHECK => Attribute policy?
      if (string.IsNullOrEmpty(sessionFactoryConfigPath))
        throw new ApplicationException("sessionFactoryConfigPath may not be null nor empty");

      lock (typeof(NHibernateSessionManager))
      {
        //  Attempt to retrieve a stored SessionFactory from the hashtable.
        ISessionFactory sessionFactory = sessionFactories.ContainsKey(sessionFactoryConfigPath)
                                         ? sessionFactories[sessionFactoryConfigPath]
                                         : null;

        //  Failed to find a matching SessionFactory so make a new one.
        if (sessionFactory == null)
        {
          if (!File.Exists(sessionFactoryConfigPath))
            throw new ApplicationException("The config file at '" + sessionFactoryConfigPath + "' could not be found (current directory is '" + Directory.GetCurrentDirectory() + "')");

          Configuration cfg = GetConfigurationFrom(sessionFactoryConfigPath);

          foreach (ConfigurationSettings.MappingAssemblyElement e in ConfigurationSettings.Settings.MappingAssemblies)
          {
            Assembly a = Assembly.Load(e.Assembly);
            cfg.AddAssembly(a);
          }

          //  Now that we have our Configuration object, create a new SessionFactory
          sessionFactory = cfg.BuildSessionFactory();

          if (sessionFactory == null)
            throw new InvalidOperationException("cfg.BuildSessionFactory() returned null.");

          sessionFactories.Add(sessionFactoryConfigPath, sessionFactory);
        }

        return sessionFactory;
      }
    }


    public Configuration GetConfigurationFrom(string sessionFactoryConfigPath)
    {
      if (!configurations.ContainsKey(sessionFactoryConfigPath))
      {
        Configuration config = new Configuration();
        config.Configure(sessionFactoryConfigPath);

        configurations[sessionFactoryConfigPath] = config;
      }

      return configurations[sessionFactoryConfigPath];
    }


    public ISession GetSession()
    {
      return GetSessionFrom(DefaultNHibernateConfigPath, null);
    }


    public ISession GetSessionFrom(string sessionFactoryConfigPath)
    {
      return GetSessionFrom(sessionFactoryConfigPath, null);
    }


    /// <summary>
    /// Gets a session with or without an interceptor.  This method is not called directly; instead,
    /// it gets invoked from other public methods.
    /// </summary>
    private ISession GetSessionFrom(string sessionFactoryConfigPath, IInterceptor interceptor)
    {
      ISession session = ContextSessions.ContainsKey(sessionFactoryConfigPath)
                         ? ContextSessions[sessionFactoryConfigPath]
                         : null;

      if (session == null)
      {
        if (interceptor != null)
        {
          session = GetSessionFactoryFor(sessionFactoryConfigPath).OpenSession(interceptor);
        }
        else
        {
          session = GetSessionFactoryFor(sessionFactoryConfigPath).OpenSession();
        }

        ContextSessions[sessionFactoryConfigPath] = session;
      }

      if (session == null)
        throw new ApplicationException("NHibernate session was null");

      return session;
    }


    public IEnumerable<ISession> Sessions
    {
      get
      {
        return ContextSessions.Values;
      }
    }


    /// <summary>
    /// Flushes anything left in the session and closes the connection.
    /// </summary>
    public void CloseSession(string sessionFactoryConfigPath)
    {
      ISession session = ContextSessions.ContainsKey(sessionFactoryConfigPath)
                         ? ContextSessions[sessionFactoryConfigPath]
                         : null;

      if (session != null && session.IsOpen)
      {
        session.Flush();
        session.Close();
      }

      ContextSessions.Remove(sessionFactoryConfigPath);
    }


    private INHibernateContextStore _contextStore;

    protected INHibernateContextStore ContextStore
    {
      get
      {
        if (_contextStore == null)
        {
          _contextStore = Xyperico.Base.Castle.Container.Resolve<INHibernateContextStore>();
        }
        return _contextStore;
      }
    }

    private Dictionary<string, ISession> ContextSessions
    {
      get
      {
        if (ContextStore == null)
          throw new ApplicationException("No ContextStore has been configured for NHibernateSessionManager. Please make sure the object container contains an implementation of INHibernateContextStore.");
        if (ContextStore.GetData(SESSION_KEY) == null)
          ContextStore.SetData(SESSION_KEY, new Dictionary<string, ISession>());

        return (Dictionary<string, ISession>)ContextStore.GetData(SESSION_KEY);
      }
    }

    private Dictionary<string, ISessionFactory> sessionFactories = new Dictionary<string, ISessionFactory>();
    private Dictionary<string, Configuration> configurations = new Dictionary<string, Configuration>();
    private const string SESSION_KEY = "NHIBERNATE_SESSIONS";
  }
}
