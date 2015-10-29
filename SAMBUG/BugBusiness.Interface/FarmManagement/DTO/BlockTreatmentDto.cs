using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.FarmManagement.DTO
{
    public class BlockTreatmentDto
    {
        public long BlockID { get; set; }
        public string BlockName { get; set; }
        public double PestsPerTree { get; set; }
        public string LastTreatment { get; set; }
        public string NextTreatment { get; set; }
    }
}
