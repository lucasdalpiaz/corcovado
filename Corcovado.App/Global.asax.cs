using Corcovado.App.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private System.Threading.Timer timer;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ProcessaEAIS();
            RegistroParaLogPorcentagem();
            RegistraLogPorcentagem();

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

        public void RegistroParaLogPorcentagem()
        {
            var t = Task.Run(async delegate
            {
                while (true)
                {
                    LogController.RegistraLogComDiferencaMaiorQue(20);
                    await Task.Delay(TimeSpan.FromMinutes(2));
                }
            });
        }

        public void RegistraLogPorcentagem()
        {
            var t = Task.Run(async delegate
            {
                while (true)
                {
                    LogController.RegistraLogPorcentagem(2);
                    await Task.Delay(TimeSpan.FromMinutes(3));
                }
            });
        }

        //private void SetUpTimer(TimeSpan alertTime)
        //{
        //    DateTime current = DateTime.Now;
        //    TimeSpan timeToGo = alertTime - current.TimeOfDay;
        //    if (timeToGo < TimeSpan.Zero)
        //    {
        //        return;//time already passed
        //    }
        //    this.timer = new System.Threading.Timer(x =>
        //    {
        //        this.SomeMethodRunsAt1600();
        //    }, null, timeToGo, Timeout.InfiniteTimeSpan);
        //}
    }
}
