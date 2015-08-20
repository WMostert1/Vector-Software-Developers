using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using BugBusiness.BugSecurity;
using BugBusiness.Interface.BugSecurity;
using BugBusiness.FarmManagement;
using BugBusiness.Interface.FarmManagement;
using DataAccess.Interface;
using DataAccess.MSSQL;

namespace BugWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //build IoC container and register controllers
            var builder = new ContainerBuilder();

            // Register MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Regster other types
            builder.RegisterType<DbAuthentication>().As<IDbAuthentication>();
            builder.RegisterType<BugSecurity>().As<IBugSecurity>();
            builder.RegisterType<DbFarmManagement>().As<IDbFarmManagement>();
            builder.RegisterType<FarmManagement>().As<IFarmManagement>();

            // Acquire IoC Container
            var container = builder.Build();

            // Set AutoFac to be the dependency resolver.
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
