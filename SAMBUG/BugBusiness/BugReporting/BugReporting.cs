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
            List<ScoutStop> scoutStops = _dbBugReporting.GetScoutStopsByUserId(getCapturedDataRequest.UserId);
            List<Treatment> treatments = _dbBugReporting.GetTreatmentsByUserId(getCapturedDataRequest.UserId);

            if (!scoutStops.Any() && !treatments.Any())
                return null;
            
            List<ScoutStopDto> scoutStopsDto = AutoMapper.Mapper.Map<List<ScoutStopDto>>(scoutStops);
            List<TreatmentDto> treatmentsDto = AutoMapper.Mapper.Map<List<TreatmentDto>>(treatments);

            return new GetCapturedDataResponse()
            {
                ScoutStops = scoutStopsDto,
                Treatments = treatmentsDto
            };
        }

        public GetCapturedDataResponse GetAllCapturedData()
        {
            List<ScoutStop> scoutStops = _dbBugReporting.GetAllScoutStops();
            List<Treatment> treatments = _dbBugReporting.GetAllTreatments();

            if (!scoutStops.Any() && !treatments.Any())
                return null;

            List<ScoutStopDto> scoutStopsDto = AutoMapper.Mapper.Map<List<ScoutStopDto>>(scoutStops);
            List<TreatmentDto> treatmentsDto = AutoMapper.Mapper.Map<List<TreatmentDto>>(treatments);

            return new GetCapturedDataResponse()
            {
                ScoutStops = scoutStopsDto,
                Treatments = treatmentsDto
            };
        }

        public GetSpeciesResponse GetSpecies()
        {
            List<Species> species = _dbBugReporting.GetAllSpecies();
            
            if (!species.Any() )
                return null;

            List<SpeciesDto> speciesDto = AutoMapper.Mapper.Map<List<SpeciesDto>>(species);
            
            return new GetSpeciesResponse()
            {
                Species = speciesDto
            };
        }
    }
}
