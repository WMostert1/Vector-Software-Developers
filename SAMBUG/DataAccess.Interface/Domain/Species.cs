using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface.Domain
{
    //TODO: Ensure that ID isn't required
    public class Species
    {
        public string SpeciesName { get; set; }
        public int Lifestage { get; set; }
        public bool IsPest { get; set; }
    }
}
