using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugWeb.Models
{
    public class BlockViewModel
    {
        public long FarmID { get; set; }
        public long BlockID { get; set; }
        public string BlockName { get; set; }
    }
}