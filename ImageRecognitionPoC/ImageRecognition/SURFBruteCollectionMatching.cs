using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.ML;
using Emgu.CV.ML.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.GPU;
using System.Diagnostics;
using Emgu.CV.Util;
using Emgu.CV.Flann;
using System.IO;
using System;


namespace ImageRecognition
{
    class SURFBruteCollectionMatching : Classifier
    {
        public void runMatchCollectionOfImages()
        {
            Console.WriteLine("Running SURF feature detection with colelction of images...");
            string[] dbImages = Directory.GetFiles("C:\\Users\\Aeolus\\Pictures\\SAMBUG\\BF");
            string queryImage = "C:\\Users\\Aeolus\\Pictures\\SAMBUG\\Samples\\C1.JPG";

            IList<IndecesMapping> imap;

            // compute descriptors for each image
            var dbDescsList = ComputeMultipleDescriptors(dbImages, out imap);

            // concatenate all DB images descriptors into single Matrix
            Matrix<float> dbDescs = ConcatDescriptors(dbDescsList);

            // compute descriptors for the query image
            Matrix<float> queryDescriptors = ComputeSingleDescriptors(queryImage);



            FindMatches(dbDescs, queryDescriptors, ref imap);
            int Y = 0;
            int C = 0;

            int highest = 0;
            IndecesMapping high = null;
            foreach (var img in imap)
            {
                if (img.fileName.Contains("coconut"))
                {
                    C += img.Similarity;
                }
                else
                {
                    Y += img.Similarity;
                }

                if (img.Similarity > highest)
                {
                    highest = img.Similarity;
                    high = img;
                }
            }

            Console.WriteLine("C: " + C);
            Console.WriteLine("Y: " + Y);
            Console.WriteLine("Image used as sample:");
            Emgu.CV.UI.ImageViewer.Show(new Image<Bgr, byte>(queryImage));

            Console.WriteLine("Query image matches this:");
            Emgu.CV.UI.ImageViewer.Show(new Image<Bgr, byte>(high.fileName));


        }
        private void FindMatches(Matrix<float> dbDescriptors, Matrix<float> queryDescriptors, ref IList<IndecesMapping> imap)
        {
            var indices = new Matrix<int>(queryDescriptors.Rows, 2); // matrix that will contain indices of the 2-nearest neighbors found
            var dists = new Matrix<float>(queryDescriptors.Rows, 2); // matrix that will contain distances to the 2-nearest neighbors found

            // create FLANN index with 4 kd-trees and perform KNN search over it look for 2 nearest neighbours
            var flannIndex = new Index(dbDescriptors, 5);
            flannIndex.KnnSearch(queryDescriptors, indices, dists, 2, 50);

            for (int i = 0; i < indices.Rows; i++)
            {
                // filter out all inadequate pairs based on distance between pairs
                if (dists.Data[i, 0] < (0.7 * dists.Data[i, 1]))
                {
                    // find image from the db to which current descriptor range belongs and increment similarity value.
                    // in the actual implementation this should be done differently as it's not very efficient for large image collections.
                    foreach (var img in imap)
                    {
                        if (img.IndexStart <= i && img.IndexEnd >= i)
                        {
                            img.Similarity++;
                            break;
                        }
                    }
                }
            }
        }

    }
}
