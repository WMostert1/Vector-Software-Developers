using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using BugBusiness.Interface.BugSecurity;
using BugBusiness.Interface.BugSecurity.DTO;
using BugBusiness.Interface.BugSecurity.Exceptions;
using Newtonsoft.Json.Linq;


namespace BugWeb.Controllers
{
    [RoutePrefix("api/authentication")]
    public class ApiAuthenticationController : ApiController
    {
        private readonly IBugSecurity _bugSecurity;

        public ApiAuthenticationController(IBugSecurity bugSecurity)
        {
            _bugSecurity = bugSecurity;
        }

       [HttpPost]
        [Route("login")]
        public LoginResponse Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                LoginResponse loginResponse = _bugSecurity.Login(loginRequest);
                return loginResponse;
            }
            catch (NotRegisteredException)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost]
        [Route("register")]
        public RegisterResponse Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                RegisterResponse registerResponse = _bugSecurity.Register(registerRequest);
                return registerResponse;
            }
            catch (UserExistsException)
            {
                throw new HttpResponseException(HttpStatusCode.PreconditionFailed);
            }
        }

        [HttpPost]
        public void RecoverAccount(RecoverAccountRequest recoverAccountRequest)
        {
            _bugSecurity.RecoverAccount(recoverAccountRequest);

        }

       
    }
}
