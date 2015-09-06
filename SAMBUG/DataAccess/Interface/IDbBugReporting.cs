using System.Collections.Generic;
using DataAccess.Models;

namespace DataAccess.Interface
{
    public interface IDbBugReporting
    {
        List<ScoutStop> GetScoutStopsByFarmId(long farmId);
        List<Treatment> GetTreatmentsByFarmId(long farmId);
        List<Species> GetSpeciesByFarmId(long farmId);
    }
}