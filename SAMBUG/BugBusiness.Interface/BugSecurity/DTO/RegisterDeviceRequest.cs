using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugSecurity.DTO
{
    public class RegisterDeviceRequest
    {
        public int UserID { get; set; }
        public string DeviceToken { get; set; } 
    }
}
