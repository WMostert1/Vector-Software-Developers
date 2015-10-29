using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BugBusiness.Interface.FarmManagement;
using BugBusiness.Interface.FarmManagement.DTO;
using BugBusiness.Interface.FarmManagement.Exceptions;

namespace BugWeb.Controllers
{
    public class ApiTreatmentController : ApiController
    {
        [RoutePrefix("api/treatments")]
        public class apiTreatmentsController : ApiController
        {
            private readonly IFarmManagement _farmManagement;

            public apiTreatmentsController(IFarmManagement farmManagement)
            {
                _farmManagement = farmManagement;
            }

            [Route("{id}")]
            public GetTreatmentInfoResponse Get(long id)
            {
                return _farmManagement.GetTreatmentInfo(id);
            }

            [Route("")]
            public AddTreatmentResponse Post(AddTreatmentRequest addTreatmentRequest)
            {
                try
                {
                    return _farmManagement.AddTreatment(addTreatmentRequest);
                }
                catch (InvalidInputException)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

            }

        }
    }
}
