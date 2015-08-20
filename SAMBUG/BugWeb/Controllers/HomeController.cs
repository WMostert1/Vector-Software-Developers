using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugWeb.Controllers
{
    public class HomeController : Controller
    {
        //TODO: Still need to check if user is logged in before returning view. Redirects to login page if not
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View("~/Views/Authentication/Login.cshtml");
        }

        public ActionResult Register()
        {    
            return View("~/Views/Authentication/Register.cshtml");
        }

        public ActionResult BlockEdit()
        {
            return RedirectToAction("index", "farmmanagement");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return View("~/Views/Authentication/Login.cshtml");
        }
    }
}