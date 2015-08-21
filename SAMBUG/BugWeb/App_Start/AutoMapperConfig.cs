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
            AutoMapper.Mapper.CreateMap<DataAccess.MSSQL.Farm, DataAccess.Interface.Domain.Farm>();
            AutoMapper.Mapper.CreateMap<DataAccess.MSSQL.Block, DataAccess.Interface.Domain.Block>();
            AutoMapper.Mapper.CreateMap<DataAccess.MSSQL.Treatment, DataAccess.Interface.Domain.Treatment>();
            AutoMapper.Mapper.CreateMap<DataAccess.MSSQL.ScoutStop, DataAccess.Interface.Domain.ScoutStop>();
            AutoMapper.Mapper.CreateMap<DataAccess.MSSQL.ScoutBug, DataAccess.Interface.Domain.ScoutBug>();
            AutoMapper.Mapper.CreateMap<DataAccess.MSSQL.Species, DataAccess.Interface.Domain.Species>();
        }
    }
}