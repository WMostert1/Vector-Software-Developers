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
    class Classifier
    {
        public float Classify(Image<Bgr, Byte> testImg, string folder)
        {
            int class_num = 3;  //number of clusters/classes
            int input_num = 0;  //number of train images
            int j = 0;

            using (SURFDetector detector = new SURFDetector(500, false))
            using (BruteForceMatcher<float> matcher = new BruteForceMatcher<float>(DistanceType.L2))
            {
                BOWKMeansTrainer bowTrainer = new BOWKMeansTrainer(class_num, new MCvTermCriteria(10, 0.01), 3, Emgu.CV.CvEnum.KMeansInitType.PPCenters);
                BOWImgDescriptorExtractor<float> bowDE = new BOWImgDescriptorExtractor<float>(detector, matcher);

                FileInfo[] files = new DirectoryInfo(folder).GetFiles();
                foreach (FileInfo file in files)
                {
                    if (file.Extension.Equals(".db")) continue;
                    using (Image<Bgr, Byte> model = new Image<Bgr, byte>(file.FullName))
                    using (Image<Gray, Byte> modelGray = model.Convert<Gray, Byte>())
                    //Detect SURF key points from images
                    using (VectorOfKeyPoint modelKeyPoints = detector.DetectKeyPointsRaw(modelGray, null))
                    //Compute detected SURF key points & extract modelDescriptors
                    using (Matrix<float> modelDescriptors = detector.ComputeDescriptorsRaw(modelGray, null, modelKeyPoints))
                    {
                        //Add the extracted BoW modelDescriptors into BOW trainer
                        bowTrainer.Add(modelDescriptors);
                    }
                    input_num++;
                }

                //Cluster the feature vectors
                Matrix<float> dictionary = bowTrainer.Cluster();
                //Store the vocabulary
                bowDE.SetVocabulary(dictionary);
                //To store all modelBOWDescriptor in a single trainingDescriptors
                Matrix<float> trainingDescriptors = new Matrix<float>(input_num, class_num);
                //To label each modelBOWDescriptor, in this case all train images are labelled with different integer 
                //hence all images are considered as a unique class, i.e class_num = input_num
                Matrix<float> labels = new Matrix<float>(input_num, 1);
                //Use labels of type <int> instead of <float> for NormalBayesClassifier
                //Matrix<int> labels = new Matrix<int>(input_num, 1);

                foreach (FileInfo file in files)
                {
                    if (file.Extension.Equals(".db")) continue;
                    using (Image<Bgr, Byte> model = new Image<Bgr, byte>(file.FullName))
                    using (Image<Gray, Byte> modelGray = model.Convert<Gray, Byte>())
                    using (VectorOfKeyPoint modelKeyPoints = detector.DetectKeyPointsRaw(modelGray, null))
                    using (Matrix<float> modelBOWDescriptor = bowDE.Compute(modelGray, modelKeyPoints))
                    {
                        //To merge all modelBOWDescriptor into single trainingDescriptors
                        for (int i = 0; i < trainingDescriptors.Cols; i++)
                        {
                            trainingDescriptors.Data[j, i] = modelBOWDescriptor.Data[0, i];
                        }
                        labels.Data[j, 0] = (j + 1);
                        j++;
                    }
                }

                //Declaration for Support Vector Machine & parameters
                SVM my_SVM = new SVM();
                SVMParams p = new SVMParams();
                p.KernelType = Emgu.CV.ML.MlEnum.SVM_KERNEL_TYPE.LINEAR;
                p.SVMType = Emgu.CV.ML.MlEnum.SVM_TYPE.C_SVC;
                p.C = 1;
                p.TermCrit = new MCvTermCriteria(100, 0.00001);
                bool trained = my_SVM.Train(trainingDescriptors, labels, null, null, p);

                //NormalBayesClassifier classifier = new NormalBayesClassifier();
                //classifier.Train(trainingDescriptors, labels, null, null, false);

                using (Image<Gray, Byte> testImgGray = testImg.Convert<Gray, Byte>())
                using (VectorOfKeyPoint testKeyPoints = detector.DetectKeyPointsRaw(testImgGray, null))
                using (Matrix<float> testBOWDescriptor = bowDE.Compute(testImgGray, testKeyPoints))
                {
                   
                    float result = my_SVM.Predict(testBOWDescriptor);
                    //float result = classifier.Predict(testBOWDescriptor, null);
                    //result will indicate whether test image belongs to trainDescriptor label 1, 2 or 3  
                    return result;
                }
            }
        }

        public void runBoW()
        {
            try
            {
                float result = ClassifyUsingBoW(new Image<Bgr, Byte>("C:\\Users\\Aeolus\\Pictures\\SAMBUG\\coconut25.jpg"), "C:\\Users\\Aeolus\\Pictures\\SAMBUG\\ANN\\Training");
                Debug.WriteLine("");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public int restructureTrainingData(string root_folder)
        {
            if (Directory.Exists(root_folder + "\\Training_Data"))
                Directory.Delete(root_folder + "\\Training_Data", true);

            DirectoryInfo root = new DirectoryInfo(root_folder);
            DirectoryInfo[] directories_of_classes = root.GetDirectories();
           
            DirectoryInfo output_folder = root.CreateSubdirectory("Training_Data");
            foreach (var directory in directories_of_classes)
            {
                int number = 0;
                foreach (var file in directory.GetFiles())
                {
                    string path = output_folder.FullName + "\\" + Path.GetFileName(directory.FullName) + "--" + (number++) + file.Extension;
                    if(!File.Exists(path))
                        file.CopyTo(path);
                }
            }
            return directories_of_classes.Length;
        }

        public float ClassifyUsingBoW(Image<Bgr, Byte> testImg, string folder)
        {
            

            int class_num = restructureTrainingData(folder);  //number of clusters/classes
            int input_num = 0;  //number of train images
            int j = 0;

            List<string> class_labels = new List<string>();


            using (SURFDetector detector = new SURFDetector(500, false))
            using (BruteForceMatcher<float> matcher = new BruteForceMatcher<float>(DistanceType.L2))
            {
                BOWKMeansTrainer bowTrainer = new BOWKMeansTrainer(class_num, new MCvTermCriteria(10, 0.01), 3, Emgu.CV.CvEnum.KMeansInitType.PPCenters);
                BOWImgDescriptorExtractor<float> bowDE = new BOWImgDescriptorExtractor<float>(detector, matcher);

                FileInfo[] files = new DirectoryInfo(folder+"\\Training_Data").GetFiles();
                foreach (FileInfo file in files)
                {
                    if (file.Extension.Equals(".db")) continue;
        
                    using (Image<Bgr, Byte> model = new Image<Bgr, byte>(file.FullName))
                    using (Image<Gray, Byte> modelGray = model.Convert<Gray, Byte>())
                    //Detect SURF key points from images
                    using (VectorOfKeyPoint modelKeyPoints = detector.DetectKeyPointsRaw(modelGray, null))
                    //Compute detected SURF key points & extract modelDescriptors
                    using (Matrix<float> modelDescriptors = detector.ComputeDescriptorsRaw(modelGray, null, modelKeyPoints))
                    {
                        //Add the extracted BoW modelDescriptors into BOW trainer
                        bowTrainer.Add(modelDescriptors);
                    }
                    
                    string fileName = Path.GetFileName(file.FullName);
                    string class_category = fileName.Substring(0,fileName.IndexOf("--"));
                    if (!class_labels.Contains(class_category))
                    {
                        class_labels.Add(class_category);
                    }
                    input_num++;
                }

                //Cluster the feature vectors
                Matrix<float> dictionary = bowTrainer.Cluster();
                //Store the vocabulary
                bowDE.SetVocabulary(dictionary);
                //To store all modelBOWDescriptor in a single trainingDescriptors
                Matrix<float> trainingDescriptors = new Matrix<float>(input_num, class_num);
                //To label each modelBOWDescriptor, in this case all train images are labelled with different integer 
                //hence all images are considered as a unique class, i.e class_num = input_num
                Matrix<float> labels = new Matrix<float>(input_num, 1);
                //Use labels of type <int> instead of <float> for NormalBayesClassifier
                //Matrix<int> labels = new Matrix<int>(input_num, 1);

                foreach (FileInfo file in files)
                {
                    if (file.Extension.Equals(".db")) continue;

                    using (Image<Bgr, Byte> model = new Image<Bgr, byte>(file.FullName))
                    using (Image<Gray, Byte> modelGray = model.Convert<Gray, Byte>())
                    using (VectorOfKeyPoint modelKeyPoints = detector.DetectKeyPointsRaw(modelGray, null))
                    using (Matrix<float> modelBOWDescriptor = bowDE.Compute(modelGray, modelKeyPoints))
                    {
                        //To merge all modelBOWDescriptor into single trainingDescriptors
                        for (int i = 0; i < trainingDescriptors.Cols; i++)
                        {
                            trainingDescriptors.Data[j, i] = modelBOWDescriptor.Data[0, i];
                        }
                        string fileName = Path.GetFileName(file.FullName);
                        string class_category = fileName.Substring(0, fileName.IndexOf("--"));

                        labels.Data[j, 0] = class_labels.IndexOf(class_category)+1;
                        j++;
                    }
                }

                //Declaration for Support Vector Machine & parameters
                SVM my_SVM = new SVM();
                SVMParams p = new SVMParams();
                p.KernelType = Emgu.CV.ML.MlEnum.SVM_KERNEL_TYPE.LINEAR;
                p.SVMType = Emgu.CV.ML.MlEnum.SVM_TYPE.C_SVC;
                p.C = 1;
                p.TermCrit = new MCvTermCriteria(100, 0.00001);
                bool trained = my_SVM.Train(trainingDescriptors, labels, null, null, p);

                //NormalBayesClassifier classifier = new NormalBayesClassifier();
                //classifier.Train(trainingDescriptors, labels, null, null, false);

                using (Image<Gray, Byte> testImgGray = testImg.Convert<Gray, Byte>())
                using (VectorOfKeyPoint testKeyPoints = detector.DetectKeyPointsRaw(testImgGray, null))
                using (Matrix<float> testBOWDescriptor = bowDE.Compute(testImgGray, testKeyPoints))
                {
                    float result = my_SVM.Predict(testBOWDescriptor);
                    //float result = classifier.Predict(testBOWDescriptor, null);
                    //result will indicate whether test image belongs to trainDescriptor label 1, 2 or 3  
                    return result;
                }
            }
        }

        //------------------------------------------------------   <wifi_BEGIN>
        //              Black and White neural network
        //------------------------------------------------------

        public float runBWANNTesting(int[] layers_d, float dw_scale, float moment_scale, float alpha, float beta, Matrix<float> training_data, Matrix<float> training_classifications, Matrix<float> testing_data, Matrix<float> testing_classifications, int number_of_testing_samples, int number_of_classes)
        {
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
            //Console.WriteLine("Training the neural network...");

            int iterations = network.Train(training_data, training_classifications, null, parameters, Emgu.CV.ML.MlEnum.ANN_MLP_TRAINING_FLAG.DEFAULT);
           // Console.WriteLine("Iterations: " + iterations);
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
                //  Console.Write("{" + testing_classifications.Data[tsample, 0]);
                //      for(int i = 1 ; i < number_of_classes; i++)
                //         Console.Write("," + testing_classifications.Data[tsample, i]);
                // Console.Write("} was classified as {" + classification_result.Data[0, 0]);
                //    for(int i = 1 ; i < number_of_classes; i++)
                //         Console.Write("," + classification_result.Data[0, i]);

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

            //Console.WriteLine(correct_class +" classes correctly classified.");
            // Console.WriteLine(wrong_class + " classes wrongly classified.");
            // Console.WriteLine("False Positives:");
            //   for(int i = 0; i < false_positive.Length; i++)
            //Console.WriteLine("Class "+i+" = "+false_positive[i]);
            //Console.WriteLine("");
            //Console.WriteLine((correct_class/(double)number_of_testing_samples)*100.0 + "% classification accuracy");
            //Console.ReadLine();
            return correct_class / (float)number_of_testing_samples * (float)100.0;
        }

        public int getMatrices(out Matrix<float> training_data,out Matrix<float> training_classifications,out Matrix<float> testing_data,out Matrix<float> testing_classifications)
        {
            //Set up matrices

                String path = "C:\\Users\\Aeolus\\Pictures\\SAMBUG\\ANN\\";
                String[] trainingDirectories = Directory.GetDirectories(path + "Training");
                List<string[]> trainingPaths = new List<string[]>();

                foreach (String p in trainingDirectories)
                {
                    trainingPaths.Add(Directory.GetFiles(p));
                }

                String[] testingDirectories = Directory.GetDirectories(path + "Testing");
                List<string[]> testingPaths = new List<string[]>();

                foreach (String p in testingDirectories)
                {
                    testingPaths.Add(Directory.GetFiles(p));
                }

                int number_of_training_samples = 0;
                foreach (var set in trainingPaths)
                    number_of_training_samples += set.Length;

                const int DIMENSIONS = 64;

                int attributes_per_sample = DIMENSIONS * DIMENSIONS; //Dimensions of the picture

                int number_of_testing_samples = 0;
                foreach (var set in testingPaths)
                    number_of_testing_samples += set.Length;

                int number_of_classes = trainingDirectories.Length;

                 training_data = new Matrix<float>(number_of_training_samples, attributes_per_sample);
                 training_classifications = new Matrix<float>(number_of_training_samples, number_of_classes);
                training_classifications.SetZero();

                 testing_data = new Matrix<float>(number_of_testing_samples, attributes_per_sample);
                testing_classifications = new Matrix<float>(number_of_testing_samples, number_of_classes);
                testing_classifications.SetZero();

                Matrix<float> classification_result = new Matrix<float>(1, number_of_classes);
                Point max_loc = new Point(0, 0);

                //Populate training and testing data
                Console.WriteLine("Loading images from disk...");
                List<List<Image<Gray, byte>>> training_images = new List<List<Image<Gray, byte>>>();
                for (int i = 0; i < number_of_classes; i++)
                {
                    training_images.Add(new List<Image<Gray, byte>>());
                    foreach (var c in trainingPaths[i])
                    {
                        if (c.Contains("Thumbs.db"))
                        {
                            training_images[i].Add((new Image<Gray, byte>(trainingPaths[i][2])).Resize(DIMENSIONS, DIMENSIONS, INTER.CV_INTER_CUBIC));
                        }
                        else
                            training_images[i].Add((new Image<Gray, byte>(c)).Resize(DIMENSIONS, DIMENSIONS, INTER.CV_INTER_CUBIC));
                    }
                }

                List<List<Image<Gray, byte>>> testing_images = new List<List<Image<Gray, byte>>>();
                for (int i = 0; i < number_of_classes; i++)
                {
                    testing_images.Add(new List<Image<Gray, byte>>());
                    foreach (var c in testingPaths[i])
                        testing_images[i].Add((new Image<Gray, byte>(c)).Resize(DIMENSIONS, DIMENSIONS, INTER.CV_INTER_CUBIC));
                }


                //Set classification matrix values
                Console.WriteLine("Converting images to numerical format...");
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
                        //Emgu.CV.UI.ImageViewer.Show(img);
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
                        //Emgu.CV.UI.ImageViewer.Show(img);
                        Matrix<float> flattened = new Matrix<float>(1, DIMENSIONS * DIMENSIONS);
                        int c = 0;
                        for (int i = 0; i < DIMENSIONS; i++)
                            for (int j = 0; j < DIMENSIONS; j++)
                                flattened.Data[0, c++] = img.Data[i, j, 0];
                        flattenedTestImages.Add(flattened);
                    }
                }


                testing_data = ConcatDescriptors(flattenedTestImages);
                return number_of_testing_samples;
            
        }

        public Image<Bgr, byte> doGrabCut(Image<Bgr, byte> image)
        {

            //1. Convert the image to grayscale.

            int numberOfIterations = 15;
            Image<Gray, byte> grayImage = image.Convert<Gray, Byte>();

            //2. Threshold it using otsu.


            grayImage = getOtsuThresholdedImage(grayImage);
           // Emgu.CV.UI.ImageViewer.Show(grayImage);
            grayImage._Not();

            //3. Extract the contours.
            Rectangle ROI = new Rectangle(0,0,0,0);
            #region Extracting the Contours
            using (MemStorage storage = new MemStorage())
            {
                for (Contour<Point> contours = grayImage.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE, storage); contours != null; contours = contours.HNext)
                {

                    Contour<Point> currentContour = contours.ApproxPoly(contours.Perimeter * 0.015, storage);
                    if (currentContour.BoundingRectangle.Width > ROI.Width)
                    {
                        //CvInvoke.cvDrawContours(color, contours, new MCvScalar(255), new MCvScalar(255), -1, 2, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, new Point(0, 0));
                        //color.Draw(currentContour.BoundingRectangle, new Bgr(0, 255, 0), 1);
                        //To crop the image around the Region of Interest
                        ROI = currentContour.BoundingRectangle;
                        
                    }

                }

            }
            #endregion
            //Emgu.CV.UI.ImageViewer.Show(image);
            //4. Setting the results to the output variables.

            #region Asigning output
           
            Image<Gray, byte> mask = image.GrabCut(ROI, numberOfIterations);
            
            
            mask = mask.ThresholdBinary(new Gray(2), new Gray(255));
        
            Image<Bgr, byte> result = image.Copy(mask);

            if (ROI.Width < ROI.Height)
                ROI.Width = ROI.Height;
            else if (ROI.Height < ROI.Width)
                ROI.Height = ROI.Width;

            result.ROI = ROI;
            result = result.Resize(256, 256, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            for (int x = 0; x < result.Width; x++)
                for (int y = 0; y < result.Height; y++)
                    if (result.Data[x, y, 0] == 0 && result.Data[x, y, 1] == 0 && result.Data[x, y, 2] == 0){
                        result.Data[x, y, 0] = 255;
                        result.Data[x, y, 1] = 255;
                        result.Data[x, y, 2] = 255;
                    }



            #endregion
                    return result;
        }

        public Image<Gray, byte> getOtsuThresholdedImage(Image<Gray, byte> Img_Org_Gray)
        {
            Image<Gray, byte> Img_Source_Gray = Img_Org_Gray.Copy();
            Image<Gray, byte> Img_Otsu_Gray = Img_Org_Gray.CopyBlank();

            CvInvoke.cvThreshold(Img_Source_Gray.Ptr, Img_Otsu_Gray.Ptr, 0, 255, Emgu.CV.CvEnum.THRESH.CV_THRESH_OTSU | Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY);

            return Img_Otsu_Gray;
        }


        public Image<Gray, byte> doPreProcessing(Image<Gray, byte> image)
        {
            return null;
        }

        public float runBWANN(int [] layers_d, float dw_scale, float moment_scale, float alpha, float beta)
        {
            try
            {
                Console.WriteLine("Running Black and White neural network...");

                //Set up matrices

                String path = "C:\\Users\\Aeolus\\Pictures\\SAMBUG\\ANN\\";
                String[] trainingDirectories = Directory.GetDirectories(path + "Training");
                List<string[]> trainingPaths = new List<string[]>();

                foreach (String p in trainingDirectories)
                {
                    trainingPaths.Add(Directory.GetFiles(p));
                }

                String[] testingDirectories = Directory.GetDirectories(path + "Testing");
                List<string[]> testingPaths = new List<string[]>();

                foreach (String p in testingDirectories)
                {
                    testingPaths.Add(Directory.GetFiles(p));
                }

                int number_of_training_samples = 0;
                foreach (var set in trainingPaths)
                    number_of_training_samples += set.Length;

                const int DIMENSIONS = 64;

                int attributes_per_sample = DIMENSIONS * DIMENSIONS; //Dimensions of the picture

                int number_of_testing_samples = 0;
                foreach (var set in testingPaths)
                    number_of_testing_samples += set.Length;

                int number_of_classes = trainingDirectories.Length;

                Matrix<float> training_data = new Matrix<float>(number_of_training_samples, attributes_per_sample);
                Matrix<float> training_classifications = new Matrix<float>(number_of_training_samples, number_of_classes);
                training_classifications.SetZero();

                Matrix<float> testing_data = new Matrix<float>(number_of_testing_samples, attributes_per_sample);
                Matrix<float> testing_classifications = new Matrix<float>(number_of_testing_samples, number_of_classes);
                testing_classifications.SetZero();

                Matrix<float> classification_result = new Matrix<float>(1, number_of_classes);
                Point max_loc = new Point(0, 0);

                //Populate training and testing data
                Console.WriteLine("Loading images from disk...");
                List<List<Image<Gray, byte>>> training_images = new List<List<Image<Gray, byte>>>();
                for (int i = 0; i < number_of_classes; i++)
                {
                    training_images.Add(new List<Image<Gray, byte>>());
                    foreach (var c in trainingPaths[i])
                    {
                        if (c.Contains("Thumbs.db"))
                        {
                            training_images[i].Add((new Image<Gray, byte>(trainingPaths[i][2])).Resize(DIMENSIONS, DIMENSIONS, INTER.CV_INTER_CUBIC));
                        }
                        else
                            training_images[i].Add((new Image<Gray, byte>(c)).Resize(DIMENSIONS, DIMENSIONS, INTER.CV_INTER_CUBIC));
                    }
                }

                List<List<Image<Gray, byte>>> testing_images = new List<List<Image<Gray, byte>>>();
                for (int i = 0; i < number_of_classes; i++)
                {
                    testing_images.Add(new List<Image<Gray, byte>>());
                    foreach (var c in testingPaths[i])
                        testing_images[i].Add((new Image<Gray, byte>(c)).Resize(DIMENSIONS, DIMENSIONS, INTER.CV_INTER_CUBIC));
                }


                //Set classification matrix values
                Console.WriteLine("Converting images to numerical format...");
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
                        //Emgu.CV.UI.ImageViewer.Show(img);
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
                        //Emgu.CV.UI.ImageViewer.Show(img);
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
                //int [] layers_d = { attributes_per_sample,  ,  number_of_classes};

                Matrix<int> layerSize = new Matrix<int>(layers_d);

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
                    //  Console.Write("{" + testing_classifications.Data[tsample, 0]);
                    //      for(int i = 1 ; i < number_of_classes; i++)
                    //         Console.Write("," + testing_classifications.Data[tsample, i]);
                    // Console.Write("} was classified as {" + classification_result.Data[0, 0]);
                    //    for(int i = 1 ; i < number_of_classes; i++)
                    //         Console.Write("," + classification_result.Data[0, i]);

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

                //Console.WriteLine(correct_class +" classes correctly classified.");
                // Console.WriteLine(wrong_class + " classes wrongly classified.");
                // Console.WriteLine("False Positives:");
                //   for(int i = 0; i < false_positive.Length; i++)
                //Console.WriteLine("Class "+i+" = "+false_positive[i]);
                //Console.WriteLine("");
                //Console.WriteLine((correct_class/(double)number_of_testing_samples)*100.0 + "% classification accuracy");
                //Console.ReadLine();
                return correct_class / (float)number_of_testing_samples * (float)100.0;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return 0.0F;
        }


        public Matrix<float> convertBinaryDescriptorsToBase10Float(Matrix<byte> desc)
        {
            Matrix<float> result = new Matrix<float>(desc.Rows, 1);
            
            for (int i = 0; i < desc.Rows; i++)
            {
                String binaryString = "";
                for (int c = 0; c < desc.Cols; c++)
                {
                    string binary = Convert.ToString(desc[i, c], 2);
                    if (binary.Length != 8)
                    {
                        int diff = 8 - binary.Length;
                        for (int b = 0; b < diff; b++)
                            binary = "0" + binary;
                    }
                    
                    binaryString += binary;
                }
                Debug.WriteLine(binaryString);
                result[i, 0] = (float)binaryString.Length;
            }
            return result;
        }

      

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
                Emgu.CV.UI.ImageViewer.Show(new Image<Bgr,byte>(queryImage));

                Console.WriteLine("Query image matches this:");
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


        public Matrix<float> getSURFFeatureDescriptorMatrix(Image<Gray, byte> modelImage,int hessian, bool extended)
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
        //                                                    ANN Attempt
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
