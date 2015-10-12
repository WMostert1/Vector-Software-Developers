using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace BugWeb.Models
{
    public class IntelligenceTrainingViewModel
    {
        public string [] fileNames { get; set; }
        public byte [][] images { get; set; }
        public int SpeciesID { get; set; }
    }
}