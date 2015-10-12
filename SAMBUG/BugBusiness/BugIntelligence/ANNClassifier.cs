using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.ML;
using Emgu.CV.ML.Structure;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BugBusiness.BugIntelligence
{
    public sealed class ANNClassifier
    {
        private static volatile ANNClassifier instance;
        private static object syncRoot = new Object();
        private const int DIMENSIONS = 64;
        private bool isTraining;
        private int number_of_classes;
        private Emgu.CV.ML.ANN_MLP network;

        public int classify(byte [] picture)
        {
            Bitmap bmp;
            using (var ms = new MemoryStream(picture))
            {
                bmp = new Bitmap(ms);
            }
            Image<Gray, byte> classification_image = new Image<Gray, byte>(bmp).Resize(DIMENSIONS, DIMENSIONS, INTER.CV_INTER_CUBIC);

            Emgu.CV.UI.ImageViewer.Show(classification_image);

            Matrix<float> classification_result = new Matrix<float>(1, number_of_classes);
            Matrix<float> flattened = new Matrix<float>(1, DIMENSIONS * DIMENSIONS);

            int c = 0;
            for (int i = 0; i < DIMENSIONS; i++)
                for (int j = 0; j < DIMENSIONS; j++)
                    flattened.Data[0, c++] = classification_image.Data[i, j, 0];

            Matrix<float> test_sample = new Matrix<float>(1, DIMENSIONS * DIMENSIONS);
            network.Predict(test_sample, classification_result);

            float highest = classification_result[0, 0];
            int index = 0;
            for (int i = 1; i < number_of_classes; i++)
            {
                if (classification_result[0, i] > highest)
                {
                    highest = classification_result[0, i];
                    index = i;
                }
            }
            //Should correspond with species ID's
            return index;

        }

        private ANNClassifier() //Trains the network
        {
            isTraining = true;
            Debug.WriteLine("Running Black and White neural network...");

            //Set up matrices

            String path = Directory.GetCurrentDirectory() + "\\ANN";

            String[] trainingDirectories = Directory.GetDirectories(path + "\\Training");
            List<string[]> trainingPaths = new List<string[]>();

            foreach (String p in trainingDirectories)
            {
                trainingPaths.Add(Directory.GetFiles(p));
            }

            String[] testingDirectories = Directory.GetDirectories(path + "\\Testing");
            List<string[]> testingPaths = new List<string[]>();

            foreach (String p in testingDirectories)
            {
                testingPaths.Add(Directory.GetFiles(p));
            }

            int number_of_training_samples = 0;
            foreach (var set in trainingPaths)
                number_of_training_samples += set.Length;

            int attributes_per_sample = DIMENSIONS * DIMENSIONS; //Dimensions of the picture

            int number_of_testing_samples = 0;
            foreach (var set in testingPaths)
                number_of_testing_samples += set.Length;

            number_of_classes = trainingDirectories.Length;

            Matrix<float> training_data = new Matrix<float>(number_of_training_samples, attributes_per_sample);
            Matrix<float> training_classifications = new Matrix<float>(number_of_training_samples, number_of_classes);
            training_classifications.SetZero();

            Matrix<float> testing_data = new Matrix<float>(number_of_testing_samples, attributes_per_sample);
            Matrix<float> testing_classifications = new Matrix<float>(number_of_testing_samples, number_of_classes);
            testing_classifications.SetZero();

            Matrix<float> classification_result = new Matrix<float>(1, number_of_classes);

            //Populate training and testing data

            List<List<Image<Gray, byte>>> training_images = new List<List<Image<Gray, byte>>>();
            for (int i = 0; i < number_of_classes; i++)
            {
                training_images.Add(new List<Image<Gray, byte>>());
                foreach (var c in trainingPaths[i])
                    training_images[i].Add((new Image<Gray, byte>(c)).Resize(DIMENSIONS, DIMENSIONS, INTER.CV_INTER_CUBIC));
            }

            List<List<Image<Gray, byte>>> testing_images = new List<List<Image<Gray, byte>>>();
            for (int i = 0; i < number_of_classes; i++)
            {
                testing_images.Add(new List<Image<Gray, byte>>());
                foreach (var c in testingPaths[i])
                    testing_images[i].Add((new Image<Gray, byte>(c)).Resize(DIMENSIONS, DIMENSIONS, INTER.CV_INTER_CUBIC));
            }


            //Set classification matrix values

            int offset = 0;
            for (int directory = 0; directory < trainingDirectories.Length; directory++)
            {
                for (int samples = 0; samples < trainingPaths[directory].Length; samples++)
                {
                    training_classifications.Data[offset + samples, directory] = 1;
                }
                offset += trainingPaths[directory].Length - 1;
            }

            offset = 0;

            for (int directory = 0; directory < testingDirectories.Length; directory++)
            {
                for (int samples = 0; samples < testingPaths[directory].Length; samples++)
                {
                    testing_classifications.Data[offset + samples, directory] = 1;
                }
                offset += testingPaths[directory].Length;
            }



            //Set training and testing data
            List<Matrix<float>> flattenedImages = new List<Matrix<float>>();
            foreach (var imageList in training_images)
            {
                foreach (var img in imageList)
                {
                    Matrix<float> flattened = new Matrix<float>(1, DIMENSIONS * DIMENSIONS);
                    int c = 0;
                    for (int i = 0; i < DIMENSIONS; i++)
                        for (int j = 0; j < DIMENSIONS; j++)
                            flattened.Data[0, c++] = img.Data[i, j, 0];
                    flattenedImages.Add(flattened);
                }
            }


            training_data = ConcatDescriptors(flattenedImages);

            //Testing data
            List<Matrix<float>> flattenedTestImages = new List<Matrix<float>>();
            foreach (var imageList in testing_images)
            {
                foreach (var img in imageList)
                {
                    Matrix<float> flattened = new Matrix<float>(1, DIMENSIONS * DIMENSIONS);
                    int c = 0;
                    for (int i = 0; i < DIMENSIONS; i++)
                        for (int j = 0; j < DIMENSIONS; j++)
                            flattened.Data[0, c++] = img.Data[i, j, 0];
                    flattenedTestImages.Add(flattened);
                }
            }


            testing_data = ConcatDescriptors(flattenedTestImages);

            //Start up the Neural net
            int[] layers_d = { attributes_per_sample, DIMENSIONS / 2, DIMENSIONS / 4, number_of_classes };

            Matrix<int> layerSize = new Matrix<int>(layers_d);

            MCvANN_MLP_TrainParams parameters = new MCvANN_MLP_TrainParams(); //Parameters for Artificla Neural Network - MultiLayer Perceptron
            //The termination criteria
            parameters.term_crit = new MCvTermCriteria(1000, 1.0e-8); //Create the termination criteria using the constrain of maximum iteration as well as epsilon (learning rate)
            parameters.train_method = Emgu.CV.ML.MlEnum.ANN_MLP_TRAIN_METHOD.BACKPROP; //Sets the training method to backpropogation
            parameters.bp_dw_scale = 0.1;
            parameters.bp_moment_scale = 0.1;

            this.network = new ANN_MLP(layerSize, Emgu.CV.ML.MlEnum.ANN_MLP_ACTIVATION_FUNCTION.SIGMOID_SYM, 0.6, 1.0); //use normal sigmoid

            int iterations = network.Train(training_data, training_classifications, null, parameters, Emgu.CV.ML.MlEnum.ANN_MLP_TRAINING_FLAG.DEFAULT);

            Matrix<float> test_sample;

            int correct_class = 0;
            int wrong_class = 0;
            int[] false_positive = { 0, 0 };


            for (int tsample = 0; tsample < number_of_testing_samples; tsample++)
            {
                test_sample = testing_data.GetRow(tsample);
                network.Predict(test_sample, classification_result);

                float highest = classification_result[0, 0];
                int index = 0;
                for (int i = 1; i < number_of_classes; i++)
                {
                    if (classification_result[0, i] > highest)
                    {
                        highest = classification_result[0, i];
                        index = i;
                    }
                }
                Debug.WriteLine("{" + testing_classifications.Data[tsample, 0] + "," + testing_classifications.Data[tsample, 1] + "} was classified as {" + classification_result.Data[0, 0] + "," + classification_result.Data[0, 1] + "}");


                int _class;
                for (_class = 0; _class < number_of_classes && testing_classifications[tsample, _class] != 1; _class++) ;

                if (_class == index)
                    correct_class++;
                else
                {
                    wrong_class++;
                    false_positive[index]++;
                }
            }

            Debug.WriteLine(correct_class + " classes correctly classified.");
            Debug.WriteLine(wrong_class + " classes wrongly classified.");
            Debug.WriteLine("False Positives:");
            for (int i = 0; i < false_positive.Length; i++)
                Debug.WriteLine("Class " + i + " = " + false_positive[i]);
            Debug.WriteLine("");
            Debug.WriteLine((correct_class / (double)number_of_testing_samples) * 100.0 + "% classification accuracy");

            isTraining = false;

        }



        public bool isCurrentlyTraining(){
            return isTraining;
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

        public static ANNClassifier getInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ANNClassifier();
                        }
                    }
                }

                return instance;
            }
        }
    }
}
