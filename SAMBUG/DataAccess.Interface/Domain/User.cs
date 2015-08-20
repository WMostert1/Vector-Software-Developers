using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface.Domain
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public List<Role> Roles { get; set; }
        public List<Farm> Farms { get; set; }
    }
}
