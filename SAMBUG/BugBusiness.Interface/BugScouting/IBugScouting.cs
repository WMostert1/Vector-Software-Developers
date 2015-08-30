using BugBusiness.Interface.BugScouting.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugScouting
{
    public interface IBugScouting
    {
        PersistScoutStopsResult PersistScoutStops(PersistScoutStopsRequest persistScoutStopsRequest);//Int64 scoutStopID, Int64 userID, Int64 blockID, int numberOfTrees, float latitude, float longitude, DateTime date, int lastModifiedID, DateTime tmStamp);
        PersistScoutBugsResult PersistScoutBugs(PersistScoutBugsRequest persistScoutBugsRequest);//Int64 scoutBugID, Int64 scoutStopID, Int64 speciesID, int numberOfBugs, byte[] fieldImage, string comments, int lastModifiedID, DateTime tmpStamp);
    }
}
