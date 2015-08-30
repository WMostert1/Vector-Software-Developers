using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugScouting.DTO
{
    public class PersistScoutBugsRequest
    {
        public Int64 ScoutBugID { get; set; }
        public Int64 ScoutStopID { get; set; }
        public Int64 SpeciesID { get; set; }
        public int   NumberOfBugs { get; set; }
        public byte[] FieldImage { get; set; }
        public string Comments { get; set; }
        public int LastModifiedID { get; set; }
        public DateTime TmStamp { get; set; }
    }
}
