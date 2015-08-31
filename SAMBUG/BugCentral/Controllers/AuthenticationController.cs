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
using BugCentral.HelperClass;

namespace BugCentral.Controllers
{
    [RoutePrefix("api/authentication")]
    public class AuthenticationController : ApiController
    {

        private readonly IBugSecurity _bugSecurity;

        public AuthenticationController(IBugSecurity bugSecurity)
        {
            _bugSecurity = bugSecurity;
        }

        [Route("login")]
        public LoginResponse Post([FromBody] LoginRequest loginRequest)
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

        [Route("register")]
        public RegisterResponse Post([FromBody] RegisterRequest registerRequest)
        {
           try{
                RegisterResponse registerResponse = _bugSecurity.Register(registerRequest);
                return registerResponse;
            }
            catch (UserExistsException)
            {
                throw new HttpResponseException(HttpStatusCode.PreconditionFailed);
            }
        }

       /* [Route("recover")]
        public void Post([FromBody] RecoverAccountRequest recoverAccountRequest)
        {
            EmailSender es = new EmailSender("kaleabtessera@gmail.com");
        }*/
    }
}
