//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BugDataAccessLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class ScoutStop
    {
        public ScoutStop()
        {
            this.ScoutBugs = new HashSet<ScoutBug>();
        }
    
        public int ScoutStopID { get; set; }
        public int UserID { get; set; }
        public int BlockID { get; set; }
        public int TreeAmount { get; set; }
        public string Geolocation { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<int> LastModifiedID { get; set; }
        public Nullable<System.DateTime> TMStamp { get; set; }
    
        public virtual Block Block { get; set; }
        public virtual ICollection<ScoutBug> ScoutBugs { get; set; }
        public virtual User User { get; set; }
    }
}
