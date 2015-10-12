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

        public List<Species> getAllSpecies()
        {
            return _dbBugReporting.getAllSpecies();
        }

        public GetCapturedDataResponse GetCapturedData(GetCapturedDataRequest getCapturedDataRequest)
        {
            List<ScoutStop> scoutStops = _dbBugReporting.GetScoutStopsByFarmId(getCapturedDataRequest.FarmId);
            List<Treatment> treatments = _dbBugReporting.GetTreatmentsByFarmId(getCapturedDataRequest.FarmId);

            if (!scoutStops.Any() && !treatments.Any())
                return null;

            string farmName;

            if (!scoutStops.Any())
                farmName = treatments[0].Block.Farm.FarmName;
            else
                farmName = scoutStops[0].Block.Farm.FarmName;
            
            List<ScoutStopDto> scoutStopsDto = AutoMapper.Mapper.Map<List<ScoutStopDto>>(scoutStops);
            List<TreatmentDto> treatmentsDto = AutoMapper.Mapper.Map<List<TreatmentDto>>(treatments);

            return new GetCapturedDataResponse()
            {
                FarmName = farmName,
                ScoutStops = scoutStopsDto,
                Treatments = treatmentsDto
            };
        }
    }
}
