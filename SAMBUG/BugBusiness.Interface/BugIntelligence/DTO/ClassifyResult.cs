using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugIntelligence.DTO
{
    public class ClassifyResult
    {
        public string SpeciesName { get; set; }
        public int SpeciesID { get; set; }
        public int Lifestage { get; set; }
    }
}
