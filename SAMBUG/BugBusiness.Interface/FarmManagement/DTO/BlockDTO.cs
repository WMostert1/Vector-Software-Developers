using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugBusiness.Interface.BugReporting.DTO;

namespace BugBusiness.Interface.FarmManagement.DTO
{
    public class BlockDTO
    {
        public long BlockID { get; set; }
        public string BlockName { get; set; }
        public ICollection<TreatmentDto> Treatments { get; set; }
        public ICollection<ScoutStopDto> ScoutStops { get; set; }
    }
}
