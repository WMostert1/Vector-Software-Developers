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

            string className = ANNClassifier.getInstance.classify(image);

            
            int delim_index = className.IndexOf("--");
            if (delim_index == -1) return new ClassifyResult { SpeciesName = "Coconut Bug", Lifestage = 1,SpeciesID = 1 };
            string speciesName = className.Substring(0, delim_index);
            string lifestage = className.Substring(delim_index + 2, className.IndexOf(".") - delim_index);

           // var species = _dbBugReporting.getSpeciesByID(speciesID);

            return new ClassifyResult { SpeciesName = speciesName, Lifestage = Convert.ToInt32(lifestage), SpeciesID = 1 };
        }
    }
}
