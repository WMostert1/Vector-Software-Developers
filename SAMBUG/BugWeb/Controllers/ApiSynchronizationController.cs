using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http.Results;
using BugBusiness.Interface.BugSecurity;
using BugBusiness.Interface.BugSecurity.DTO;
using BugBusiness.Interface.BugSecurity.Exceptions;
using BugBusiness.Interface.BugScouting.DTO;
using Newtonsoft.Json.Linq;
using DataAccess.Interface;
using System.Web;
using System.Text;
using System.Reflection;
using System.IO;
using System.Net.Http;
using System.Web.Http;
using BugBusiness.Interface.BugScouting;

namespace BugWeb.Controllers
{
    [RoutePrefix("api/synchronization")]
    public class ApiSynchronizationController : ApiController
    {
        private readonly IBugScouting _bugScouting;

        public ApiSynchronizationController(IBugScouting bugScouting)
        {
            _bugScouting = bugScouting;
        }

        [HttpPost]
        [Route("")]
         public HttpResponseMessage PersistCachedData([FromBody] SyncRequest request)
        {
            //TODO: The Date isn't being parsed correctly. Need to look @ Android side Date representation
            try
            {
                    _bugScouting.persistScoutingData(request);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage(HttpStatusCode.Created);

        }

       
    }
}