using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.FarmManagement.DTO
{
    public class GetTreatmentInfoResponse
    {
        public List<FarmForTreatmentDto> Farms { get; set; }
    }
}
