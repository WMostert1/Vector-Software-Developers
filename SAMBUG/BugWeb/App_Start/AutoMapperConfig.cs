using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.MSSQL;

namespace BugWeb
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            AutoMapper.Mapper.CreateMap<DataAccess.Models.Block, BugBusiness.Interface.FarmManagement.DTO.BlockDTO>();
            AutoMapper.Mapper.CreateMap<DataAccess.Models.Treatment, BugBusiness.Interface.BugReporting.DTO.TreatmentDto>();
            AutoMapper.Mapper.CreateMap<DataAccess.Models.ScoutStop, BugBusiness.Interface.BugReporting.DTO.ScoutStopDto>();
            AutoMapper.Mapper.CreateMap<DataAccess.Models.ScoutBug, BugBusiness.Interface.BugReporting.DTO.ScoutBugDto>();
            AutoMapper.Mapper.CreateMap<DataAccess.Models.Species, BugBusiness.Interface.BugReporting.DTO.SpeciesDto>();
            AutoMapper.Mapper.CreateMap<DataAccess.Models.User, BugBusiness.Interface.BugSecurity.DTO.UserDTO>();
            AutoMapper.Mapper.CreateMap<DataAccess.Models.Farm, BugBusiness.Interface.FarmManagement.DTO.FarmDTO>();
            AutoMapper.Mapper.CreateMap<DataAccess.Models.Role, BugBusiness.Interface.BugSecurity.DTO.RoleDTO>();

            AutoMapper.Mapper.CreateMap<BugBusiness.Interface.BugScouting.DTO.ScoutStopDTO,DataAccess.Models.ScoutStop>();
            AutoMapper.Mapper.CreateMap<sbyte, byte>().ConvertUsing(src => (byte)src);
            AutoMapper.Mapper.CreateMap<BugBusiness.Interface.BugScouting.DTO.ScoutBugDTO, DataAccess.Models.ScoutBug>()
               .ForMember(dest => dest.FieldPicture,
                   opts => opts. MapFrom(src => AutoMapper.Mapper.Map<byte[]>(src.FieldPicture)));             
        }
    }
}