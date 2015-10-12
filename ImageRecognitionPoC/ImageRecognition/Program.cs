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
        private class Config
        {
            public int [] layers { get; set; }
            public float dw { get; set; }
            public float moment { get; set; }

            public float alpha { get; set; }
            public float beta { get; set; }

            public float accuracy { get; set; }

            public String toString()
            {
                return "Layers: " + layers[1]+"\n"+
                    "DW: "+dw+"\n"+
                    "Moment: "+moment+"\n"+
                    "A: "+alpha+"\n"+
                    "B: "+beta+"\n"+
                    "Acc: "+accuracy;
            }
        }



        static void Main(string[] args)
        {
            Classifier classifier = new Classifier();
            classifier.restructureTrainingData("C:\\Users\\Aeolus\\Pictures\\SAMBUG\\ANN\\Training");
            FileInfo [] files = new DirectoryInfo("C:\\Users\\Aeolus\\Pictures\\SAMBUG\\ANN\\Training\\Training_Data").GetFiles();
            foreach(FileInfo file in files){
                if (file.Extension.Equals(".db")) continue;
                Image<Bgr, byte> image = new Image<Bgr, byte>(file.FullName);
                while(image.Width > 1000 || image.Height > 1000)
                    image = image.Resize(0.5,Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                classifier.doGrabCut(image).Save(file.FullName);
            }
            


            //classifier.runBoW();
           // classifier.Classify(new Image<Bgr, Byte>("C:\\Users\\Aeolus\\Pictures\\SAMBUG\\coconut25.jpg"), "C:\\Users\\Aeolus\\Pictures\\SAMBUG\\ANN\\Training\\Training_Data");

            return;

            float highest = 0.0F;
            Config high = null;

            Matrix<float> testing_data;
            Matrix<float> testing_classes;
            Matrix<float> training_data;
            Matrix<float> training_classes;

            int number_of_classes = 12;
            int number_of_testing_samples = classifier.getMatrices(out training_data, out training_classes, out testing_data, out testing_classes);


            List<Config> configurations = new List<Config>();

           
                for (float dw = 0.1F; dw <= 0.1F; dw += 0.02F)
                {
                    for (float moment = 0.5F; moment <= 0.5F; moment += 0.2F)
                    {
                        for (float alpha = 0.76F; alpha <= 0.76F; alpha += 0.02F)
                        {
                            for (float beta = 0.66F; beta <= 0.66F; beta += 0.02F)
                            {
                                int input = 64*64;
                                int [] layers = {input,input/2,input/4,input/8,input/16,input/32,input/64,12};

                                float result = classifier.runBWANNTesting(layers, dw, moment, alpha, beta, training_data, training_classes, testing_data, testing_classes, number_of_testing_samples,number_of_classes);
                                var config = new Config { layers = layers, dw =dw, moment = moment,alpha = alpha, beta = beta,accuracy =result};
                                if (result > highest)
                                {
                                    highest = result;
                                    high = config;
                                    Console.WriteLine(high.toString());
                                }

                                configurations.Add(config);
                                
                            }
                        }
                    }
                }
            

            Console.WriteLine(high.toString());
            Console.ReadLine();

            /*classifier.runMatchCollectionOfImages();

            
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
             * */
        }
    }
}


