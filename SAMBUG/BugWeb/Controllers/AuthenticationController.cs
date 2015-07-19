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
            HttpContent requestContent = new StringContent(JsonConvert.SerializeObject(loginViewModel));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var postTask = _httpClient.PostAsync("http://localhost:53358/api/authentication/login", requestContent);
            postTask.RunSynchronously();
            
            var readResponseTask = postTask.Result.Content.ReadAsStringAsync();
            readResponseTask.Wait();

            String x = readResponseTask.Result;

           /* if(.....)*/
            return View("~/Views/Home/Home.cshtml");
        }
    }
}