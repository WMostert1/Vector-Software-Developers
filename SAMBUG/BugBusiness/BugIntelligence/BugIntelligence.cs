using BugBusiness.Interface.BugIntelligence;
using BugBusiness.Interface.BugIntelligence.DTO;
using DataAccess.Interface;
using DataAccess.Models;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            try
            {
                string className = ANNClassifier.getInstance.classify(image);

                int _delim_index = className.IndexOf("_");
                
                string speciesName = className.Substring(0, _delim_index);
                int @delim_index = className.IndexOf("@");
                string lifestage = className.Substring(_delim_index + 1, @delim_index - _delim_index -1);
                string id = className.Substring(@delim_index + 1, className.Length - @delim_index -1);



                return new ClassifyResult { SpeciesName = speciesName, Lifestage = Convert.ToInt32(lifestage), SpeciesID = Convert.ToInt32(id) };
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            } 
       }
    }
}
