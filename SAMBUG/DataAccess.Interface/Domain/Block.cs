using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
