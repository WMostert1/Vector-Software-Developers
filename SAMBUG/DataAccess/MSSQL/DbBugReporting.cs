using System.Collections.Generic;
using System.Linq;
using DataAccess.Interface;
using DataAccess.Models;

namespace DataAccess.MSSQL
{
    public class DbBugReporting : IDbBugReporting
    {
        public Farm GetFarmById(long farmId)
        {
            var db = new BugDBEntities();

            Farm farm = db.Farms.SingleOrDefault(frm => frm.FarmID.Equals(farmId));
            
            return farm;

        }
         
    }
}