using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugAuthentication.DTO
{
    public class ChangePasswordRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
