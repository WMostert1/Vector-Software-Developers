using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BugBusiness.Interface.BugSecurity;
using BugBusiness.Interface.BugSecurity.DTO;

namespace BugCentral.Controllers
{
    public class AuthenticationController : ApiController
    {

        private readonly IBugSecurity _bugSecurity;


        public AuthenticationController(IBugSecurity bugSecurity)
        {
            _bugSecurity = bugSecurity;
        }
   
        // POST api/authentication/login/
        [HttpPost]
        public LoginResponse Login(LoginRequest loginRequest)
        {
            

           /* if (loginResponse == null)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);     
            }

            return loginResponse;*/

            return null;
        }


    }
}
