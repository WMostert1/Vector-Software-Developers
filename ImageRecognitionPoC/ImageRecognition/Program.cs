using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace ImageRecognition
{


    /// Function header

    class Program
    {
        static void Main(string[] args)
        {
            Classifier classifier = new Classifier();

            classifier.runBWANN();

            classifier.runMatchCollectionOfImages();

            
            string[] CfileNames = Directory.GetFiles("C:\\Users\\Aeolus\\Pictures\\SAMBUG\\C\\Adult");
            string[] YfileNames = Directory.GetFiles("C:\\Users\\Aeolus\\Pictures\\SAMBUG\\Y\\Adult");
            string[] TfileNames = Directory.GetFiles("C:\\Users\\Aeolus\\Pictures\\SAMBUG\\T\\Adult");

            List<Bitmap> C = new List<Bitmap>();
            List<Bitmap> Y = new List<Bitmap>();
            List<Bitmap> T = new List<Bitmap>();

            foreach (var file in CfileNames)
                C.Add(new Bitmap(file));

            foreach (var file in YfileNames)
                Y.Add(new Bitmap(file));

            List<ImageBundle> C_Bundle = new List<ImageBundle>();
            List<ImageBundle> Y_Bundle = new List<ImageBundle>();

            Matrix<byte> ORBfeatures = classifier.getORBDescriptors(new Image<Gray, byte>(C[0]));


            foreach (var b in C)
            {
                Image<Gray, byte> temp = new Image<Gray, byte>(b);
                temp = temp.Resize(475, 550, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
   
                ImageBundle bundle = new ImageBundle(temp);
                C_Bundle.Add(bundle);
            }

            foreach (var b in Y)
            {
                Image<Gray, byte> temp = new Image<Gray, byte>(b);
                temp = temp.Resize(475, 550, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                ImageBundle bundle = new ImageBundle(temp);
                Y_Bundle.Add(bundle);
            }

          
            Emgu.CV.ML.ANN_MLP network = classifier.getImageANN(C_Bundle,Y_Bundle);
            if (network == null) return;
            List<ImageBundle> CB = new List<ImageBundle>();
            List<ImageBundle> YB = new List<ImageBundle>();
            Matrix<int> confusion = classifier.getConfusionMatrix(network, C_Bundle, Y_Bundle);
            for (int r = 0; r < confusion.Rows; r++)
            {
                for (int c = 0; c < confusion.Cols; c++)
                {
                    Console.Write(confusion.Data[r, c] + " ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}


