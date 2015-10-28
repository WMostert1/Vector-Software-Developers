using System.Collections.Generic;

namespace BugBusiness.Interface.FarmManagement.DTO
{
    public class FarmForSprayDto
    {
        public long FarmID { get; set; }
        public string FarmName { get; set; }
        public List<BlockSprayDto> Blocks { get; set; }
    }
}