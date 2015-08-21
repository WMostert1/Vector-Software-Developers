using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface.Domain
{
    public class ScoutBug
    {
        public int NumberOfBugs { get; set; }
        public string Comments { get; set; }

        public Species Species { get; set; }
    }
}
