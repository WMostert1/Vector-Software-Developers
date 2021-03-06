﻿using System.Collections.Generic;
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
    class NeuralNetwork : Classifier
    {
        public void run()
        {
            float highest = 0.0F;
            Config high = null;

            Matrix<float> testing_data;
            Matrix<float> testing_classes;
            Matrix<float> training_data;
            Matrix<float> training_classes;


            int number_of_classes = computeTrainingAndTestingMatrices("C:\\Users\\Aeolus\\Pictures\\SAMBUG\\ANN\\Training", out training_data, out training_classes, out testing_data, out testing_classes);
            int number_of_testing_samples = testing_data.Rows;

            List<Config> configurations = new List<Config>();


            for (float dw = 0.1F; dw <= 0.1F; dw += 0.02F)
            {
                for (float moment = 0.1F; moment <= 0.9F; moment += 0.2F)
                {
                    for (float alpha = 0.1F; alpha <= 1.0F; alpha += 0.1F)
                    {
                        for (float beta = 0.1F; beta <= 1.0F; beta += 0.1F)
                        {
                            int input = 64 * 64;
                            int[] layers = { input, 3, number_of_classes };
                            try
                            {
                                float result = this.runANN(layers, dw, moment, alpha, beta, training_data, training_classes, testing_data, testing_classes, number_of_classes);
                                var config = new Config { layers = layers, dw = dw, moment = moment, alpha = alpha, beta = beta, accuracy = result };
                                if (result > highest)
                                {
                                    highest = result;
                                    high = config;
                                    Console.WriteLine(high.toString());
                                }

                                configurations.Add(config);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }

                        }
                    }
                }
            }


            Console.WriteLine(high.toString());
            Console.ReadLine();
        }

        protected int computeTrainingAndTestingMatrices(String path, out Matrix<float> training_data, out Matrix<float> training_classifications, out Matrix<float> testing_data, out Matrix<float> testing_classifications)
        {
            //Set up matrices
            float training_percentage = 0.2F;

            FileInfo[] files = new DirectoryInfo(path).GetFiles();



            const int DIMENSIONS = 64;

            int attributes_per_sample = DIMENSIONS * DIMENSIONS; //Dimensions of the picture



            //Populate training data
            Console.WriteLine("Loading images from disk...");
            List<Image<Gray, byte>> training_images = new List<Image<Gray, byte>>();
            List<Image<Gray, byte>> testing_images = new List<Image<Gray, byte>>();
            List<String> class_labels_unique = new List<String>();
            List<String> class_labels_training = new List<String>();
            List<String> class_labels_testing = new List<String>();

            Random r = new Random();

            foreach (var file in files)
            {
                if (file.Extension.Equals(".db")) continue;  //random db file for windows sytems

                string fileName = Path.GetFileName(file.FullName);
                string class_label = fileName.Substring(0, fileName.IndexOf("--"));
                if (!class_labels_unique.Contains(class_label))
                    class_labels_unique.Add(class_label);


                if ((float)r.NextDouble() > training_percentage)
                {
                    training_images.Add(new Image<Gray, byte>(file.FullName));
                    class_labels_training.Add(class_label);
                }
                else
                {
                    testing_images.Add(new Image<Gray, byte>(file.FullName));
                    class_labels_testing.Add(class_label);
                }
            }

            int number_of_classes = class_labels_unique.Count;
            int number_of_training_samples = class_labels_training.Count;
            int number_of_testing_samples = class_labels_testing.Count;

            training_data = new Matrix<float>(number_of_training_samples, attributes_per_sample);
            training_classifications = new Matrix<float>(number_of_training_samples, number_of_classes);
            training_classifications.SetZero();

            testing_data = new Matrix<float>(number_of_testing_samples, attributes_per_sample);
            testing_classifications = new Matrix<float>(number_of_testing_samples, number_of_classes);
            testing_classifications.SetZero();

            Matrix<float> classification_result = new Matrix<float>(1, number_of_classes);

            //Set classification matrix values
            Console.WriteLine("Converting images to numerical format...");
            int count = 0;
            foreach (var class_label in class_labels_training)
                training_classifications.Data[count++, class_labels_unique.IndexOf(class_label)] = 1;

            count = 0;
            foreach (var class_label in class_labels_testing)
                testing_classifications.Data[count++, class_labels_unique.IndexOf(class_label)] = 1;





            //Set training and testing data
            List<Matrix<float>> flattenedImages = new List<Matrix<float>>();
            foreach (var img in training_images)
            {

                //Emgu.CV.UI.ImageViewer.Show(img);
                Matrix<float> flattened = new Matrix<float>(1, DIMENSIONS * DIMENSIONS);
                int c = 0;
                for (int i = 0; i < DIMENSIONS; i++)
                    for (int j = 0; j < DIMENSIONS; j++)
                        flattened.Data[0, c++] = img.Data[i, j, 0] / 255F;
                flattenedImages.Add(flattened);

            }


            training_data = ConcatDescriptors(flattenedImages);

            //Testing data
            List<Matrix<float>> flattenedTestImages = new List<Matrix<float>>();
            foreach (var img in testing_images)
            {
                //Emgu.CV.UI.ImageViewer.Show(img);
                Matrix<float> flattened = new Matrix<float>(1, DIMENSIONS * DIMENSIONS);
                int c = 0;
                for (int i = 0; i < DIMENSIONS; i++)
                    for (int j = 0; j < DIMENSIONS; j++)
                        flattened.Data[0, c++] = img.Data[i, j, 0];
                flattenedTestImages.Add(flattened);

            }


            testing_data = ConcatDescriptors(flattenedTestImages);
            return class_labels_unique.Count;

        }

        private float runANN(int[] layers_d, float dw_scale, float moment_scale, float alpha, float beta, Matrix<float> training_data, Matrix<float> training_classifications, Matrix<float> testing_data, Matrix<float> testing_classifications, int number_of_classes)
        {
            int number_of_testing_samples = testing_data.Rows;
            Matrix<int> layerSize = new Matrix<int>(layers_d);
            Matrix<float> classification_result = new Matrix<float>(1, number_of_classes);
            MCvANN_MLP_TrainParams parameters = new MCvANN_MLP_TrainParams(); //Parameters for Artificla Neural Network - MultiLayer Perceptron
            //The termination criteria
            parameters.term_crit = new MCvTermCriteria(100, 1.0e-8); //Create the termination criteria using the constrain of maximum iteration as well as epsilon (learning rate)
            parameters.train_method = Emgu.CV.ML.MlEnum.ANN_MLP_TRAIN_METHOD.BACKPROP; //Sets the training method to backpropogation
            parameters.bp_dw_scale = dw_scale;
            parameters.bp_moment_scale = moment_scale;

            //Emgu.CV.ML.ANN_MLP network = new ANN_MLP(layerSize, Emgu.CV.ML.MlEnum.ANN_MLP_ACTIVATION_FUNCTION.SIGMOID_SYM, 0.4, 1.0); //use normal sigmoid
            Emgu.CV.ML.ANN_MLP network = new ANN_MLP(layerSize, Emgu.CV.ML.MlEnum.ANN_MLP_ACTIVATION_FUNCTION.SIGMOID_SYM, alpha, beta);
            Console.WriteLine("Training the neural network...");

            int iterations = network.Train(training_data, training_classifications, null, parameters, Emgu.CV.ML.MlEnum.ANN_MLP_TRAINING_FLAG.DEFAULT);
            Console.WriteLine("Iterations: " + iterations);
            //    <wifi_BEGIN>

            Matrix<float> test_sample;

            int correct_class = 0;
            int wrong_class = 0;
            int[] false_positive = new int[number_of_classes];
            for (int i = 0; i < false_positive.Length; i++)
                false_positive[i] = 0;


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
                //     Console.Write("{" + testing_classifications.Data[tsample, 0]);
                //        for(int i = 1 ; i < number_of_classes; i++)
                //            Console.Write("," + testing_classifications.Data[tsample, i]);
                //     Console.Write("} was classified as {" + classification_result.Data[0, 0]);
                //        for(int i = 1 ; i < number_of_classes; i++)
                //              Console.Write("," + classification_result.Data[0, i]);

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

            //  Console.WriteLine(correct_class +" classes correctly classified.");
            //  Console.WriteLine(wrong_class + " classes wrongly classified.");
            //   Console.WriteLine("False Positives:");
            //    for(int i = 0; i < false_positive.Length; i++)
            // Console.WriteLine("Class "+i+" = "+false_positive[i]);
            // Console.WriteLine("");
            // Console.WriteLine((correct_class/(double)number_of_testing_samples)*100.0 + "% classification accuracy");

            return correct_class / (float)number_of_testing_samples * (float)100.0;
        }
    }
}
