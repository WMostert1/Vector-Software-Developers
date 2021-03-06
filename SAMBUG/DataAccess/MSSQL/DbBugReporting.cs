﻿using System.Collections.Generic;
using System.Linq;
using DataAccess.Interface;
using DataAccess.Models;
using System.Data.Entity;

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

        public List<ScoutStop> GetScoutStopsByUserId(long userId)
        {
            var db = new BugDBEntities();

            List<ScoutStop> scoutStops = db.ScoutStops.Select(stop => stop).Where(stop => stop.Block.Farm.User.UserID.Equals(userId)).ToList();

            return scoutStops;

        }

        public List<Treatment> GetTreatmentsByUserId(long userId)
        {
            var db = new BugDBEntities();

            List<Treatment> treatments = db.Treatments.Select(tr => tr).Where(tr => tr.Block.Farm.User.UserID.Equals(userId)).ToList();

            return treatments;
        }

        public List<Species> GetAllSpecies()
        {
            var db = new BugDBEntities();

            List<Species> species = db.Species.Select(sp => sp).ToList();

            return species;
        }

        public List<ScoutStop> GetAllScoutStops()
        {
            var db = new BugDBEntities();

            List<ScoutStop> scoutStops = db.ScoutStops.Include(stp => stp.ScoutBugs.Select(bg => bg.Species)).Include(stp => stp.Block).ToList();

            return scoutStops;
        }

        public List<Treatment> GetAllTreatments()
        {
            var db = new BugDBEntities();

            List<Treatment> treatments = db.Treatments.Include(trtm => trtm.Block).Include(trtm => trtm.Block.Farm).ToList();

            return treatments;
        }

    }
}