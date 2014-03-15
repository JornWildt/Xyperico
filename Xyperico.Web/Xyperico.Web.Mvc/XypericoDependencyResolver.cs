using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Mvc;
using log4net;
using Xyperico.Base;


namespace Xyperico.Web.Mvc
{
  public class XypericoDependencyResolver : IDependencyResolver
  {
    static ILog Logger = LogManager.GetLogger(typeof(XypericoDependencyResolver));

    IObjectContainer Container;


    public XypericoDependencyResolver(IObjectContainer container)
    {
      Container = container;

      //
      //  Register all controller types so the container will be able to resolve them
      //
      Logger.Debug("Dependency resolver scanning for controllers");
      //foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
      foreach (Assembly a in BuildManager.GetReferencedAssemblies())
      {
        try
        {
          Logger.DebugFormat("Scanning assembly {0}", a.FullName);
          foreach (Type controllerType in (from t in a.GetTypes() where typeof(IController).IsAssignableFrom(t) select t))
          {
            Logger.DebugFormat("Adding {0} as a controller", controllerType);
            Container.AddTransientComponent(controllerType, controllerType);
            // OLD: AddComponentLifeStyle(controllerType.ToString(), controllerType, Castle.Core.LifestyleType.Transient);
          }
        }
        catch (ReflectionTypeLoadException ex)
        {
          Logger.Warn(string.Format("Could not load assembly {0}.", a.FullName), ex);
          foreach (Exception ex2 in ex.LoaderExceptions)
            Logger.Warn("Loader exception", ex2);

        }
        catch (Exception ex)
        {
          Logger.Warn(string.Format("Could not load assembly {0}.", a.FullName), ex);
        }
      }
    }


    #region IDependencyResolver Members

    public object GetService(Type serviceType)
    {
      try
      {
        return Container.Resolve(serviceType);
      }
      catch (Exception ex)
      {
        Logger.Warn(ex);
        return null;
      }
    }

    public IEnumerable<object> GetServices(Type serviceType)
    {
      foreach (object s in Container.ResolveAll(serviceType))
        yield return s;
    }

    #endregion
  }
}
