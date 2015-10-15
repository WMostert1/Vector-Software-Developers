using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
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
using BugBusiness.BugIntelligence;

namespace BugWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(WebApiConfig.Register);

            AutoFacConfig.RegisterIoc();
            AutoMapperConfig.RegisterMappings();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);          
        }
    }
}
