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
         
           
         /*   Stopwatch sw = Stopwatch.StartNew();
            Classifier classifier = new Classifier();
            classifier.preProcessTrainingData();
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
          * */

        
            try
            {
                Stopwatch sw1 = Stopwatch.StartNew();
                BagOfWords bow = new BagOfWords();
                bow.runBoW();
                sw1.Stop();
                Console.WriteLine(sw1.Elapsed);

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        
          Console.ReadLine();

        }
    }
}


