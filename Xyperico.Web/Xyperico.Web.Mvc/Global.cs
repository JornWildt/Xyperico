using System;
using System.Web.Mvc;
using System.Web.Routing;
using log4net;
using Xyperico.Base;
using Xyperico.Base.Collections;
using Xyperico.Web.Mvc.Implementation;
using System.Reflection;
using System.Web.Compilation;
using System.IO;
using System.Globalization;


namespace Xyperico.Web.Mvc
{
  public class Global : System.Web.HttpApplication
  {
    ILog Logger = LogManager.GetLogger(typeof(Global));


    protected virtual void Application_Start()
    {
      log4net.Config.XmlConfigurator.Configure();
      Logger.Debug("**************************************");
      Logger.Debug("Starting application");
      Logger.Debug("**************************************");

      DependencyResolver.SetResolver(new XypericoDependencyResolver(Xyperico.Base.ObjectContainer.Container));
      ConfigureContainer(Xyperico.Base.ObjectContainer.Container);

      //RouteTable.Routes.MapRoute(
      //  "Area_Styles",
      //  "Areas",

      Logger.Debug("Register all areas");
      AreaRegistration.RegisterAllAreas();

      if (Configuration.Settings.ActivateRouteDebugging)
        RouteDebug.PreApplicationStart.Start();
        //RouteDebug..RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);

      Logger.Debug("System ready to run");
    }


    public static void ConfigureContainer(IObjectContainer container)
    {
      container.AddComponent<IApplicationBaseAddress, WebContextApplicationBaseUrl>();
      container.AddComponent<INameValueContextCollection, CallContextNamedValueCollection>();
      container.RegisterInstance<IObjectResolver>(Xyperico.Base.ObjectContainer.Container);
    }


    protected void Application_Error(object sender, EventArgs e)
    {
      Exception ex = Server.GetLastError();
      Logger.Error("Got exception", ex);
    }


    protected void LoadConfig()
    {
      System.Configuration.
    }
  }
}