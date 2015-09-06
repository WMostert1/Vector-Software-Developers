using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugBusiness.Interface.BugReporting.DTO;

namespace BugBusiness.Interface.BugReporting
{
    public interface IBugReporting
    {
        GetCapturedDataResponse GetCapturedData(GetCapturedDataRequest getCapturedDataRequest);
        GetSpeciesResponse GetSpecies(GetSpeciesRequest getSpeciesRequest);
    }
}
