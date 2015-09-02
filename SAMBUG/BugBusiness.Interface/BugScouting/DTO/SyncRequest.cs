using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BugBusiness.Interface.BugScouting.DTO 
{
    public class SyncRequest
    {
        public ICollection<ScoutStopDTO> ScoutStops { get; set; }
        public ICollection<ScoutBugDTO> scoutBugs { get; set; }
    }
}