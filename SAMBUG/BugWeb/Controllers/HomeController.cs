using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugWeb.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return RedirectToAction("login", "home");
        }

        public ActionResult Login()
        {
            @ViewBag.Title = "Login Page";
            return View("~/Views/Authentication/Login.cshtml");
        }

        public ActionResult Register()
        {
            @ViewBag.Title = "Register Page";
            return View("~/Views/Authentication/Register.cshtml");
        }
    }
}