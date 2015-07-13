using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataAccess.Interface.DTOModels;

namespace BugCentral.Controllers
{
    [RoutePrefix("api")]
    public class AuthenticationController : ApiController
    {
        
        
        // POST api/authentication/login/
        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login(LoginRequest loginRequest)
        {
            return null;
        }

    }
}
