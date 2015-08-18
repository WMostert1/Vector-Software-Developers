using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface.Domain;

namespace DataAccess.Interface
{
    class IDbSynchronization
    {
        bool PersistBugStops(Int64 scoutStopID, Int64 userID, Int64 blockID, int numberOfTrees, float latitude, float longitude, DateTime datetime, int lastModifiedID, DateTime tmStamp);
        bool PersistScoutBugs(Int64 scoutBugID, Int64 scoutStopID, Int64 speciesID, int NumberOfBugs, byte[] image, string comments, int lastModifiedID, DateTime tmpStamp);
    }
}
