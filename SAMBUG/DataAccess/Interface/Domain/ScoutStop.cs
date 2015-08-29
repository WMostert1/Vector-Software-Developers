using System;
using System.Collections.Generic;

namespace DataAccess.Interface.Domain
{
    public class ScoutStop
    {
        public DateTime Date { get; set; }
        public int NumberOfTrees { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public ICollection<ScoutBug> ScoutBugs { get; set; }
    }
}
