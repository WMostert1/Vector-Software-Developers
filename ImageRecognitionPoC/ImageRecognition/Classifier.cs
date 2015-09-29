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

namespace ImageRecognition
{
    class Classifier
    {


        public Matrix<float> getFeatureMatrix(Image<Gray, Byte> modelImage)
        {
           
            

            SURFDetector surfCPU = new SURFDetector(500, false);
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

        public void runANNDerivitive(int numberOfFeatures,List<ImageBundle> CB,List<ImageBundle> YB)
        {
            try
            {

                int trainSampleCount = CB.ToArray().Length  + YB.ToArray().Length;

                #region Generate the traning data and classes
                Matrix<float> trainData = new Matrix<float>(trainSampleCount, numberOfFeatures);
                Matrix<float> trainClasses = new Matrix<float>(trainSampleCount, 2);
                //1 : C , 2: Y

                //This is the output image (showing visually the result of the neural network computation)
                Image<Bgr, Byte> img = new Image<Bgr, byte>(500, 500);

                Matrix<float> sample = new Matrix<float>(1, numberOfFeatures);
                Matrix<float> prediction = new Matrix<float>(1, 1);

                //Training data1 is half of training set
                Matrix<float> trainData1 = trainData.GetRows(0, trainSampleCount / 2, 1);  //trainSampleCount >> 1 = trainSampleCount/2

                //Inplace fills Array with normally distributed random numbers
                trainData1.SetRandNormal(new MCvScalar(200), new MCvScalar(50));

                //Return the matrix corresponding to a specified row span of the input array
                Matrix<float> trainData2 = trainData.GetRows(trainSampleCount / 3, trainSampleCount * 2 / 3, 1);

                //Inplace fills Array with normally distributed random numbers
                trainData2.SetRandNormal(new MCvScalar(300), new MCvScalar(50));

                Matrix<float> trainData3 = trainData.GetRows(trainSampleCount * 2 / 3, trainSampleCount, 1);

                trainData3.SetRandNormal(new MCvScalar(400), new MCvScalar(100));


                Matrix<float> trainClasses1 = trainClasses.GetRows(0, trainSampleCount / 3, 1); //as above
                //Set the element of the Array to value
                trainClasses1.SetValue(1); //1 : The value to be set for each element of the Array

                Matrix<float> trainClasses2 = trainClasses.GetRows(trainSampleCount / 3, trainSampleCount * 2 / 3, 1);

                trainClasses2.SetValue(2);

                Matrix<float> trainClasses3 = trainClasses.GetRows(trainSampleCount * 2 / 3, trainSampleCount, 1);

                trainClasses3.SetValue(3);


                #endregion

                Matrix<int> layerSize = new Matrix<int>(new int[] { 3, 5, 1 }); //Number of hidden nodez

                MCvANN_MLP_TrainParams parameters = new MCvANN_MLP_TrainParams(); //Parameters for Artificla Neural Network - MultiLayer Perceptron
                //The termination criteria
                parameters.term_crit = new MCvTermCriteria(10, 1.0e-8); //Create the termination criteria using the constrain of maximum iteration as well as epsilon (learning rate)
                parameters.train_method = Emgu.CV.ML.MlEnum.ANN_MLP_TRAIN_METHOD.BACKPROP; //Sets the training method to backpropogation
                parameters.bp_dw_scale = 0.1; //Not sure what dw is
                parameters.bp_moment_scale = 0.1; //momentum?
                //Free parameters of the activation function, alpha and beta
                using (Emgu.CV.ML.ANN_MLP network = new ANN_MLP(layerSize, Emgu.CV.ML.MlEnum.ANN_MLP_ACTIVATION_FUNCTION.SIGMOID_SYM, 1.0, 1.0)) //use normal sigmoid
                {
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

                    for (int i = 0; i < img.Height; i++)
                    {
                        for (int j = 0; j < img.Width; j++)
                        {
                            //each pixel in the image (The one being shown as output)
                            sample.Data[0, 0] = j; //Set the sample
                            sample.Data[0, 1] = i; //Basically, having it run and do a prediction
                            sample.Data[0, 2] = i < j ? i : j;
                            //The prediction results, should have the same # of rows as the samples
                            network.Predict(sample, prediction); //Saves the prediction to (prediction)

                            // estimates the response and get the neighbors' labels
                            //In case of classification the method returns (response) the class label, in case of regression - the output function value
                            float response = prediction.Data[0, 0];

                            // highlight the pixel depending on the accuracy (or confidence)

                            //Debug.WriteLine(response);
                            if (response < 1.5)
                                img[i, j] = new Bgr(90, 0, 0);
                            else if (response < 2.5)
                                img[i, j] = new Bgr(0, 90, 0);
                            else
                                img[i, j] = new Bgr(0, 0, 90);
                            //img[i, j] = response < 1.5 ? new Bgr(90, 0, 0) : new Bgr(0, 90, 0);
                        }
                    }
                }

                // display the original training samples
                for (int i = 0; i < (trainSampleCount / 3); i++)
                {
                    //Class 2
                    PointF p1 = new PointF(trainData1[i, 0], trainData1[i, 1]);
                    img.Draw(new CircleF(p1, 2), new Bgr(255, 100, 100), -1);
                    //Class 2
                    PointF p2 = new PointF((int)trainData2[i, 0], (int)trainData2[i, 1]);
                    img.Draw(new CircleF(p2, 2), new Bgr(100, 255, 100), -1);

                    PointF p3 = new PointF((int)trainData3[i, 0], (int)trainData3[i, 1]);
                    img.Draw(new CircleF(p3, 2), new Bgr(100, 100, 255), -1);
                }
                Emgu.CV.UI.ImageViewer.Show(img);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
