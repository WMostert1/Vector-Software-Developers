using System.Collections.Generic;

namespace DataAccess.Interface.Domain
{
    public class User
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public List<Role> Roles { get; set; }
        public List<Farm> Farms { get; set; }

    }
}
