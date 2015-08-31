using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.Interface.Domain;
using DataAccess.MSSQL;

namespace BugWeb
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            AutoMapper.Mapper.CreateMap<DataAccess.Models.Treatment, BugBusiness.Interface.BugReporting.DTO.TreatmentDto>();
            AutoMapper.Mapper.CreateMap<DataAccess.Models.ScoutStop, BugBusiness.Interface.BugReporting.DTO.ScoutStopDto>();
            AutoMapper.Mapper.CreateMap<DataAccess.Models.ScoutBug, BugBusiness.Interface.BugReporting.DTO.ScoutBugDto>();

            //TODO: update the following with the mapping from DataAccess.Models instead of old Domain models
            AutoMapper.Mapper.CreateMap<Farm, Models.ReportingViewModel.FarmViewModel>();
            AutoMapper.Mapper.CreateMap<Block, Models.ReportingViewModel.BlockViewModel>();

        }
    }
}