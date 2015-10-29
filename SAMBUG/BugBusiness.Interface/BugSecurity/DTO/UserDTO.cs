using System.Collections.Generic;
using BugBusiness.Interface.FarmManagement.DTO;

namespace BugBusiness.Interface.BugSecurity.DTO
{
    public class UserDTO
    {
        public long UserID { get; set; }
        public string Email { get; set; }
        public List<RoleDTO> Roles { get; set; }
        public List<FarmForFarmManDto> Farms { get; set; }
    }
}
