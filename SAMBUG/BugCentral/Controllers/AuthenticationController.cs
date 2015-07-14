using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using DataAccess.Interface.DTOModels;
using DataAccess.MSSQL;

namespace BugCentral.Controllers
{
    [RoutePrefix("api")]
    public class AuthenticationController : ApiController
    {               
        // POST api/authentication/login/
        [HttpPost]
        [Route("login")]
        public LoginResponse Login(LoginRequest loginRequest)
        {
            var authentication = new Authentication();
            return authentication.Login(loginRequest);
        }


    }
}
