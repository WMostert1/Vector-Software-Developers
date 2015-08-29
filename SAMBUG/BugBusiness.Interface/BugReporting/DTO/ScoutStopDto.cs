using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugReporting.DTO
{
    public class ScoutStopDto
    {
        public string BlockBlockName { get; set; }
        public int NumberOfTrees { get; set; }
        public DateTime Date { get; set; }
        public ICollection<ScoutBugDto> ScoutBugs { get; set; }

    }
}
