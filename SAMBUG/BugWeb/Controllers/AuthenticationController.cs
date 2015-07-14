using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugWeb.Models;

namespace BugWeb.Controllers
{
    public class AuthenticationController : Controller
    {
        private WebClient webClient = new WebClient();

        // GET: Authentication
        public ActionResult Index()
        {
            return null;
        }

        public ActionResult Login(LoginViewModel loginViewModel)
        {

            var nameValCollection = new NameValueCollection();
            nameValCollection.Add("Username", loginViewModel.Username);
            nameValCollection.Add("Password", loginViewModel.Password); 

            webClient.UploadValues("api/authentication/login", nameValCollection);

            return null;
        }
    }
}