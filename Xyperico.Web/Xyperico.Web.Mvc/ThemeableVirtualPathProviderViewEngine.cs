using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using CuttingEdge.Conditions;


namespace Xyperico.Web.Mvc
{
  // Thanks to Kazi Manzur Rashid
  // From http://kazimanzurrashid.com/posts/asp-dot-net-mvc-theme-supported-razor-view-engine
  // License: Creative Commons Attribution 3.0 License.

  public abstract class ThemeableVirtualPathProviderViewEngine : IViewEngine
  {
    public Func<HttpContextBase, string> ThemeSelector { get; set; }

    public const string DefaultMasterName = "MasterLayout";

    public string[] AreaMasterLocationFormats { get; set; }

    public string[] AreaPartialViewLocationFormats { get; set; }

    public string[] AreaViewLocationFormats { get; set; }

    public string[] MasterLocationFormats { get; set; }

    public string[] PartialViewLocationFormats { get; set; }

    public string[] ViewLocationFormats { get; set; }

    public IViewLocationCache ViewLocationCache { get; set; }

    public string[] StylesheetLocationFormats { get; set; }

    public string[] AreaStylesheetLocationFormats { get; set; }

    public string[] ImageLocationFormats { get; set; }

    public string[] AreaImageLocationFormats { get; set; }


    #region IViewEngine

    public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
    {
      Condition.Requires(controllerContext, "controllerContext").IsNotNull();
      Condition.Requires(viewName, "viewName").IsNotNullOrEmpty();

      string[] viewLocationsSearched;
      string[] masterLocationsSearched;
      bool incompleteMatch = false;

      string controllerName = controllerContext.RouteData.GetRequiredString("controller");

      string viewPath = GetPath(controllerContext, ViewLocationFormats, AreaViewLocationFormats,
                                "ViewLocationFormats", viewName, controllerName, _cacheKeyPrefix_View, useCache,
        /* checkPathValidity */ true, ref incompleteMatch, out viewLocationsSearched);

      masterName = (string.IsNullOrEmpty(masterName) ? DefaultMasterName : masterName);

      string masterPath = GetPath(controllerContext, MasterLocationFormats, AreaMasterLocationFormats,
                                  "MasterLocationFormats", masterName, controllerName, _cacheKeyPrefix_Master,
                                  useCache, /* checkPathValidity */ false, ref incompleteMatch,
                                  out masterLocationsSearched);

      if (string.IsNullOrEmpty(viewPath) || (string.IsNullOrEmpty(masterPath) && !string.IsNullOrEmpty(masterName)))
      {
        return new ViewEngineResult(viewLocationsSearched.Union(masterLocationsSearched));
      }

      return new ViewEngineResult(CreateView(controllerContext, viewPath, masterPath), this);
    }


    public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
    {
      Condition.Requires(controllerContext, "controllerContext").IsNotNull();
      Condition.Requires(partialViewName, "partialViewName").IsNotNullOrEmpty();

      string[] searched;
      bool incompleteMatch = false;
      string controllerName = controllerContext.RouteData.GetRequiredString("controller");

      string partialPath = GetPath(controllerContext, PartialViewLocationFormats, AreaPartialViewLocationFormats,
                                   "PartialViewLocationFormats", partialViewName, controllerName,
                                   _cacheKeyPrefix_Partial, useCache, /* checkBaseType */ true,
                                   ref incompleteMatch, out searched);

      if (string.IsNullOrEmpty(partialPath))
      {
        return new ViewEngineResult(searched);
      }

      return new ViewEngineResult(CreatePartialView(controllerContext, partialPath), this);
    }


    public void ReleaseView(ControllerContext controllerContext, IView view)
    {
      var disposable = view as IDisposable;

      if (disposable != null)
      {
        disposable.Dispose();
      }
    }

    #endregion


    public string FindStylesheet(ControllerContext controllerContext, string stylesheet, bool useCache)
    {
      Condition.Requires(controllerContext, "controllerContext").IsNotNull();
      Condition.Requires(stylesheet, "stylesheet").IsNotNullOrEmpty();

      string[] searched;
      bool incompleteMatch = false;
      string controllerName = controllerContext.RouteData.GetRequiredString("controller");

      string cssPath = GetPath(controllerContext, StylesheetLocationFormats, AreaStylesheetLocationFormats,
                               "StylesheetLocationFormats", stylesheet, controllerName,
                               _cacheKeyPrefix_Stylesheet, useCache, /* checkBaseType */ true,
                               ref incompleteMatch, out searched);

      return cssPath;
    }


    public string FindImageUrl(ControllerContext controllerContext, string image, bool useCache)
    {
      Condition.Requires(controllerContext, "controllerContext").IsNotNull();
      Condition.Requires(image, "image").IsNotNullOrEmpty();

      string[] searched;
      bool incompleteMatch = false;
      string controllerName = controllerContext.RouteData.GetRequiredString("controller");

      string imagePath = GetPath(controllerContext, ImageLocationFormats, AreaImageLocationFormats,
                                 "ImageLocationFormats", image, controllerName,
                                 _cacheKeyPrefix_Image, useCache, /* checkBaseType */ true,
                                 ref incompleteMatch, out searched);

      return imagePath;
    }


    #region 

    //public string FindMasterLayout(ControllerContext controllerContext)
    //{
    //}

    #endregion


    #region Internals

    // format is ":ViewCacheEntry:{cacheType}:{theme}:{prefix}:{name}:{controllerName}:{areaName}:"
    private const string _cacheKeyFormat = ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}:{5}:";
    private const string _cacheKeyPrefix_Master = "Master";
    private const string _cacheKeyPrefix_Partial = "Partial";
    private const string _cacheKeyPrefix_View = "View";
    private const string _cacheKeyPrefix_Stylesheet = "Style";
    private const string _cacheKeyPrefix_Image = "Image";

    private static readonly string[] _emptyLocations = new string[0];


    private VirtualPathProvider _vpp;
    protected VirtualPathProvider VirtualPathProvider
    {
      get
      {
        return _vpp ?? (_vpp = HostingEnvironment.VirtualPathProvider);
      }
      set
      {
        _vpp = value;
      }
    }


    protected ThemeableVirtualPathProviderViewEngine(Func<HttpContextBase, string> themeSelector)
    {
      Condition.Requires(themeSelector, "themeSelector").IsNotNull();
      ViewLocationCache = DefaultViewLocationCache.Null;
      ThemeSelector = themeSelector;
    }


    private string GetPath(ControllerContext controllerContext, 
                           IEnumerable<string> locations, IEnumerable<string> areaLocations, 
                           string locationsPropertyName, string name, string controllerName, string cacheKeyPrefix, 
                           bool useCache, bool checkPathValidity, 
                           ref bool incompleteMatch, out string[] searchedLocations)
    {
      searchedLocations = _emptyLocations;

      if (string.IsNullOrEmpty(name))
        return string.Empty;

      string areaName = GetAreaName(controllerContext.RouteData);
      bool usingAreas = !string.IsNullOrEmpty(areaName);

      string theme = ThemeSelector(controllerContext.HttpContext);
      if (theme == null)
        throw new InvalidOperationException("No theme supplied by theme selector (returned null).");

      List<ViewLocation> viewLocations = GetViewLocations(locations, (usingAreas) ? areaLocations : null);

      if (viewLocations.Count == 0)
        throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The property '{0}' cannot be null or empty.", locationsPropertyName));

      bool nameRepresentsPath = IsSpecificPath(name);

      string cacheKey = CreateCacheKey(theme, cacheKeyPrefix, name, (nameRepresentsPath) ? string.Empty : controllerName, areaName);

      if (useCache)
      {
        return ViewLocationCache.GetViewLocation(controllerContext.HttpContext, cacheKey);
      }

      return nameRepresentsPath ?
             GetPathFromSpecificName(controllerContext, name, cacheKey, checkPathValidity, ref searchedLocations, ref incompleteMatch) :
             GetPathFromGeneralName(controllerContext, viewLocations, name, controllerName, areaName, theme, cacheKey, ref searchedLocations);
    }


    private static string GetAreaName(RouteData routeData)
    {
      object area;

      if (routeData.DataTokens.TryGetValue("area", out area))
      {
        return area as string;
      }

      return GetAreaName(routeData.Route);
    }


    private static string GetAreaName(RouteBase route)
    {
      IRouteWithArea routeWithArea = route as IRouteWithArea;

      if (routeWithArea != null)
      {
        return routeWithArea.Area;
      }

      Route castRoute = route as Route;

      if (castRoute != null && castRoute.DataTokens != null)
      {
        return castRoute.DataTokens["area"] as string;
      }

      return null;
    }


    private static bool IsSpecificPath(string name)
    {
      char c = name[0];

      return (c == '~' || c == '/');
    }


    private static List<ViewLocation> GetViewLocations(IEnumerable<string> viewLocationFormats, IEnumerable<string> areaViewLocationFormats)
    {
      var allLocations = new List<ViewLocation>();

      if (areaViewLocationFormats != null)
      {
        allLocations.AddRange(areaViewLocationFormats.Select(areaViewLocationFormat => new AreaAwareViewLocation(areaViewLocationFormat)));
      }

      if (viewLocationFormats != null)
      {
        allLocations.AddRange(viewLocationFormats.Select(viewLocationFormat => new ViewLocation(viewLocationFormat)));
      }

      return allLocations;
    }


    private string CreateCacheKey(string theme, string prefix, string name, string controllerName, string areaName)
    {
      return string.Format(CultureInfo.InvariantCulture, _cacheKeyFormat, GetType().AssemblyQualifiedName, theme, prefix, name, controllerName, areaName);
    }


    private string GetPathFromGeneralName(ControllerContext controllerContext, IList<ViewLocation> locations, string name, string controllerName, string areaName, string theme, string cacheKey, ref string[] searchedLocations)
    {
      string result = string.Empty;
      searchedLocations = new string[locations.Count];

      for (int i = 0; i < locations.Count; i++)
      {
        ViewLocation location = locations[i];
        string virtualPath = location.Format(name, controllerName, areaName, theme);

        if (FileExists(controllerContext, virtualPath))
        {
          searchedLocations = _emptyLocations;
          result = virtualPath;
          ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, result);
          break;
        }

        searchedLocations[i] = virtualPath;
      }

      return result;
    }

    private string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, bool checkPathValidity, ref string[] searchedLocations, ref bool incompleteMatch)
    {
      string result = name;
      bool fileExists = FileExists(controllerContext, name);

      if (checkPathValidity && fileExists)
      {
        bool? validPath = IsValidPath(controllerContext, name);

        if (validPath == false)
        {
          fileExists = false;
        }
        else if (validPath == null)
        {
          incompleteMatch = true;
        }
      }

      if (!fileExists)
      {
        result = string.Empty;
        searchedLocations = new[] { name };
      }

      if (!incompleteMatch)
      {
        ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, result);
      }

      return result;
    }


    protected virtual bool FileExists(ControllerContext controllerContext, string virtualPath)
    {
      return VirtualPathProvider.FileExists(virtualPath);
    }


    protected virtual bool? IsValidPath(ControllerContext controllerContext, string virtualPath)
    {
      return null;
    }

    #endregion


    #region Internal helper classes

    class AreaAwareViewLocation : ViewLocation
    {
      public AreaAwareViewLocation(string virtualPathFormatString)
        : base(virtualPathFormatString)
      {
      }

      public override string Format(string viewName, string controllerName, string areaName, string theme)
      {
        return string.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName, areaName, theme);
      }
    }


    class ViewLocation
    {
      protected readonly string _virtualPathFormatString;

      public ViewLocation(string virtualPathFormatString)
      {
        _virtualPathFormatString = virtualPathFormatString;
      }

      public virtual string Format(string viewName, string controllerName, string areaName, string theme)
      {
        return string.Format(CultureInfo.InvariantCulture, _virtualPathFormatString, viewName, controllerName, theme);
      }
    }


    #endregion


    #region To be implemented by implementors

    protected abstract IView CreatePartialView(ControllerContext controllerContext, string partialPath);

    protected abstract IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath);

    #endregion
  }
}
