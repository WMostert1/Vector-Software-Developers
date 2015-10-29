using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BugBusiness.Interface.BugSecurity;
using BugBusiness.Interface.BugSecurity.DTO;
using BugBusiness.Interface.FarmManagement;
using BugBusiness.Interface.FarmManagement.DTO;
using BugBusiness.Interface.FarmManagement.Exceptions;

namespace BugWeb.Controllers
{
    public class ApiFarmController : ApiController
    {
        [RoutePrefix("api/farms")]
        public class ApiFarmsController : ApiController
        {
            private readonly IFarmManagement _farmManagement;

            public ApiFarmsController(IFarmManagement farmManagement)
            {
                _farmManagement = farmManagement;
            }

            [Route("{id}")]
            public GetFarmsByUserIDResponse Get(long id)
            {
                return _farmManagement.GetFarmsByUserID(new GetFarmsByUserIDRequest(){id = id});
            }

            [Route("")]
            public AddFarmResponse Post(AddFarmRequest addFarmRequest)
            {
                try
                {
                    return _farmManagement.AddFarm(addFarmRequest);
                }
                catch (InvalidInputException)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
                catch (FarmExistsException)
                {
                    throw new HttpResponseException(HttpStatusCode.Conflict);
                }
                
            }

            [Route("{id}")]
            public DeleteFarmByIDResponse Delete(long id)
            {
                try
                {
                    return _farmManagement.DeleteFarmByID(id);
                }
                catch (InvalidInputException)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
                catch (CouldNotDeleteFarmException)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

            }

        }
    }
}
