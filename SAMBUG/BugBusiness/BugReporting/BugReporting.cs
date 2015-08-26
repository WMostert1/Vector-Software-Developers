using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugBusiness.Interface.BugReporting;
using BugBusiness.Interface.BugReporting.DTO;
using DataAccess.Interface;
using DataAccess.Models;

namespace BugBusiness.BugReporting
{
    public class BugReporting : IBugReporting
    {
        private readonly IDbBugReporting _dbBugReporting;

        public BugReporting(IDbBugReporting dbBugReporting)
        {
            _dbBugReporting = dbBugReporting;
        }

        public GetCapturedDataResponse GetCapturedData(GetCapturedDataRequest getCapturedDataRequest)
        {
            List<ScoutStop> scoutStops = _dbBugReporting.GetScoutStopsByFarmId(getCapturedDataRequest.FarmId);
            List<ScoutStopDto> scoutStopsDto = AutoMapper.Mapper.Map<List<ScoutStopDto>>(scoutStops);
            
            List<Treatment> treatments = _dbBugReporting.GetTreatmentsByFarmId(getCapturedDataRequest.FarmId);
            List<TreatmentDto> treatmentsDto = AutoMapper.Mapper.Map<List<TreatmentDto>>(treatments);

            return new GetCapturedDataResponse()
            {
                FarmName = scoutStops[0].Block.Farm.FarmName,
                ScoutStops = scoutStopsDto,
                Treatments = treatmentsDto
            };
        }
    }
}
