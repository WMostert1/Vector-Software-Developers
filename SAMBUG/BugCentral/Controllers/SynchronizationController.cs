using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using BugBusiness.Interface.BugSecurity;
using BugBusiness.Interface.BugSecurity.DTO;
using BugBusiness.Interface.BugSecurity.Exceptions;

namespace BugCentral.Controllers
{
    [RoutePrefix("api/synchronization")]
    public class SynchronizationController
    {
        public SynchronizationController()
        {
            _bugSecurity = bugSecurity;
        }
    }
}