using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugWeb.Models;
using BugBusiness.Interface.BugSecurity.DTO;
using BugWeb.Security;

namespace BugWeb.Controllers
{
    public class ReportingController : Controller
    {
        // GET: reporting
        public ActionResult Index()
        {
            return View();
        }

        // GET: reporting/tabular
        public ActionResult Tabular()
        {
            return View(new ReportingViewModel(Session));
        }

        // GET: reporting/charts
        //todo this is just a demo of the new authentication functionality
        [Authenticate(Roles = "1, 2")]
        public ActionResult Charts()
        {
            return View(new ReportingViewModel(Session));
		}

        public ActionResult Map()
        {
            return View(new ReportingViewModel(Session));
        }
    }
}