using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BugBusiness.Interface.BugReporting;
using BugBusiness.Interface.BugReporting.DTO;

namespace BugWeb.Controllers
{
    [RoutePrefix("api/species")]
    public class ApiSpeciesController : ApiController
    {
        private readonly IBugReporting _bugReporting;

        public ApiSpeciesController(IBugReporting bugReporting)
        {
            _bugReporting = bugReporting;
        }

        [Route("")]
        public GetSpeciesResponse Get()
        {
            return _bugReporting.GetSpecies();
        }
    }
}