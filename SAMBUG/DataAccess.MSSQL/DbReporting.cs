using System.Collections.Generic;
using System.Linq;
using DataAccess.Interface;

namespace DataAccess.MSSQL
{
    public class DbReporting : IDbReporting
    {
        public  GetFarmById(long farmId)
        {
            var db = new BugDBEntities();

            IEnumerable<Interface.Domain.ScoutStop> entityScoutStop = db.ScoutStops.Select(stop => AutoMapper.Mapper.Map<Interface.Domain.ScoutStop>(stop)).ToList();

            if (!entityScoutStop.Any())
            {
                return null;
            }

           

            /* var entityUser = db.Users.SingleOrDefault(usr => usr.Email.Equals(username) && usr.Password.Equals(password));

            if (entityUser == default(User))
            {
                return null;
            }

            

           

            return domainUser;*/

            return new Interface.Domain.Farm();

        }
         
    }
}