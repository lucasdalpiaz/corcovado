using Corcovado.App.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Corcovado.App
{
    public class WebApiApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ProcessaEAIS();

        }

        public void ProcessaEAIS()
        {
            var t = Task.Run(async delegate
            {
                while (true)
                {
                    await LerXmlController.LerXml();
                    await Task.Delay(TimeSpan.FromMinutes(2));
                }
            });
        }
    }
}
