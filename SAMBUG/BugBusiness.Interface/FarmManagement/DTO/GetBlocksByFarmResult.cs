using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugBusiness.Interface.FarmManagement.DTO;

namespace BugBusiness.Interface.FarmManagement.DTO
{
    public class GetBlocksByFarmResult
    {
        public ICollection<BlockDTO> Blocks;
    }
}
