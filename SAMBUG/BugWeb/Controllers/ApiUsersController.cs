using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BugBusiness.Interface.BugSecurity;
using BugBusiness.Interface.BugSecurity.DTO;

namespace BugWeb.Controllers
{
    [RoutePrefix("api/users")]
    public class ApiUsersController : ApiController
    {
            private readonly IBugSecurity _bugSecurity;

            public ApiUsersController(IBugSecurity bugSecurity)
            {
                _bugSecurity = bugSecurity;
            }

            [Route("")]
            public GetUsersResponse Get()
            {
                return _bugSecurity.GetUsers(); 
            }

            [Route("")]
            public EditUserRoleResponse Put(EditUserRoleRequest editUserRoleRequest)
            {
                return _bugSecurity.EditUserRoles(editUserRoleRequest);
            }
    }
}
