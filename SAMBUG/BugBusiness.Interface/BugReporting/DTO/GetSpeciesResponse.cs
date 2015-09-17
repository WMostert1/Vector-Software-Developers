using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugReporting.DTO
{
    public class GetSpeciesResponse
    {
        public ICollection<SpeciesDto> Species { get; set; }
    }
}
