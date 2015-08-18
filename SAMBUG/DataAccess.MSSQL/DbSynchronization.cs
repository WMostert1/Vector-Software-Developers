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
    class DbSynchronization: IDbAuthentication
    {
       
        public bool PersistBugStops(Int64 scoutStopID, Int64 userID, Int64 blockID, int numberOfTrees, float latitude, float longitude, DateTime datetime, int lastModifiedID, DateTime tmStamp)
        {
            bool success = false;
            var db = new BugDBEntities();
            return success;
        }

        public bool PersistScoutBugs(Int64 scoutBugID, Int64 scoutStopID, Int64 speciesID, int NumberOfBugs, byte[] image, string comments, int lastModifiedID, DateTime tmpStamp)
        {
            bool success = false;
            var db = new BugDBEntities();
            return success;

        }
    }
}
