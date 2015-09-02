using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugScouting.DTO
{
    public class ScoutBugDTO
    {
        public long ScoutBugID { get; set; }
        public long ScoutStopID { get; set; }
        public long SpeciesID { get; set; }
        public int NumberOfBugs { get; set; }
        public sbyte[] FieldPicture { get; set; }
        public string Comments { get; set; }

        public virtual ScoutStopDTO ScoutStop { get; set; }
        
    }
}
