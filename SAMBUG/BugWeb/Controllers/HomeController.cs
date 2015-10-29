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
        
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
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

        public ActionResult AboutUs()
        {
            return View();
        }

        public FileResult Download()
        {
            string app_data_path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
            string fileName = "UserManual.pdf";

            byte[] fileBytes = System.IO.File.ReadAllBytes(app_data_path+"\\Documents\\"+fileName);
            
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        
    }
}