using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using DataAccess.Interface;
using DataAccess.MSSQL;

namespace BugCentral
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Acquire container builder
            var builder = new ContainerBuilder();
            
            // Register Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //register other types
            builder.RegisterType<DbAuthentication>().As<IDbAuthentication>();

            // Get HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Set AutoFac to be the dependency resolver.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
