using System.Collections.Generic;
using System.Linq;
using DataAccess.Interface;
using DataAccess.Models;

namespace DataAccess.MSSQL
{
    public class DbBugReporting : IDbBugReporting
    {
        public Species getSpeciesByID(int id)
        {
            var db = new BugDBEntities();

            return db.Species.SingleOrDefault(s => s.SpeciesID == id);
        }

        public List<Species> getAllSpecies()
        {
            var db = new BugDBEntities();
            return db.Species.ToList();
        }

        public List<ScoutStop> GetScoutStopsByFarmId(long farmId)
        {
            var db = new BugDBEntities();

            List<ScoutStop> scoutStops = db.ScoutStops.Select(stop => stop).Where(stop => stop.Block.Farm.FarmID.Equals(farmId)).ToList();

            return scoutStops;

        }

        public List<Treatment> GetTreatmentsByFarmId(long farmId)
        {
            var db = new BugDBEntities();

            List<Treatment> treatments = db.Treatments.Select(tr => tr).Where(tr => tr.Block.Farm.FarmID.Equals(farmId)).ToList();

            return treatments;
        }
    }
}