using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugWeb.Security;

namespace BugWeb.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
          /* if (!SecurityProvider.isGrower(Session) && !SecurityProvider.isAdmin(Session))
               return RedirectToAction("login","home");*/
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
            if (!SecurityProvider.isGrower(Session)) 
                return View("~/Views/Shared/Error.cshtml");
                return RedirectToAction("index", "farmmanagement");
        }

        public ActionResult RecoverAccount()
        {
            //return RedirectToAction("login", "home");
            return View("~/Views/Authentication/RecoverAccount.cshtml");
        }

        public ActionResult ChangePassword()
        {
            return View("~/Views/Authentication/ChangePassword.cshtml");
        }


        public ActionResult viewMap()
        {
            return PartialView("_HeatMap");
        }
    }
}