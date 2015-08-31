using BugBusiness.Interface.BugReporting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BugWeb.Controllers
{
    
    public class TestController : ApiController
    {
        private readonly IBugReporting _bugReporting;

        public TestController(IBugReporting bugReporting)
        {
            _bugReporting = bugReporting;
        }

        public void Get(long id)
        {
            Debug.WriteLine("Work");
        }
    }
}
