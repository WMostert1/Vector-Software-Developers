using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.FarmManagement.DTO
{
    public class FarmForFarmManDto
    {
        public long FarmID { get; set; }
        public string FarmName { get; set; }
        public List<BlockFarmManDto> Blocks { get; set; }
    }
}
