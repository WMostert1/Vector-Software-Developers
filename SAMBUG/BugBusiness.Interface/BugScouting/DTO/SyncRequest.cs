using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess.Models;

namespace BugBusiness.Interface.BugScouting.DTO 
{
    public class SyncRequest
    {
        public ICollection<ScoutStop> ScoutStops { get; set; }
        public ICollection<ScoutBug> ScoutBugs { get; set; }
    }
}