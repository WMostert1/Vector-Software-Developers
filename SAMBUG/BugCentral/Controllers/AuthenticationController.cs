using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using DataAccess.Interface;
using DataAccess.Interface.DTOModels;
using DataAccess.MSSQL;

namespace BugCentral.Controllers
{
    public class AuthenticationController : ApiController
    {

        private readonly IDbAuthentication _dbAuthentication;

        public AuthenticationController(IDbAuthentication dbAuthentication)
        {
            _dbAuthentication = dbAuthentication;
        }
   
        // POST api/authentication/login/
        [HttpPost]
        public LoginResponse Login(LoginRequest loginRequest)
        {
            LoginResponse loginResponse = _dbAuthentication.GetUserIdRoles(loginRequest);

            if (loginResponse == null)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);     
            }

            return loginResponse;
        }


    }
}
