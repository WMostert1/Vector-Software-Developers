using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugReporting.DTO
{
    public class GetCapturedDataResponse
    {
        public ICollection<ScoutStopDto> ScoutStops { get; set; }
        public ICollection<TreatmentDto> Treatments { get; set; } 
    }
}
