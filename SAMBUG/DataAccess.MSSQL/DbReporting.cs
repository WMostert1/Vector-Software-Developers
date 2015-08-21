using System.Collections.Generic;
using System.Linq;
using DataAccess.Interface;

namespace DataAccess.MSSQL
{
    public class DbReporting : IDbReporting
    {
        public Interface.Domain.Farm GetFarmById(long farmId)
        {
            var db = new BugDBEntities();

            IEnumerable<Interface.Domain.ScoutStop> entityScoutStop = db.ScoutStops.Select(stop => AutoMapper.Mapper.Map<Interface.Domain.ScoutStop>(stop)).ToList();

            if (!entityScoutStop.Any())
            {
                return null;
            }

            //map EF Farm to Domain Farm
           /* var farm = new Interface.Domain.Farm()
            {
                FarmID = entityFarm.FarmID,
                FarmName = entityFarm.FarmName,
                Blocks = entityFarm.Blocks.Select(block =>
                    new Interface.Domain.Block()
                    {
                        BlockID = block.BlockID,
                        BlockName = block.BlockName,
                        Block
                        
                    }).ToList()
            };*/

            /* var entityUser = db.Users.SingleOrDefault(usr => usr.Email.Equals(username) && usr.Password.Equals(password));

            if (entityUser == default(User))
            {
                return null;
            }

            //map EF Farm to Domain Farm
            var farms = entityUser.Farms.Select(farm =>
                new Interface.Domain.Farm()
                {
                    FarmID = farm.FarmID,
                    FarmName = farm.FarmName,
                    Blocks = farm.Blocks.Select(block =>
                        new Interface.Domain.Block()
                        {
                            BlockID = block.BlockID,
                            BlockName = block.BlockName
                        }).ToList()
                }).ToList();

            //map EF Role to Domain Role
            var roles = entityUser.Roles.Select(role =>
            new Interface.Domain.Role()
            {
                Type = role.RoleType,
                Description = role.RoleDescription
            }).ToList();

            //map EF user to Domain User
            var domainUser = new Interface.Domain.User()
            {
                Id = entityUser.UserID,
                Roles = roles,
                Farms = farms
            };

            return domainUser;*/

            return new Interface.Domain.Farm();

        }
         
    }
}