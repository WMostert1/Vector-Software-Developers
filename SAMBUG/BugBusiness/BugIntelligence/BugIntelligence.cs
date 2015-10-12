using BugBusiness.Interface.BugIntelligence;
using BugBusiness.Interface.BugIntelligence.DTO;
using DataAccess.Interface;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.BugIntelligence
{
    public class BugIntelligence : IBugIntelligence
    {
        private readonly IDbBugReporting _dbBugReporting;

        public BugIntelligence(IDbBugReporting dbBugReporting)
        {
           
            _dbBugReporting = dbBugReporting;
        }

        public List<Species> getAllSpecies()
        {
            return _dbBugReporting.getAllSpecies();
        }

        public ClassifyResult classify(byte[] image)
        {
            if (ANNClassifier.getInstance.isCurrentlyTraining())
                throw new Exception("The nerual network is busy training. Please try again.");

            int speciesID = ANNClassifier.getInstance.classify(image);

            var species = _dbBugReporting.getSpeciesByID(speciesID);

            return new ClassifyResult { SpeciesName = species.SpeciesName, Lifestage = (int)species.SpeciesID, SpeciesID = (int)species.SpeciesID };
        }
    }
}
