using System;
using DataAccess.Interface;
using DataAccess.Models;
using System.Collections.Generic;
using System.Diagnostics;

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
                    //=====================================
                    stop.Date = DateTime.Now;               //  TODO: Get the data representation from Android > Web right
                    byte[] test = {1,2};                    //  This needs to be removed. 
                    foreach(var b in stop.ScoutBugs)        //  Mock's out the wrong formatted data
                    b.FieldPicture = test;
                    //=====================================

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
