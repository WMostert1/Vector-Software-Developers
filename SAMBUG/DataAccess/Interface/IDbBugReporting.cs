using System.Collections.Generic;
using DataAccess.Models;

namespace DataAccess.Interface
{
    public interface IDbBugReporting
    {
        List<Species> getAllSpecies();
        List<ScoutStop> GetScoutStopsByFarmId(long farmId);
        List<Treatment> GetTreatmentsByFarmId(long farmId);
        Species getSpeciesByID(int id);
    }
}