using Corcovado.WebApi.Interfaces;
using Corcovado.WebApi.Repositorios;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace Corcovado.App
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<ILog, RLog>();

            container.RegisterType<IEAIS, REAIS>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}