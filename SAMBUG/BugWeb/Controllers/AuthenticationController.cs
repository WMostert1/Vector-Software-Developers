using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using BugWeb.Models;
using Newtonsoft.Json;

namespace BugWeb.Controllers
{
    public class AuthenticationController : Controller
    {
        /*private WebClient webClient = new WebClient();*/

        private readonly HttpClient _httpClient = new HttpClient();

        // GET: Authentication
        public ActionResult Index()
        {

            return null;
        }

        //TODO: Look at asynchronous calls
        public ActionResult Login(LoginViewModel loginViewModel)
        {
           
           /* if(.....)*/
            return View("~/Views/Home/Home.cshtml");
        }
    }
}