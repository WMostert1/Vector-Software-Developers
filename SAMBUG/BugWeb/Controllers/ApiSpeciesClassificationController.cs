using BugBusiness.Interface.BugIntelligence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BugBusiness.Interface.BugIntelligence.DTO;

namespace BugWeb.Controllers
{
    [RoutePrefix("api/classification")]
    public class ApiSpeciesClassificationController : ApiController
    {
        private readonly IBugIntelligence _bugIntelligence;

        public ApiSpeciesClassificationController(IBugIntelligence bugIntelligence)
        {
            _bugIntelligence = bugIntelligence;
        }

        [HttpPost]
        public ClassifyResult Post([FromBody] ClassifyRequest request)
        {
           
            try
            {
                return _bugIntelligence.classify(BugBusiness.ExtensionMethods.DataConversion.sbyteToByteArray(request.FieldPicture));
            }
            catch (Exception e)
            {
                return new ClassifyResult();
            }

        }

    }
}
