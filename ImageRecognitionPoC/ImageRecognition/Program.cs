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
            try
            {
                BagOfWords bow = new BagOfWords();
                bow.runBoW();
            }catch(Exception e){
                Debug.WriteLine(e.Message);
            }
        }
    }
}


