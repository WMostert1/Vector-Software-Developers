using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;

namespace DataAccess.MSSQL
{
    class DbSynchronization : IDbSynchronization
    {
       
        public bool PersistBugStops(Int64 scoutStopID, Int64 userID, Int64 blockID, int numberOfTrees, float latitude, float longitude, DateTime date, int lastModifiedID, DateTime tmStamp)
        {

            try
            {
                var db = new BugDBEntities();
                var ScoutStop = new ScoutStop()
                {
                    ScoutStopID = scoutStopID,
                    UserID = userID,
                    BlockID = blockID,
                    NumberOfTrees = numberOfTrees,
                    Latitude = latitude,
                    Longitude = longitude,
                    Date = date,
                    LastModifiedID = lastModifiedID,
                    TMStamp = tmStamp
                };
                db.ScoutStops.Add(ScoutStop);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool PersistScoutBugs(Int64 scoutBugID, Int64 scoutStopID, Int64 speciesID, int numberOfBugs, byte[] fieldImage, string comments, int lastModifiedID, DateTime tmpStamp)
        {
            try
            {
                var db = new BugDBEntities();

                var ScoutBug = new ScoutBug()
                {
                    ScoutBugID = scoutBugID,
                    ScoutStopID = scoutStopID,
                    SpeciesID = speciesID,
                    NumberOfBugs = numberOfBugs,
                    FieldPicture = fieldImage,
                    Comments = comments,
                    LastModifiedID = lastModifiedID,
                    TMStamp = tmpStamp
                };
                db.ScoutBugs.Add(ScoutBug);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;

        }
    }
}
