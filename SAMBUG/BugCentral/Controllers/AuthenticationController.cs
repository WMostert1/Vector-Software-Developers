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
    public class AuthenticationController : ApiController
    {               
        // POST api/authentication/login/
        [HttpPost]
        public LoginResponse Login(LoginRequest loginRequest)
        {
            var authentication = new Authentication();

            LoginResponse loginResponse = authentication.Login(loginRequest);

            if (loginResponse == null)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);     
            }

            return loginResponse;
        }


    }
}
