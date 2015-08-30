using System;

namespace DataAccess.Interface
{
    public interface IDbSynchronization
    {
        bool PersistScoutStops(Int64 scoutStopID, Int64 userID, Int64 blockID, int numberOfTrees, float latitude, float longitude, DateTime date, int lastModifiedID, DateTime tmStamp);
        bool PersistScoutBugs(Int64 scoutBugID, Int64 scoutStopID, Int64 speciesID, int numberOfBugs, byte[] fieldImage, string comments, int lastModifiedID, DateTime tmpStamp);
    }
}
