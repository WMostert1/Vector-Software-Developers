using BugBusiness.ExtensionMethods;
using BugBusiness.Interface.BugIntelligence;
using BugWeb.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugWeb.Controllers
{
    public class IntelligenceController : Controller
    {
        private readonly IBugIntelligence _bugIntelligence;

        public IntelligenceController(IBugIntelligence bugIntelligence)
        {
            _bugIntelligence = bugIntelligence;
        }

        // GET: IntelligenceControlPanel
        public ActionResult Index()
        {
            var species = _bugIntelligence.getAllSpecies();

            return View(new IntelligenceViewModel { species = species });
        }

        public ActionResult deleteSpeciesTrainingData(int SpeciesID)
        {
            return new EmptyResult();
        }

        public ActionResult deleteSpeciesTrainingData(int SpeciesID, string fileName)
        {
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult addSpeciesTrainingData(HttpPostedFileBase file, int SpeciesID)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/App_Data/ANN/"+SpeciesID+"/"), fileName);
                file.SaveAs(path);
            }
            int id = SpeciesID;
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index");    
        }

        public ActionResult showSpeciesTrainingData(int SpeciesID)
        {
            var path = Server.MapPath("~/App_Data/ANN/" + SpeciesID + "/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string[] trainingFiles = Directory.GetFiles(path);
            byte [][] training_images = new byte[trainingFiles.Length][];


            for (int i = 0; i < trainingFiles.Length; i++)
            {
                var image = new Bitmap(trainingFiles[i]);
                training_images[i] = DataConversion.ToByteArray(image, ImageFormat.Bmp);

            }
            int id = SpeciesID;

            return PartialView("_TrainingData",new IntelligenceTrainingViewModel { SpeciesID = id, fileNames = trainingFiles, images = training_images });
        }
    }
}