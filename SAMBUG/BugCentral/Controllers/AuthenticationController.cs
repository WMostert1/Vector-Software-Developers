using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BugBusiness.Interface.BugSecurity;
using BugBusiness.Interface.BugSecurity.DTO;
using BugBusiness.Interface.BugSecurity.Exceptions;

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
            try
            {
                LoginResponse loginResponse = Login(loginRequest);
                return loginResponse;
            }
            catch (NotRegisteredException)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);  
            }
        }

        [HttpPost]
        public RegisterResponse Register(RegisterRequest registerRequest)
        {
           try{
                RegisterResponse registerResponse = Register(registerRequest);
                return registerResponse;
            }
            catch (UserExistsException)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
        }
    }
}
