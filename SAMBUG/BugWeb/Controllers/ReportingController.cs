using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            throw new NotImplementedException();
        }

        // GET: reporting/graphical
        public ActionResult Graphical()
        {
            throw new NotImplementedException();
        }
    }
}