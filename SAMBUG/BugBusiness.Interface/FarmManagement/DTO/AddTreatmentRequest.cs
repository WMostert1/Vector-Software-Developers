using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.FarmManagement.DTO
{
    public class AddTreatmentRequest
    {
        public long BlockID { get; set; }
        public DateTime TreatmentDate { get; set; }
        public string TreatmentComments { get; set; }
    }
}
