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
    [RoutePrefix("api/blocks")]
    public class ApiBlockController : ApiController
    {
        private readonly IFarmManagement _farmManagement;

        public ApiBlockController(IFarmManagement farmManagement)
        {
            _farmManagement = farmManagement;
        }

        [Route("")]
        public AddBlockResponse Post(AddBlockRequest addBlockRequest)
        {
            try
            {
                return _farmManagement.AddBlock(addBlockRequest);
            }
            catch (InvalidInputException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            catch (BlockExistsException)
            {
                throw new HttpResponseException(HttpStatusCode.Conflict);
            }

        }

        [Route("{id}")]
        public DeleteBlockByIDResponse Delete(long id)
        {
            try
            {
                return _farmManagement.DeleteBlockByID(id);
            }
            catch (InvalidInputException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            catch (CouldNotDeleteBlockException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

        }

        [Route("")]
        public UpdateBlockByIDResponse Put(UpdateBlockByIDRequest updateBlockByIdRequest)
        {
            try
            {
                return _farmManagement.UpdateBlockByID(updateBlockByIdRequest);
            }
            catch (InvalidInputException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            catch (CouldNotUpdateException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

        }
    }
}
