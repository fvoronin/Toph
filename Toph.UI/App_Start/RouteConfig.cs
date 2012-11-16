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

            routes.MapRouteLowercase("home", "", new {controller = "home", action = "index"});
            routes.MapRouteLowercase("about", "about", new {controller = "home", action = "about"});

            routes.MapRouteLowercase("account", "account/{action}/{id}", new {controller = "account", action = "index", id = UrlParameter.Optional});

            routes.MapRouteLowercase("user", "{username}/{action}", new {controller = "user", action = "index"}, new {action = "^(index|add)$"});

            routes.MapRouteLowercase("customer", "{username}/{customer}/{action}", new {controller = "customer", action = "index"});
        }
    }
}
