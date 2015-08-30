using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugScouting.DTO
{
    public class PersistScoutStopsRequest
    {
        public Int64 ScoutStopID { get; set; }
        public Int64 UserID { get; set; }
        public Int64 BlockID { get; set; }
        public int NumberOfTrees { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public DateTime Date { get; set; }
        public int LastModifiedID { get; set; }
        public DateTime TmStamp { get; set; }

    }
}
