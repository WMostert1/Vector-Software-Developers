using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Autofac;
using Autofac.Integration.Mvc;
using BugBusiness.BugSecurity;
using BugBusiness.Interface.BugSecurity;
using BugBusiness.BugReporting;
using BugBusiness.Interface.BugReporting;
using BugBusiness.FarmManagement;
using BugBusiness.Interface.FarmManagement;
using DataAccess.Interface;
using DataAccess.MSSQL;

namespace BugWeb
{
    public static class AutoFacConfig
    {
        public static void RegisterIoc()
        {
            //build IoC container and register controllers
            var builder = new ContainerBuilder();

            // Register MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register other types
            builder.RegisterType<DbBugSecurity>().As<IDbBugSecurity>();
            builder.RegisterType<BugSecurity>().As<IBugSecurity>();

            builder.RegisterType<DbBugReporting>().As<IDbBugReporting>();
            builder.RegisterType<BugReporting>().As<IBugReporting>();

            builder.RegisterType<DbFarmManagement>().As<IDbFarmManagement>();
            builder.RegisterType<FarmManagement>().As<IFarmManagement>();

            // Acquire IoC Container
            var container = builder.Build();

            // Set AutoFac to be the dependency resolver.
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}