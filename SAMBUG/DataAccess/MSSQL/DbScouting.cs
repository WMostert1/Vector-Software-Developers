using System;
using DataAccess.Interface;
using DataAccess.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DataAccess.MSSQL
{
    public class DbScouting : IDbScouting
    {
       //TODO: Some fields are commented out because db structure has changed
        public bool PersistScoutStops(ICollection<ScoutStop> stops){

            try
            {
                
                var db = new BugDBEntities();

                foreach (var stop in stops)
                {
                    //Fixed Date and sByte to byte conversion.
                    db.ScoutStops.Add(stop);
                }

                db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        
       
    }
}
