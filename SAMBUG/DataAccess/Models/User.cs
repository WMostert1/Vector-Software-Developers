//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Farms = new HashSet<Farm>();
            this.ScoutStops = new HashSet<ScoutStop>();
            this.Roles = new HashSet<Role>();
        }
    
        public long UserID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string LastModifiedID { get; set; }
        public System.DateTime TMStamp { get; set; }
    
        public virtual ICollection<Farm> Farms { get; set; }
        public virtual ICollection<ScoutStop> ScoutStops { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
