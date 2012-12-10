using System;
using System.Web.Mvc;
using System.Web.Routing;
using LowercaseRoutesMVC4;

namespace Toph.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRouteLowercase("home index", "", new {controller = "home", action = "index"});
            routes.MapRouteLowercase("home about", "about", new {controller = "home", action = "about"});

            routes.MapRouteLowercase("default", "{controller}/{action}/{id}", new {action = "index", id = UrlParameter.Optional});
        }
    }
}
