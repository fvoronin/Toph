using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Toph.Common;
using WebMatrix.WebData;
using log4net.Config;

namespace Toph.UI
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            XmlConfigurator.Configure();

            var version = Assembly
                .GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;

            Application["version"] = version;
            Application["versionUrl"] = "https://github.com/rtennys/Toph/commit/{0}".F(version.Split('.').Last());
            Application["name"] = "Toph";

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            IocConfig.Initialize();

            WebSecurity.InitializeDatabaseConnection("toph_conn", "UserProfile", "Id", "Username", true);
        }
    }
}
