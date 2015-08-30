using BugBusiness.Interface.BugScouting;
using BugBusiness.Interface.BugScouting.DTO;
using BugBusiness.Interface.BugScouting.Exceptions;
using DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.BugScouting
{
    public class BugScouting: IBugScouting
    {
        private IDbSynchronization dbSynchronization { get; set; }
        BugScouting(IDbSynchronization _dbSynchronization)
        {
            dbSynchronization = _dbSynchronization;
        }

        public PersistScoutStopsResult PersistScoutStops(PersistScoutStopsRequest persistScoutStopsRequest)//Int64 scoutStopID, Int64 userID, Int64 blockID, int numberOfTrees, float latitude, float longitude, DateTime date, int lastModifiedID, DateTime tmStamp)
        {
            if(dbSynchronization.PersistScoutStops(persistScoutStopsRequest.ScoutStopID, persistScoutStopsRequest.UserID, persistScoutStopsRequest.BlockID, persistScoutStopsRequest.NumberOfTrees, persistScoutStopsRequest.Latitude, persistScoutStopsRequest.Longitude, persistScoutStopsRequest.Date, persistScoutStopsRequest.LastModifiedID, persistScoutStopsRequest.TmStamp) == false)
                throw new PersistScoutStopsException();

            return new PersistScoutStopsResult();
        }
        public PersistScoutBugsResult PersistScoutBugs(PersistScoutBugsRequest persistScoutBugsRequest)//Int64 scoutBugID, Int64 scoutStopID, Int64 speciesID, int numberOfBugs, byte[] fieldImage, string comments, int lastModifiedID, DateTime tmpStamp)
        {
            if(dbSynchronization.PersistScoutBugs(persistScoutBugsRequest.ScoutBugID,persistScoutBugsRequest.ScoutStopID,persistScoutBugsRequest.SpeciesID,persistScoutBugsRequest.NumberOfBugs,persistScoutBugsRequest.FieldImage,persistScoutBugsRequest.Comments,persistScoutBugsRequest.LastModifiedID,persistScoutBugsRequest.TmStamp) == false)
                    throw new PersistScoutBugsException();

            return new PersistScoutBugsResult();
        }
    }
}
