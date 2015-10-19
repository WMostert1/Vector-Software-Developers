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
       // GET: reporting/tables
        [Authenticate(Roles = "1, 2", Alternate = true)]
        public ActionResult Tables()
        {
            return View(new ReportingViewModel(Session));
        }

        // GET: reporting/charts
        [Authenticate(Roles = "1, 2", Alternate = true)]
        public ActionResult Charts()
        {
            return View(new ReportingViewModel(Session));
		}

        // GET: reporting/map
        [Authenticate(Roles = "1, 2", Alternate = true)]
        public ActionResult Map()
        {
            return View(new ReportingViewModel(Session));
        }
    }
}