using System.Collections.Generic;
using DataAccess.Interface.Domain;

namespace BugBusiness.Interface.BugSecurity.DTO
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public List<Role> Roles{ get; set; }
    }
}
