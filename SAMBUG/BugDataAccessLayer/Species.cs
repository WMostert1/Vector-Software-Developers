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
    
    public partial class Species
    {
        public Species()
        {
            this.ScoutBugs = new HashSet<ScoutBug>();
        }
    
        public int SpeciesID { get; set; }
        public string SpeciesName { get; set; }
        public bool IsPest { get; set; }
        public Nullable<int> LastModifiedID { get; set; }
        public Nullable<System.DateTime> TMStamp { get; set; }
    
        public virtual ICollection<ScoutBug> ScoutBugs { get; set; }
    }
}
