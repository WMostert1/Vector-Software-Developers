using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using BugBusiness.BugSecurity;
using BugBusiness.Interface.BugSecurity;
using BugBusiness.BugReporting;
using BugBusiness.Interface.BugReporting;
using DataAccess.Interface;
using DataAccess.MSSQL;

namespace BugCentral
{
    public static class AutoFacConfig
    {
        public static void RegisterIoc()
        {
            // Acquire container builder
            ContainerBuilder builder = new ContainerBuilder();
            
            // Register Web API controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register other types
            builder.RegisterType<DbBugSecurity>().As<IDbBugSecurity>();
            builder.RegisterType<BugSecurity>().As<IBugSecurity>();

            builder.RegisterType<DbBugReporting>().As<IDbBugReporting>();
            builder.RegisterType<BugReporting>().As<IBugReporting>();

            // Get HttpConfiguration
            var config = GlobalConfiguration.Configuration;

            // Set AutoFac to be the dependency resolver
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);   
        }
    }
}