using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface.Domain
{
    public class Role
    {
        public long RoleId { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
    }
}
