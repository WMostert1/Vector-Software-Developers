using System.Collections.Generic;

namespace BugBusiness.Interface.FarmManagement.DTO
{
    public class FarmForTreatmentDto
    {
        public long FarmID { get; set; }
        public string FarmName { get; set; }
        public List<BlockTreatmentDto> Blocks { get; set; }
    }
}