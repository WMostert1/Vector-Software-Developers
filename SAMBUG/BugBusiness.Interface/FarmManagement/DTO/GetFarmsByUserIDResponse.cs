using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.FarmManagement.DTO
{
    public class GetFarmsByUserIDResponse
    {
        public List<FarmForFarmManDto> Farms { get; set; }
    }
}
