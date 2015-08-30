using System;
using DataAccess.Interface;
using DataAccess.Models;

namespace DataAccess.MSSQL
{
    class DbSynchronization : IDbSynchronization
    {
       //TODO: Some fields are commented out because db structure has changed
        public bool PersistScoutStops(Int64 scoutStopID, Int64 userID, Int64 blockID, int numberOfTrees, float latitude, float longitude, DateTime date, int lastModifiedID, DateTime tmStamp)
        {

            try
            {
                var db = new BugDBEntities();
                var scoutStop = new ScoutStop()
                {
                    ScoutStopID = scoutStopID,
                    //UserID = userID,
                    BlockID = blockID,
                    NumberOfTrees = numberOfTrees,
                    Latitude = latitude,
                    Longitude = longitude,
                    Date = date
                    /*LastModifiedID = lastModifiedID,
                    TMStamp = tmStamp*/
                };
                db.ScoutStops.Add(scoutStop);
                db.SaveChanges();
            }
            catch (Exception)
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

                var scoutBug = new ScoutBug()
                {
                    ScoutBugID = scoutBugID,
                    ScoutStopID = scoutStopID,
                    SpeciesID = speciesID,
                    NumberOfBugs = numberOfBugs,
                    FieldPicture = fieldImage,
                    Comments = comments
                    /*LastModifiedID = lastModifiedID,
                    TMStamp = tmpStamp*/
                };
                db.ScoutBugs.Add(scoutBug);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }
    }
}
