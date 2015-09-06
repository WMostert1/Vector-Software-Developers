using BugBusiness.Interface.BugReporting;
using BugBusiness.Interface.BugReporting.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace BugWeb.Controllers
{
    [RoutePrefix("api/records")]
    public class ApiRecordsController : ApiController
    {
        private readonly IBugReporting _bugReporting;

        public ApiRecordsController(IBugReporting bugReporting)
        {
            _bugReporting = bugReporting;
        }

        [Route("{id}")]
        public GetCapturedDataResponse Get(long id)
        {
            var response = _bugReporting.GetCapturedData(new GetCapturedDataRequest() { FarmId = id });

            if (response == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return response;
        }

    }
}
