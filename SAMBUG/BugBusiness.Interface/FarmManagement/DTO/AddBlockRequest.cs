using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.FarmManagement.DTO
{
    public class AddBlockRequest
    {
        public int FarmID { get; set; }
        public string BlockName { get; set; }
    }
}
