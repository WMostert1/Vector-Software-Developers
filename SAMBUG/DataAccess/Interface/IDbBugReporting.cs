using System.Collections.Generic;
using DataAccess.Models;

namespace DataAccess.Interface
{
    public interface IDbBugReporting
    {
        List<ScoutStop> GetScoutStopsByUserId(long userId);
        List<Treatment> GetTreatmentsByUserId(long userId);
        List<ScoutStop> GetAllScoutStops();
        List<Treatment> GetAllTreatments();
        List<Species> GetAllSpecies();
    }
}