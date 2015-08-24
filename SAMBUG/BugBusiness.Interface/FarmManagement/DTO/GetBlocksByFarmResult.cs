using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface.Domain;

namespace BugBusiness.Interface.FarmManagement.DTO
{
    public class GetBlocksByFarmResult
    {
        public ICollection<Block> Blocks;
    }
}
