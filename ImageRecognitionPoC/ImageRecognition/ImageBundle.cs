using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageRecognition
{
    class ImageBundle
    {
        public Image<Gray, byte> image;
        public Matrix<float> features;
        
        public ImageBundle(Image<Gray,byte> img){
            Classifier classifier = new Classifier();
            image = img;
            features = classifier.getSURFFeatureDescriptorMatrix(img,300,true);
        }
    }
}
