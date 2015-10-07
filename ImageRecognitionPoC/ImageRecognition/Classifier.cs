using System;
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

namespace ImageRecognition
{
    class Classifier
    {


        public void runMatchCollectionOfImages()
        {
            
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

            Debug.WriteLine("C: " + C);
            Debug.WriteLine("Y: " + Y);
            Emgu.CV.UI.ImageViewer.Show(new Image<Bgr,byte>(high.fileName));

            
        }

        private Matrix<float> ConcatDescriptors(IList<Matrix<float>> descriptors)
        {
            int cols = descriptors[0].Cols;
            int rows = descriptors.Sum(a => a.Rows);

            float[,] concatedDescs = new float[rows, cols];

            int offset = 0;

            foreach (var descriptor in descriptors)
            {
                // append new descriptors
                Buffer.BlockCopy(descriptor.ManagedArray, 0, concatedDescs, offset, sizeof(float) * descriptor.ManagedArray.Length);
                offset += sizeof(float) * descriptor.ManagedArray.Length;
            }

            return new Matrix<float>(concatedDescs);
        }

        private void FindMatches(Matrix<float> dbDescriptors, Matrix<float> queryDescriptors, ref IList<IndecesMapping> imap)
        {
            var indices = new Matrix<int>(queryDescriptors.Rows, 2); // matrix that will contain indices of the 2-nearest neighbors found
            var dists = new Matrix<float>(queryDescriptors.Rows, 2); // matrix that will contain distances to the 2-nearest neighbors found

            // create FLANN index with 4 kd-trees and perform KNN search over it look for 2 nearest neighbours
            var flannIndex = new Index(dbDescriptors, 4);
            flannIndex.KnnSearch(queryDescriptors, indices, dists, 2, 24);

            for (int i = 0; i < indices.Rows; i++)
            {
                // filter out all inadequate pairs based on distance between pairs
                if (dists.Data[i, 0] < (0.5 * dists.Data[i, 1]))
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

        private IList<Matrix<float>> ComputeMultipleDescriptors(string[] fileNames, out IList<IndecesMapping> imap)
        {
            imap = new List<IndecesMapping>();

            IList<Matrix<float>> descs = new List<Matrix<float>>();

            int r = 0;

            for (int i = 0; i < fileNames.Length; i++)
            {
                var desc = ComputeSingleDescriptors(fileNames[i]);
                descs.Add(desc);

                imap.Add(new IndecesMapping()
                {
                    fileName = fileNames[i],
                    IndexStart = r,
                    IndexEnd = r + desc.Rows - 1
                });

                r += desc.Rows;
            }

            return descs;
        }

        private Matrix<float> ComputeSingleDescriptors(string fileName)
        {
            Matrix<float> descs;

            using (Image<Gray, Byte> img = new Image<Gray, byte>(fileName))
            {
               
                descs = getSURFFeatureDescriptorMatrix(img,300,true);
            }

            return descs;
        } 

        public Matrix<byte> getORBDescriptors(Image<Gray, byte> image)
        {
            const int NumberOfFeatures = 256; 
            ORBDetector orbCPU = new ORBDetector(NumberOfFeatures);
            VectorOfKeyPoint modelKeyPoints;

            modelKeyPoints = orbCPU.DetectKeyPointsRaw(image, null);
            //var features = orbCPU.DetectFeatures(image, null);
            return orbCPU.ComputeDescriptorsRaw(image, null, modelKeyPoints);
        }


        public Matrix<float> getSURFFeatureDescriptorMatrix(Image<Gray, Byte> modelImage,int hessian, bool extended)
        {
            SURFDetector surfCPU = new SURFDetector(hessian, extended);
            VectorOfKeyPoint modelKeyPoints;

            if (GpuInvoke.HasCuda)
            {
                GpuSURFDetector surfGPU = new GpuSURFDetector(surfCPU.SURFParams, 0.01f);
                using (GpuImage<Gray, Byte> gpuModelImage = new GpuImage<Gray, byte>(modelImage))
                //extract features from the object image
                using (GpuMat<float> gpuModelKeyPoints = surfGPU.DetectKeyPointsRaw(gpuModelImage, null))
                using (GpuMat<float> gpuModelDescriptors = surfGPU.ComputeDescriptorsRaw(gpuModelImage, null, gpuModelKeyPoints))
                using (GpuBruteForceMatcher<float> matcher = new GpuBruteForceMatcher<float>(DistanceType.L2))
                {
                    modelKeyPoints = new VectorOfKeyPoint();
                    surfGPU.DownloadKeypoints(gpuModelKeyPoints, modelKeyPoints);
                }
            }
            else
            {
                //extract features from the object image
                modelKeyPoints = surfCPU.DetectKeyPointsRaw(modelImage, null);
            }
            Matrix<float> modelDescriptors = surfCPU.ComputeDescriptorsRaw(modelImage, null, modelKeyPoints);
            return modelDescriptors;
        }

        // ---------------------------------------------------------------------------------------------------------------------
                                //                    ANN Attempt
        //----------------------------------------------------------------------------------------------------------------------

        private float predict(Emgu.CV.ML.ANN_MLP network, ImageBundle sampleImg, int resultClass)
        {
            Matrix<float> sample = sampleImg.features;
            Matrix<float> prediction = new Matrix<float>(sampleImg.features.Rows, 1);

            network.Predict(sample, prediction);
            float average = (float)0.0;
            for (int i = 0; i < sampleImg.features.Rows; i++)
            {
                average += prediction.Data[i, 0];
            }
            average /= sampleImg.features.Rows;
            return average ;
        }

        public Matrix<int> getConfusionMatrix(Emgu.CV.ML.ANN_MLP network, List<ImageBundle> CB, List<ImageBundle> YB)
        {
            Matrix<int> confusionMat = new Matrix<int>(2, 2);
            confusionMat.SetValue(0);
            foreach(ImageBundle bundle in CB)
            {
                float result = predict(network, bundle, 1);
                int classification = result < 1.5 ? 0 : 1;
                confusionMat[0, classification] = confusionMat[0, classification] + 1;
            }

            foreach (ImageBundle bundle in YB)
            {
                float result = predict(network, bundle, 2);
                int classification = result < 1.5 ? 0 : 1;
                confusionMat[1, classification] = confusionMat[0, classification] + 1;
            }

            return confusionMat;
        }

        public Emgu.CV.ML.ANN_MLP getImageANN(List<ImageBundle> CB, List<ImageBundle> YB)
        {
            const int COCONUT_BUG = 1;
            const int YELLOW_EDGED_BUG = 2;
            if (CB.Count == 0 || YB.Count == 0) throw new Exception("Taining data can not be empty");
            try
            {
                int CBsize = 0;
                foreach (var b in CB)
                    CBsize += b.features.Rows;

                int YBsize = 0;
                foreach (var b in YB)
                    YBsize += b.features.Rows;

                //int numberOfClasses = 2;
                int inputLayerSize = CB[0].features.Cols;

                int trainSampleCount = CBsize + YBsize;
                Matrix<float> trainData = new Matrix<float>(trainSampleCount, inputLayerSize);
                Matrix<float> trainClasses = new Matrix<float>(trainSampleCount, 1);

                #region Generate the training data and classes

                Matrix<float> trainDataCB = trainData.GetRows(0, CBsize, 1);
                foreach (var bundle in CB)
                {
                    for (int col = 0; col < inputLayerSize; col++)
                    {
                        int currRow = 0;
                        for (int row = 0; row < bundle.features.Rows; row++)
                        {
                            trainDataCB.Data[currRow++, col] = bundle.features.Data[row, col];
                        }
                    }
                }

                Matrix<float> trainDataYB = trainData.GetRows(CBsize, trainSampleCount, 1);
                foreach (var bundle in YB)
                {
                    for (int col = 0; col < inputLayerSize; col++)
                    {
                        int currRow = 0;
                        for (int row = 0; row < bundle.features.Rows; row++)
                        {
                            trainDataYB.Data[currRow++, col] = bundle.features.Data[row, col];
                        }
                    }
                }

                Matrix<float> trainClassesCB = new Matrix<float>(CBsize, 1);
                trainClassesCB.SetValue(COCONUT_BUG);
                Matrix<float> trainClassesYB = new Matrix<float>(YBsize, 1);
                trainClassesYB.SetValue(YELLOW_EDGED_BUG);

                #endregion

                Matrix<int> layerSize = new Matrix<int>(new int[] { inputLayerSize, 5, 1 }); //Number of hidden nodez

                MCvANN_MLP_TrainParams parameters = new MCvANN_MLP_TrainParams(); //Parameters for Artificla Neural Network - MultiLayer Perceptron
                //The termination criteria
                parameters.term_crit = new MCvTermCriteria(10, 1.0e-8); //Create the termination criteria using the constrain of maximum iteration as well as epsilon (learning rate)
                parameters.train_method = Emgu.CV.ML.MlEnum.ANN_MLP_TRAIN_METHOD.BACKPROP; //Sets the training method to backpropogation
                parameters.bp_dw_scale = 0.1; //Not sure what dw is
                parameters.bp_moment_scale = 0.1; //momentum?
                //Free parameters of the activation function, alpha and beta
                Emgu.CV.ML.ANN_MLP network = new ANN_MLP(layerSize, Emgu.CV.ML.MlEnum.ANN_MLP_ACTIVATION_FUNCTION.SIGMOID_SYM, 1.0, 1.0); //use normal sigmoid

                //trainData : The training data. A 32-bit floating-point, single-channel matrix, one vector per row
                //responses (trainClasses) : A floating-point matrix of the corresponding output vectors, one vector per row.
                //sampleWeights(null): It is not null only for RPROP. The optional floating-point vector of weights for each sample. Some samples
                //may be more important than others for training, e.g. user may want to gain the weight of certain classes to find 
                //the right balance between hit-rate and false-alarm rate etc
                //paramaters: The parameters for ANN_MLP
                //flag: The flags for the neural network training function,DEFAULT	0	
                //UPDATE_WEIGHTS	1	
                //NO_INPUT_SCALE	2	
                //NO_OUTPUT_SCALE	4

                network.Train(trainData, trainClasses, null, parameters, Emgu.CV.ML.MlEnum.ANN_MLP_TRAINING_FLAG.DEFAULT);
                return network;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
    }
}
