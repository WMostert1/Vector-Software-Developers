using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface.Domain
{
    public class User
    {

        public long UserId { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
