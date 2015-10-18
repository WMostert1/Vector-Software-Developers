using System;
using DataAccess.Interface;
using DataAccess.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity.Validation;

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
                    
                    stop.LastModifiedID = "dbo";
                    foreach (var bug in stop.ScoutBugs)
                    {
                        bug.LastModifiedID = "dbo";
                        bug.TMStamp = DateTime.Now;
                        if (bug.SpeciesID == 0)
                            bug.SpeciesID = 1;
                    }

                    stop.TMStamp = DateTime.Now;

                    db.ScoutStops.Add(stop);
                }

                db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                        return false;
                    }
                }
            }
            return true;
        }

        
       
    }
}
