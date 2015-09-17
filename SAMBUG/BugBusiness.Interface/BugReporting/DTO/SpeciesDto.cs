using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugReporting.DTO
{
    public class SpeciesDto
    {
        public string SpeciesName { get; set; }
        public int Lifestage { get; set; }
    }
}
