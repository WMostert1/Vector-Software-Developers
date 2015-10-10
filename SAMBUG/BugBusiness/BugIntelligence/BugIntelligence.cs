using BugBusiness.Interface.BugIntelligence;
using BugBusiness.Interface.BugIntelligence.DTO;
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
        public ClassifyResult classify(byte[] image)
        {
            string workingPath = Directory.GetCurrentDirectory();
            string path = workingPath+"\\ProcessedImages";
            Directory.CreateDirectory(path);
            var ms = new MemoryStream(image);
            Bitmap colourImage = new Bitmap(ms);
            Bitmap grayImage;

            Bitmap[] colourPyramid;
            Bitmap[] grayPyramid;
            var processor = new ImageProcessing();

                processor.IdentifyContours(colourImage, 100, true, out grayImage, out colourImage);
                colourPyramid = processor.getImagePyramid(colourImage, 1);
                grayPyramid = processor.getImagePyramid(grayImage, 1);
            

            int count = 0;
            foreach (var img in colourPyramid)
            {
                img.Save(count+"colour"+(count++)+".jpg");
            }

            count = 0;

            foreach (var img in grayPyramid)
            {
                img.Save(count + "gray"+(count++)+".jpg");
            }

            return new ClassifyResult { SpeciesName = "Coconut Bug", Lifestage = 1, SpeciesID = 1 };
        }
    }
}
