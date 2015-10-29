using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugAuthentication.DTO
{
    public class RecoverAccountRequest
    {
        public string EmailTo { get; set; }
        public string Link { get; set; }
        public string LinkPassword { get; set; }
        public string From { get; set; }
        public string FromPassword { get; set; }
    }
}
