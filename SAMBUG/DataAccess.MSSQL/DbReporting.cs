using System.Collections.Generic;
using System.Linq;
using DataAccess.Interface;

namespace DataAccess.MSSQL
{
    public class DbReporting : IDbReporting
    {
        public void GetFarmById(long farmId)
        {
            var db = new BugDBEntities();

            IEnumerable<ScoutStop> scoutStop = db.ScoutStops.Select(stop => stop).ToList();

            if (!scoutStop.Any())
            {
                return;
            }

           

            /* var entityUser = db.Users.SingleOrDefault(usr => usr.Email.Equals(username) && usr.Password.Equals(password));

            if (entityUser == default(User))
            {
                return null;
            }

                       

            return domainUser;*/

            return;

        }
         
    }
}