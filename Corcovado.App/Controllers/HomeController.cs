using Corcovado.Contexto;
using Corcovado.WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Corcovado.App.Controllers
{
    public class HomeController : Controller
    {
        private static bool liberar = true;
    
        public async Task<ActionResult> Index()
        {
            string server = "";
            if (DPSyncContext.servidor == 9)
            {
                server = "BASE DE DADOS: HOMOLOGAÇÃO";
            }else if(DPSyncContext.servidor == 1)
            {
                server = "BASE DE DADOS: PRODUÇÃO";
            }
            else
            {
                server = "BASE DE DADOS: CHAME O SUPORTE";

            }
            ViewBag.Servidor = server;
            ViewBag.Title = "Home Page";
            //LerXmlController _xmlController = new LerXmlController(IEAIS);
            //if (liberar)
            //{
            //    liberar = false;
            //    var t = Task.Run(async delegate
            //    {
            //        while (true)
            //        {
            //            await LerXmlController.LerXml();
            //            await Task.Delay(TimeSpan.FromMinutes(2));
            //        }
            //    });

            //}

            return View();
        }
    }
}
