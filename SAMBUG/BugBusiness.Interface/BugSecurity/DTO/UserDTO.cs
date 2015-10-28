using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugSecurity.DTO
{
    public class UserDTO
    {
        public long UserID { get; set; }
        public string Email { get; set; }
        public List<RoleDTO> Roles { get; set; }
        public List<FarmManagement.DTO.FarmForFarmManDto> Farms { get; set; }
    }
}
