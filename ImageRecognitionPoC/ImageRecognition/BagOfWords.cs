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
    class BagOfWords :  Classifier
    {
        public void runBoW()
        {
            try
            {
                //analyseConfusionMatrix(classifyORB_ANN("C:\\Users\\Aeolus\\Pictures\\SAMBUG\\ANN\\Training", false));

                //Console.WriteLine("Running SURF with SVM BoW");
                //for (int i = 0; i < 3; i++ )
                //    analyseConfusionMatrix(ClassifySURF_SVM("C:\\Users\\Aeolus\\Pictures\\SAMBUG\\ANN\\Training", false));

                Console.WriteLine("Running SURF with ANN");
                for (int i = 0; i < 5; i++)
                    analyseConfusionMatrix(ClassifySURF_ANN("C:\\Users\\Aeolus\\Pictures\\SAMBUG\\ANN\\Training", false));


                Debug.WriteLine("");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        public Matrix<int> classifyORB_ANN(string folder, bool restructure)
        {
            int number_of_clusters;
            if (restructure)
            {
                restructureTrainingData(folder);  //number of classes
                preProcessTrainingData();
            }

            double testing_part = 0.2;
            number_of_clusters = 500;   //This is just for testing purposes

            int input_num = 0;  //number of train images


            List<string> class_labels = new List<string>();


            using (ORBDetector detector = new ORBDetector(1000))
            using (BruteForceMatcher<byte> matcher = new BruteForceMatcher<byte>(DistanceType.Hamming))
            {
                BOWKMeansTrainer bowTrainer = new BOWKMeansTrainer(number_of_clusters, new MCvTermCriteria(100, 0.01), 3, Emgu.CV.CvEnum.KMeansInitType.PPCenters);
                BOWImgDescriptorExtractor<byte> bowDE = new BOWImgDescriptorExtractor<byte>(detector, matcher);
                FileInfo[] files = new DirectoryInfo(folder).GetFiles();

                List<FileInfo> training_files = new List<FileInfo>();
                List<FileInfo> testing_files = new List<FileInfo>();

                Random r = new Random();
                foreach (FileInfo file in files)
                    if (file.Extension.Equals(".db")) continue;
                    else
                        if (r.NextDouble() >= testing_part)
                            training_files.Add(file);
                        else
                            testing_files.Add(file);


                List<Matrix<float>> descriptors = new List<Matrix<float>>();

                foreach (FileInfo file in training_files)
                {
                    Image<Bgr, Byte> model = new Image<Bgr, byte>(file.FullName);
                    Image<Gray, Byte> modelGray = model.Convert<Gray, Byte>();
                    //Detect SURF key points from images
                    VectorOfKeyPoint modelKeyPoints = detector.DetectKeyPointsRaw(modelGray, null);
                    //Compute detected SURF key points & extract modelDescriptors
                    Matrix<byte> modelDescriptors = detector.ComputeDescriptorsRaw(modelGray, null, modelKeyPoints);

                    //Add the extracted BoW modelDescriptors into BOW trainer
                    descriptors.Add(byteToFloatMatrix(modelDescriptors));

                    string fileName = Path.GetFileName(file.FullName);
                    string class_category = fileName.Substring(0, fileName.IndexOf("--"));
                    if (!class_labels.Contains(class_category))
                    {
                        class_labels.Add(class_category);
                    }
                    input_num++;
                }

                //Cluster the feature vectors
                Matrix<float> desc = ConcatDescriptors(descriptors);
                bowTrainer.Add(desc);
                Matrix<float> dictionary = bowTrainer.Cluster();
                //Store the vocabulary
                bowDE.SetVocabulary(floatToByteMatrix(dictionary));
                //To store all modelBOWDescriptor in a single trainingDescriptors
                Matrix<float> trainingDescriptors = new Matrix<float>(input_num, number_of_clusters);
                //To label each modelBOWDescriptor, in this case all train images are labelled with different integer 
                //hence all images are considered as a unique class, i.e class_num = input_num
                //Matrix<float> labels = new Matrix<float>(input_num, 1);
                //Use labels of type <int> instead of <float> for NormalBayesClassifier
                //Matrix<int> labels = new Matrix<int>(input_num, 1);

                Matrix<float> training_classifications = new Matrix<float>(training_files.Count, class_labels.Count);
                Matrix<float> testing_classifications = new Matrix<float>(testing_files.Count, class_labels.Count);

                int j = 0;
                foreach (var file in testing_files)
                {
                    string fileName = Path.GetFileName(file.FullName);
                    string class_category = fileName.Substring(0, fileName.IndexOf("--"));
                    testing_classifications.Data[j++, class_labels.IndexOf(class_category)] = 1;
                }


                j = 0;
                foreach (FileInfo file in training_files)
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

                        training_classifications.Data[j++, class_labels.IndexOf(class_category)] = 1;

                    }
                }

                //Declaration for Support Vector Machine & parameters
                int[] layers_d = { number_of_clusters, number_of_clusters / 2, number_of_clusters / 4, class_labels.Count };
                int number_of_testing_samples = testing_files.Count;
                Matrix<int> layerSize = new Matrix<int>(layers_d);
                Matrix<float> classification_result = new Matrix<float>(1, class_labels.Count);
                MCvANN_MLP_TrainParams parameters = new MCvANN_MLP_TrainParams(); //Parameters for Artificla Neural Network - MultiLayer Perceptron
                //The termination criteria
                parameters.term_crit = new MCvTermCriteria(100, 1.0e-8); //Create the termination criteria using the constrain of maximum iteration as well as epsilon (learning rate)
                parameters.train_method = Emgu.CV.ML.MlEnum.ANN_MLP_TRAIN_METHOD.BACKPROP; //Sets the training method to backpropogation
                parameters.bp_dw_scale = 0.1;
                parameters.bp_moment_scale = 0.1;

                Emgu.CV.ML.ANN_MLP network = new ANN_MLP(layerSize, Emgu.CV.ML.MlEnum.ANN_MLP_ACTIVATION_FUNCTION.SIGMOID_SYM, 0.6, 1.0); //use normal sigmoid

                Console.WriteLine("Training the neural network...");

                int iterations = network.Train(trainingDescriptors, training_classifications, null, parameters, Emgu.CV.ML.MlEnum.ANN_MLP_TRAINING_FLAG.DEFAULT);


                Matrix<int> confusion_matrix = new Matrix<int>(class_labels.Count, class_labels.Count);
                confusion_matrix.SetZero();

                foreach (var file in testing_files)
                {
                    using (Image<Gray, Byte> testImgGray = new Image<Gray, byte>(file.FullName))
                    using (VectorOfKeyPoint testKeyPoints = detector.DetectKeyPointsRaw(testImgGray, null))
                    using (Matrix<float> testBOWDescriptor = bowDE.Compute(testImgGray, testKeyPoints))
                    {
                        network.Predict(testBOWDescriptor, classification_result);
                        //float result = classifier.Predict(testBOWDescriptor, null);
                        //result will indicate whether test image belongs to trainDescriptor label 1, 2 or 3  
                        string fileName = Path.GetFileName(file.FullName);
                        string class_category = fileName.Substring(0, fileName.IndexOf("--"));
                        int actual_class_index = class_labels.IndexOf(class_category);
                        int predicted_class_index = 0;
                        for (int i = 0; i < classification_result.Cols; i++)
                            if (classification_result[0, i] > classification_result[0, predicted_class_index])
                                predicted_class_index = i;


                        confusion_matrix[actual_class_index, predicted_class_index]++;


                    }
                }

                return confusion_matrix;
            }
        }

        public Matrix<int> ClassifySURF_ANN(string folder, bool restructure)
        {
            int number_of_clusters;
            if (restructure)
            {
                restructureTrainingData(folder);  //number of classes
                preProcessTrainingData();
            }

            double testing_part = 0.2;
            number_of_clusters = 500;   //This is just for testing purposes

            int input_num = 0;  //number of train images
           

            List<string> class_labels = new List<string>();


            using (SURFDetector detector = new SURFDetector(400, false))
            using (BruteForceMatcher<float> matcher = new BruteForceMatcher<float>(DistanceType.L2))
            {
                BOWKMeansTrainer bowTrainer = new BOWKMeansTrainer(number_of_clusters, new MCvTermCriteria(100, 0.01), 3, Emgu.CV.CvEnum.KMeansInitType.PPCenters);
                BOWImgDescriptorExtractor<float> bowDE = new BOWImgDescriptorExtractor<float>(detector, matcher);
                FileInfo[] files = new DirectoryInfo(folder).GetFiles();

                List<FileInfo> training_files = new List<FileInfo>();
                List<FileInfo> testing_files = new List<FileInfo>();

                Random r = new Random();
                foreach (FileInfo file in files)
                    if (file.Extension.Equals(".db")) continue;
                    else
                        if (r.NextDouble() >= testing_part)
                            training_files.Add(file);
                        else
                            testing_files.Add(file);


                List<Matrix<float>> descriptors = new List<Matrix<float>>();

                foreach (FileInfo file in training_files)
                {


                    Image<Bgr, Byte> model = new Image<Bgr, byte>(file.FullName);
                    Image<Gray, Byte> modelGray = model.Convert<Gray, Byte>();
                    //Detect SURF key points from images
                    VectorOfKeyPoint modelKeyPoints = detector.DetectKeyPointsRaw(modelGray, null);
                    //Compute detected SURF key points & extract modelDescriptors
                    Matrix<float> modelDescriptors = detector.ComputeDescriptorsRaw(modelGray, null, modelKeyPoints);

                    //Add the extracted BoW modelDescriptors into BOW trainer
                    descriptors.Add(modelDescriptors);

                    string fileName = Path.GetFileName(file.FullName);
                    string class_category = fileName.Substring(0, fileName.IndexOf("--"));
                    if (!class_labels.Contains(class_category))
                    {
                        class_labels.Add(class_category);
                    }
                    input_num++;
                }

                //Cluster the feature vectors
                Matrix<float> desc = ConcatDescriptors(descriptors);
                bowTrainer.Add(desc);
                Matrix<float> dictionary = bowTrainer.Cluster();
                //Store the vocabulary
                bowDE.SetVocabulary(dictionary);
                //To store all modelBOWDescriptor in a single trainingDescriptors
                Matrix<float> trainingDescriptors = new Matrix<float>(input_num, number_of_clusters);
                //To label each modelBOWDescriptor, in this case all train images are labelled with different integer 
                //hence all images are considered as a unique class, i.e class_num = input_num
                //Matrix<float> labels = new Matrix<float>(input_num, 1);
                //Use labels of type <int> instead of <float> for NormalBayesClassifier
                //Matrix<int> labels = new Matrix<int>(input_num, 1);

                Matrix<float> training_classifications = new Matrix<float>(training_files.Count, class_labels.Count);
                Matrix<float> testing_classifications = new Matrix<float>(testing_files.Count, class_labels.Count);

                int j = 0;
                foreach (var file in testing_files)
                {
                    string fileName = Path.GetFileName(file.FullName);
                    string class_category = fileName.Substring(0, fileName.IndexOf("--"));
                    testing_classifications.Data[j++, class_labels.IndexOf(class_category)] = 1;
                }
                    

                    j = 0;
                foreach (FileInfo file in training_files)
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

                        training_classifications.Data[j++, class_labels.IndexOf(class_category)] = 1;
                        
                    }
                }

                //Declaration for Support Vector Machine & parameters
                int[] layers_d = { number_of_clusters, number_of_clusters/2,number_of_clusters/4, class_labels.Count };
                int number_of_testing_samples = testing_files.Count;
                Matrix<int> layerSize = new Matrix<int>(layers_d);
                Matrix<float> classification_result = new Matrix<float>(1, class_labels.Count);
                MCvANN_MLP_TrainParams parameters = new MCvANN_MLP_TrainParams(); //Parameters for Artificla Neural Network - MultiLayer Perceptron
                //The termination criteria
                parameters.term_crit = new MCvTermCriteria(100, 1.0e-8); //Create the termination criteria using the constrain of maximum iteration as well as epsilon (learning rate)
                parameters.train_method = Emgu.CV.ML.MlEnum.ANN_MLP_TRAIN_METHOD.BACKPROP; //Sets the training method to backpropogation
                parameters.bp_dw_scale = 0.1;
                parameters.bp_moment_scale = 0.1;

                Emgu.CV.ML.ANN_MLP network = new ANN_MLP(layerSize, Emgu.CV.ML.MlEnum.ANN_MLP_ACTIVATION_FUNCTION.SIGMOID_SYM, 0.6, 1.0); //use normal sigmoid
                
                Console.WriteLine("Training the neural network...");

                int iterations = network.Train(trainingDescriptors, training_classifications, null, parameters, Emgu.CV.ML.MlEnum.ANN_MLP_TRAINING_FLAG.DEFAULT);
            

                Matrix<int> confusion_matrix = new Matrix<int>(class_labels.Count, class_labels.Count);
                confusion_matrix.SetZero();

                foreach (var file in testing_files)
                {
                    using (Image<Gray, Byte> testImgGray = new Image<Gray, byte>(file.FullName))
                    using (VectorOfKeyPoint testKeyPoints = detector.DetectKeyPointsRaw(testImgGray, null))
                    using (Matrix<float> testBOWDescriptor = bowDE.Compute(testImgGray, testKeyPoints))
                    {
                        network.Predict(testBOWDescriptor, classification_result);
                        //float result = classifier.Predict(testBOWDescriptor, null);
                        //result will indicate whether test image belongs to trainDescriptor label 1, 2 or 3  
                        string fileName = Path.GetFileName(file.FullName);
                        string class_category = fileName.Substring(0, fileName.IndexOf("--"));
                        int actual_class_index = class_labels.IndexOf(class_category);
                        int predicted_class_index = 0;
                        for (int i = 0; i < classification_result.Cols; i++)
                            if (classification_result[0, i] > classification_result[0, predicted_class_index])
                                predicted_class_index = i;


                            confusion_matrix[actual_class_index, predicted_class_index]++;


                    }
                }

                return confusion_matrix;
            }
        }

        public Matrix<int> ClassifySURF_SVM(string folder, bool restructure)
        {
            int class_num;
            if (restructure)
            {
                restructureTrainingData(folder);  //number of classes
                preProcessTrainingData();
            }
            
            double testing_part = 0.2;
            class_num = 500;   //This is just for testing purposes

            int input_num = 0;  //number of train images
            int j = 0;

            List<string> class_labels = new List<string>();


            using (SURFDetector detector = new SURFDetector(400,false))
            using (BruteForceMatcher<float> matcher = new BruteForceMatcher<float>(DistanceType.L2))
            {
                BOWKMeansTrainer bowTrainer = new BOWKMeansTrainer(class_num, new MCvTermCriteria(100, 0.01), 3, Emgu.CV.CvEnum.KMeansInitType.PPCenters);
                BOWImgDescriptorExtractor<float> bowDE = new BOWImgDescriptorExtractor<float>(detector, matcher);
                FileInfo[] files = new DirectoryInfo(folder).GetFiles();

                List<FileInfo> training_files = new List<FileInfo>();
                List<FileInfo> testing_files = new List<FileInfo>();

                Random r = new Random();
                foreach(FileInfo file in files)
                    if (file.Extension.Equals(".db")) continue;
                    else
                    if(r.NextDouble() >= testing_part)
                        training_files.Add(file);
                    else
	                    testing_files.Add(file);


                List<Matrix<float>> descriptors = new List<Matrix<float>>();

                foreach (FileInfo file in training_files)
                {
                    

                    Image<Bgr, Byte> model = new Image<Bgr, byte>(file.FullName);
                    Image<Gray, Byte> modelGray = model.Convert<Gray, Byte>();
                    //Detect SURF key points from images
                    VectorOfKeyPoint modelKeyPoints = detector.DetectKeyPointsRaw(modelGray, null);
                    //Compute detected SURF key points & extract modelDescriptors
                    Matrix<float> modelDescriptors = detector.ComputeDescriptorsRaw(modelGray, null, modelKeyPoints);
                    
                        //Add the extracted BoW modelDescriptors into BOW trainer
                        descriptors.Add(modelDescriptors);
                    

                    
                    string fileName = Path.GetFileName(file.FullName);
                    string class_category = fileName.Substring(0,fileName.IndexOf("--"));
                    if (!class_labels.Contains(class_category))
                    {
                        class_labels.Add(class_category);
                    }
                    input_num++;
                }

                //Cluster the feature vectors
                Matrix<float> desc = ConcatDescriptors(descriptors);
                bowTrainer.Add(desc);
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

                foreach (FileInfo file in training_files)
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
                p.TermCrit = new MCvTermCriteria(1000, 0.0001);
                bool trained = my_SVM.Train(trainingDescriptors, labels, null, null, p);

                //NormalBayesClassifier classifier = new NormalBayesClassifier();
                //classifier.Train(trainingDescriptors, labels, null, null, false);
                Matrix<int> confusion_matrix = new Matrix<int>(class_labels.Count, class_labels.Count);
                confusion_matrix.SetZero();

                foreach (var file in testing_files)
                {
                    using (Image<Gray, Byte> testImgGray = new Image<Gray,byte>(file.FullName))
                    using (VectorOfKeyPoint testKeyPoints = detector.DetectKeyPointsRaw(testImgGray, null))
                    using (Matrix<float> testBOWDescriptor = bowDE.Compute(testImgGray, testKeyPoints))
                    {
                        float result = my_SVM.Predict(testBOWDescriptor);
                        //float result = classifier.Predict(testBOWDescriptor, null);
                        //result will indicate whether test image belongs to trainDescriptor label 1, 2 or 3  
                        string fileName = Path.GetFileName(file.FullName);
                        string class_category = fileName.Substring(0,fileName.IndexOf("--"));
                        int actual_class_index = class_labels.IndexOf(class_category);
                        int predicted_class_index = (int)(result -1.0);
                        confusion_matrix[actual_class_index,predicted_class_index]++;


                    }
                }

                return confusion_matrix;
            }
        }
    }
}
