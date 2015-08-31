using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using BugBusiness.Interface.BugReporting;
using BugBusiness.Interface.BugReporting.DTO;

namespace BugCentral.Controllers
{
    [EnableCors(origins: "http://localhost:53249", headers: "*", methods: "*")]
    public class ReportingController : ApiController
    {
        private readonly IBugReporting _bugReporting;

        public ReportingController(IBugReporting bugReporting)
        {
            _bugReporting = bugReporting;
        }
        
        public GetCapturedDataResponse Get(long id)
        {
            var response = _bugReporting.GetCapturedData(new GetCapturedDataRequest(){FarmId = id});

            if (response == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return response;
        }
        
    }
}