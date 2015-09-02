using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugScouting.DTO
{
    public class ScoutStopDTO
    {
        public long ScoutStopID { get; set; }
        public long BlockID { get; set; }
        public int NumberOfTrees { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual ICollection<ScoutBugDTO> ScoutBugs { get; set; }
    }
}
