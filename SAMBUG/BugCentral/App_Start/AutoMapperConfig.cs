using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugCentral
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            AutoMapper.Mapper.CreateMap<DataAccess.Models.Treatment, BugBusiness.Interface.BugReporting.DTO.TreatmentDto>();
            AutoMapper.Mapper.CreateMap<DataAccess.Models.ScoutStop, BugBusiness.Interface.BugReporting.DTO.ScoutStopDto>();
            AutoMapper.Mapper.CreateMap<DataAccess.Models.ScoutBug, BugBusiness.Interface.BugReporting.DTO.ScoutBugDto>();
        }
    }
}