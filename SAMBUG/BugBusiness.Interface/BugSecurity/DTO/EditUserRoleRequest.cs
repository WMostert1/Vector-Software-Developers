using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugSecurity.DTO
{
    public class EditUserRoleRequest
    {
        public int UserId;
        public bool IsAdministrator;
    }
}
