using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugWeb.Models
{
    public class ReportingViewModel
    {
        public long ActiveFarmId { get; set; }
        public FarmViewModel Farm;

        public class FarmViewModel
        {
            public string FarmName;
            public ICollection<BlockViewModel> Blocks;
        }

        public class BlockViewModel
        {
            public string BlockName;
        }
    }
}