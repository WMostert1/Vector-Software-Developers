//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BugCentral.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ScoutBug
    {
        public int ScoutBugID { get; set; }
        public int ScoutStopID { get; set; }
        public int SpeciesID { get; set; }
        public int NumberOfBugs { get; set; }
        public byte[] FieldPicture { get; set; }
        public string Comments { get; set; }
        public Nullable<int> LastModifiedID { get; set; }
        public Nullable<System.DateTime> TMStamp { get; set; }
    
        public virtual ScoutStop ScoutStop { get; set; }
        public virtual Species Species { get; set; }
    }
}
