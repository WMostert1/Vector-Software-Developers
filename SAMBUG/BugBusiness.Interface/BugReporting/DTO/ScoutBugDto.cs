using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugReporting.DTO
{
    public class ScoutBugDto
    {
        public int NumberOfBugs { get; set; }
        public string Comments { get; set; }
        public string SpeciesSpeciesName { get; set; }
        public int SpeciesLifestage { get; set; }
        public bool SpeciesIsPest { get; set; }
    }
}
