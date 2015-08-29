using System.Collections.Generic;

namespace DataAccess.Interface.Domain
{
    public class Block
    {
        public long BlockID;
        public string BlockName;
        public ICollection<Treatment> Treatments { get; set; }
        public ICollection<ScoutStop> ScoutStops { get; set; }
    }
}
