using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using BugBusiness.BugSecurity;
using BugBusiness.Interface.BugSecurity;
using BugBusiness.BugReporting;
using BugBusiness.Interface.BugReporting;
using BugBusiness.FarmManagement;
using BugBusiness.Interface.FarmManagement;
using DataAccess.Interface;
using DataAccess.MSSQL;
using System.Web.Http;
using System.Reflection;
using BugBusiness.BugScouting;
using BugBusiness.Interface.BugScouting;

namespace BugWeb
{
    public static class AutoFacConfig
    {
        public static void RegisterIoc()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Add your registrations
            builder.RegisterType<DbBugSecurity>().As<IDbBugSecurity>();
            builder.RegisterType<BugSecurity>().As<IBugSecurity>();

            builder.RegisterType<DbBugReporting>().As<IDbBugReporting>();
            builder.RegisterType<BugReporting>().As<IBugReporting>();

            builder.RegisterType<DbFarmManagement>().As<IDbFarmManagement>();
            builder.RegisterType<FarmManagement>().As<IFarmManagement>();

            builder.RegisterType<DbScouting>().As<IDbScouting>();
            builder.RegisterType<BugScouting>().As<IBugScouting>();
            
            

            var container = builder.Build();

            // Set the dependency resolver for Web API.
            var webApiResolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;

            // Set the dependency resolver for MVC.
            var mvcResolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(mvcResolver);

            
        }
    }
}