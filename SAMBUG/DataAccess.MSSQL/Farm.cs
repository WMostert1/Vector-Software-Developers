//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess.MSSQL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Farm
    {
        public Farm()
        {
            this.Blocks = new HashSet<Block>();
        }
    
        public long FarmID { get; set; }
        public long UserID { get; set; }
        public string FarmName { get; set; }
        public Nullable<int> LastModifiedID { get; set; }
        public Nullable<System.DateTime> TMStamp { get; set; }
    
        public virtual ICollection<Block> Blocks { get; set; }
        public virtual User User { get; set; }
    }
}