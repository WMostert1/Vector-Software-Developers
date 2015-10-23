using BugBusiness.Interface.BugScouting;
using BugBusiness.Interface.BugScouting.DTO;
using BugBusiness.Interface.BugScouting.Exceptions;
using DataAccess.Interface;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.BugScouting
{
    public class BugScouting: IBugScouting
    {
        private readonly IDbScouting _dbScouting;
        public BugScouting(IDbScouting dbScouting)
        {
            _dbScouting = dbScouting;
        }

       public void persistScoutingData(SyncRequest request){
           //Build up for EF
           foreach (var stop in request.ScoutStops)
           {
               if (stop.ScoutBugs == null)
                  stop.ScoutBugs = new List<ScoutBugDTO>();
            
               foreach (var bug in request.scoutBugs)
               {
                   if (bug.ScoutStopID == stop.ScoutStopID)
                   {
                       if(bug.ScoutStop != null)
                       bug.ScoutStop = stop;
                      
                       stop.ScoutBugs.Add(bug);
                   }
               }
           }

           List<ScoutStop> ScoutStops = AutoMapper.Mapper.Map<List<ScoutStop>>(request.ScoutStops);

            if(!_dbScouting.PersistScoutStops(ScoutStops))
                    throw new PersistScoutBugsException();

        }
    }
}
