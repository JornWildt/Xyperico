using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Reflection;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace Xyperico.Base.Reflection
{
  public static class TypeScanner
  {
    public static ILog Logger = LogManager.GetLogger(typeof(TypeScanner));


    public static IEnumerable<Type> ScanForTypesOf(Type type, IEnumerable<Assembly> assemblies = null)
    {
      List<Type> types = new List<Type>();
      Logger.DebugFormat("Scanning assemblies for type {0}", type);
      foreach (Assembly a in (assemblies ?? AppDomain.CurrentDomain.GetAssemblies()))
      {
        try
        {
          Logger.DebugFormat("Scanning assembly {0}", a.FullName);
          foreach (Type t in (from t2 in a.GetTypes() 
                              where type.IsAssignableFrom(t2) && !t2.IsAbstract
                              select t2))
          {
            Logger.DebugFormat("Found type {0}", t);
            types.Add(t);
          }
        }
        catch (Exception ex)
        {
          Logger.Info(string.Format("Could load types in assembly {0}.", a.FullName), ex);
        }
      }

      return types;
    }


    public static IEnumerable<T> ScanForAndInstantiateTypesOf<T>(IObjectContainer container, 
                                                                 object namedConstructorArgs = null, 
                                                                 IEnumerable<Assembly> assemblies = null)
    {
      IList<Type> types = ScanForTypesOf(typeof(T), assemblies).ToList();

      // Register all types first
      foreach (Type t in types)
      {
        if (!container.HasComponent(t))
          container.AddTransientComponent(t, t);
      }

      // Instantiate each type (now that it's potential dependencies has been registered)
      foreach (Type t in types)
      {
        yield return (T)ObjectContainer.Instantiate(t, namedConstructorArgs, container);
      }
    }
  }
}
