using System;
using System.Web;
using System.Web.Mvc;


namespace Xyperico.Web.Mvc
{
  // Thanks to Kazi Manzur Rashid
  // From http://kazimanzurrashid.com/posts/asp-dot-net-mvc-theme-supported-razor-view-engine
  // License: Creative Commons Attribution 3.0 License.


  /// <summary>
  /// This view engine will load views as normally unless a theme specific view has been put into a "Themes" folder.
  /// </summary>
  public class ThemeableRazorViewEngine : ThemeableVirtualPathProviderViewEngine
  {
    public ThemeableRazorViewEngine(Func<HttpContextBase, string> themeSelector)
      : base(themeSelector)
    {
      AreaViewLocationFormats = new[] {
                                                "~/Themes/{3}/Areas/{2}/Views/{1}/{0}.cshtml",
                                                "~/Themes/{3}/Areas/{2}/Views/{1}/{0}.vbhtml",
                                                "~/Themes/{3}/Areas/{2}/Views/Shared/{0}.cshtml",
                                                "~/Themes/{3}/Areas/{2}/Views/Shared/{0}.vbhtml",

                                                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                                                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                                "~/Areas/{2}/Views/Shared/{0}.vbhtml"
                                            };

      AreaMasterLocationFormats = new[] {
                                                  "~/Themes/{3}/Areas/{2}/Views/{1}/{0}.cshtml",
                                                  "~/Themes/{3}/Areas/{2}/Views/{1}/{0}.vbhtml",
                                                  "~/Themes/{3}/Areas/{2}/Views/Shared/{0}.cshtml",
                                                  "~/Themes/{3}/Areas/{2}/Views/Shared/{0}.vbhtml",

                                                  "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                  "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                                                  "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                                  "~/Areas/{2}/Views/Shared/{0}.vbhtml"
                                              };

      AreaPartialViewLocationFormats = new[] {
                                                       "~/Themes/{3}/Areas/{2}/Views/{1}/{0}.cshtml",
                                                       "~/Themes/{3}/Areas/{2}/Views/{1}/{0}.vbhtml",
                                                       "~/Themes/{3}/Areas/{2}/Views/Shared/{0}.cshtml",
                                                       "~/Themes/{3}/Areas/{2}/Views/Shared/{0}.vbhtml",

                                                       "~/Areas/{2}/Views/{1}/{0}.cshtml",
                                                       "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                                                       "~/Areas/{2}/Views/Shared/{0}.cshtml",
                                                       "~/Areas/{2}/Views/Shared/{0}.vbhtml"
                                                   };

      ViewLocationFormats = new[] {
                                            "~/Themes/{2}/Views/{1}/{0}.cshtml",
                                            "~/Themes/{2}/Views/{1}/{0}.vbhtml",
                                            "~/Themes/{2}/Views/Shared/{0}.cshtml",
                                            "~/Themes/{2}/Views/Shared/{0}.vbhtml",

                                            "~/Views/{1}/{0}.cshtml",
                                            "~/Views/{1}/{0}.vbhtml",
                                            "~/Views/Shared/{0}.cshtml",
                                            "~/Views/Shared/{0}.vbhtml"
                                        };

      MasterLocationFormats = new[] {
                                              "~/Themes/{2}/Views/{1}/{0}.cshtml",
                                              "~/Themes/{2}/Views/{1}/{0}.vbhtml",
                                              "~/Themes/{2}/Views/Shared/{0}.cshtml",
                                              "~/Themes/{2}/Views/Shared/{0}.vbhtml",

                                              "~/Views/{1}/{0}.cshtml",
                                              "~/Views/{1}/{0}.vbhtml",
                                              "~/Views/Shared/{0}.cshtml",
                                              "~/Views/Shared/{0}.vbhtml"
                                          };

      PartialViewLocationFormats = new[] {
                                                   "~/Themes/{2}/Views/{1}/{0}.cshtml",
                                                   "~/Themes/{2}/Views/{1}/{0}.vbhtml",
                                                   "~/Themes/{2}/Views/Shared/{0}.cshtml",
                                                   "~/Themes/{2}/Views/Shared/{0}.vbhtml",

                                                   "~/Views/{1}/{0}.cshtml",
                                                   "~/Views/{1}/{0}.vbhtml",
                                                   "~/Views/Shared/{0}.cshtml",
                                                   "~/Views/Shared/{0}.vbhtml"
                                               };

      ViewStartFileExtensions = new[] { "cshtml", "vbhtml", };

      StylesheetLocationFormats = new[] {
                                            "~/Themes/{2}/Styles/{0}.css",
                                            "~/Styles/{0}.css"
                                        };

      AreaStylesheetLocationFormats = new[] {
                                            "~/Themes/{3}/Areas/{2}/Styles/{0}.css",
                                            "~/Areas/{2}/Styles/{0}.css"
                                        };

      ImageLocationFormats = new[] {
                                       "~/Themes/{2}/Images/{0}",
                                       "~/Images/{0}"
                                   };

      AreaImageLocationFormats = new[] {
                                         "~/Themes/{3}/Areas/{2}/Images/{0}",
                                         "~/Areas/{2}/Images/{0}"
                                       };
    }


    public string[] ViewStartFileExtensions { get; set; }


    protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
    {
      return new RazorView(controllerContext, partialPath, null, false, ViewStartFileExtensions);
    }


    protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
    {
      return new RazorView(controllerContext, viewPath, masterPath, true, ViewStartFileExtensions);
    }
  }
}
