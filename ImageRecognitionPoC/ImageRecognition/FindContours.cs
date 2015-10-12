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
using System.IO;

namespace ImageRecognition
{
    class ImageMagic
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



        /// <summary>
        /// Draw the model image and observed image, the matched features and homography projection.
        /// </summary>
        /// <param name="modelImage">The model image</param>
        /// <param name="observedImage">The observed image</param>
        /// <param name="matchTime">The output total time for computing the homography matrix.</param>
        /// <returns>The model image and observed image, the matched features and homography projection.</returns>
        public Image<Bgr, Byte> DrawSURFMatches(Image<Gray, Byte> modelImage, Image<Gray, byte> observedImage, out long matchTime)
        {
            Stopwatch watch;
            HomographyMatrix homography = null;

            SURFDetector surfCPU = new SURFDetector(500, false);
            VectorOfKeyPoint modelKeyPoints;
            VectorOfKeyPoint observedKeyPoints;
            Matrix<int> indices;

            Matrix<byte> mask;
            int k = 2;
            double uniquenessThreshold = 0.8;
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
                    watch = Stopwatch.StartNew();

                    // extract features from the observed image
                    using (GpuImage<Gray, Byte> gpuObservedImage = new GpuImage<Gray, byte>(observedImage))
                    using (GpuMat<float> gpuObservedKeyPoints = surfGPU.DetectKeyPointsRaw(gpuObservedImage, null))
                    using (GpuMat<float> gpuObservedDescriptors = surfGPU.ComputeDescriptorsRaw(gpuObservedImage, null, gpuObservedKeyPoints))
                    using (GpuMat<int> gpuMatchIndices = new GpuMat<int>(gpuObservedDescriptors.Size.Height, k, 1, true))
                    using (GpuMat<float> gpuMatchDist = new GpuMat<float>(gpuObservedDescriptors.Size.Height, k, 1, true))
                    using (GpuMat<Byte> gpuMask = new GpuMat<byte>(gpuMatchIndices.Size.Height, 1, 1))
                    using (Emgu.CV.GPU.Stream stream = new Emgu.CV.GPU.Stream())
                    {
                        matcher.KnnMatchSingle(gpuObservedDescriptors, gpuModelDescriptors, gpuMatchIndices, gpuMatchDist, k, null, stream);
                        indices = new Matrix<int>(gpuMatchIndices.Size);
                        mask = new Matrix<byte>(gpuMask.Size);

                        //gpu implementation of voteForUniquess
                        using (GpuMat<float> col0 = gpuMatchDist.Col(0))
                        using (GpuMat<float> col1 = gpuMatchDist.Col(1))
                        {
                            GpuInvoke.Multiply(col1, new MCvScalar(uniquenessThreshold), col1, stream);
                            GpuInvoke.Compare(col0, col1, gpuMask, CMP_TYPE.CV_CMP_LE, stream);
                        }

                        observedKeyPoints = new VectorOfKeyPoint();
                        surfGPU.DownloadKeypoints(gpuObservedKeyPoints, observedKeyPoints);

                        //wait for the stream to complete its tasks
                        //We can perform some other CPU intesive stuffs here while we are waiting for the stream to complete.
                        stream.WaitForCompletion();

                        gpuMask.Download(mask);
                        gpuMatchIndices.Download(indices);

                        if (GpuInvoke.CountNonZero(gpuMask) >= 4)
                        {
                            int nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
                            if (nonZeroCount >= 4)
                                homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask, 2);
                        }

                        watch.Stop();
                    }
                }
            }
            else
            {
                //extract features from the object image
                modelKeyPoints = surfCPU.DetectKeyPointsRaw(modelImage, null);
                Matrix<float> modelDescriptors = surfCPU.ComputeDescriptorsRaw(modelImage, null, modelKeyPoints);

                watch = Stopwatch.StartNew();

                // extract features from the observed image
                observedKeyPoints = surfCPU.DetectKeyPointsRaw(observedImage, null);
                Matrix<float> observedDescriptors = surfCPU.ComputeDescriptorsRaw(observedImage, null, observedKeyPoints);

                BruteForceMatcher<float> matcher = new BruteForceMatcher<float>(DistanceType.L2);
                matcher.Add(modelDescriptors);

                indices = new Matrix<int>(observedDescriptors.Rows, k);
                using (Matrix<float> dist = new Matrix<float>(observedDescriptors.Rows, k))
                {
                    matcher.KnnMatch(observedDescriptors, indices, dist, k, null);
                    mask = new Matrix<byte>(dist.Rows, 1);
                    mask.SetValue(255);
                    Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);
                }

                int nonZeroCount = CvInvoke.cvCountNonZero(mask);
                if (nonZeroCount >= 4)
                {
                    nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
                    if (nonZeroCount >= 4)
                        homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask, 2);
                }

                watch.Stop();
            }

            //Draw the matched keypoints
            Image<Bgr, Byte> result = Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
               indices, new Bgr(255, 255, 255), new Bgr(255, 255, 255), mask, Features2DToolbox.KeypointDrawType.DEFAULT);

            #region draw the projected region on the image
            if (homography != null)
            {  //draw a rectangle along the projected model
                Rectangle rect = modelImage.ROI;
                PointF[] pts = new PointF[] { 
               new PointF(rect.Left, rect.Bottom),
               new PointF(rect.Right, rect.Bottom),
               new PointF(rect.Right, rect.Top),
               new PointF(rect.Left, rect.Top)};
                homography.ProjectPoints(pts);

                result.DrawPolyline(Array.ConvertAll<PointF, Point>(pts, Point.Round), true, new Bgr(Color.Red), 5);
            }
            #endregion

            matchTime = watch.ElapsedMilliseconds;

            return result;
        }


        //With three classes
        public void runANNDerivitive()
        {
            try
                {
            int trainSampleCount = 150;

            #region Generate the traning data and classes
            Matrix<float> trainData = new Matrix<float>(trainSampleCount, 3);
            Matrix<float> trainClasses = new Matrix<float>(trainSampleCount, 1);

            //This is the output image (showing visually the result of the neural network computation)
            Image<Bgr, Byte> img = new Image<Bgr, byte>(500, 500);

            Matrix<float> sample = new Matrix<float>(1, 3);
            Matrix<float> prediction = new Matrix<float>(1, 1);

            //Training data1 is half of training set
            Matrix<float> trainData1 = trainData.GetRows(0, trainSampleCount /3, 1);  //trainSampleCount >> 1 = trainSampleCount/2

            //Inplace fills Array with normally distributed random numbers
            trainData1.SetRandNormal(new MCvScalar(200), new MCvScalar(50));

            //Return the matrix corresponding to a specified row span of the input array
            Matrix<float> trainData2 = trainData.GetRows(trainSampleCount/3, trainSampleCount*2/3, 1);

            //Inplace fills Array with normally distributed random numbers
            trainData2.SetRandNormal(new MCvScalar(300), new MCvScalar(50));

            Matrix<float> trainData3 = trainData.GetRows(trainSampleCount*2/3, trainSampleCount,1);

            trainData3.SetRandNormal(new MCvScalar(400), new MCvScalar(100));


            Matrix<float> trainClasses1 = trainClasses.GetRows(0, trainSampleCount/3, 1); //as above
            //Set the element of the Array to value
            trainClasses1.SetValue(1); //1 : The value to be set for each element of the Array

            Matrix<float> trainClasses2 = trainClasses.GetRows(trainSampleCount/3, trainSampleCount*2/3, 1);

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
                        else if( response < 2.5)
                            img[i, j] = new Bgr(0, 90, 0);
                        else
                            img[i, j] = new Bgr(0, 0, 90);
                        //img[i, j] = response < 1.5 ? new Bgr(90, 0, 0) : new Bgr(0, 90, 0);
                    }
                }
            }

            // display the original training samples
            for (int i = 0; i < (trainSampleCount /3); i++)
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


        //With two classes
        public void runANN()
        {
            int trainSampleCount = 100;

            #region Generate the traning data and classes
            Matrix<float> trainData = new Matrix<float>(trainSampleCount, 2);
            Matrix<float> trainClasses = new Matrix<float>(trainSampleCount, 1);

            //This is the output image (showing visually the result of the neural network computation)
            Image<Bgr, Byte> img = new Image<Bgr, byte>(500, 500);

            Matrix<float> sample = new Matrix<float>(1, 2);
            Matrix<float> prediction = new Matrix<float>(1, 1);

            //Training data1 is half of training set
            int temp = trainSampleCount >> 1;
            Matrix<float> trainData1 = trainData.GetRows(0, trainSampleCount >> 1, 1);  //trainSampleCount >> 1 = trainSampleCount/2

            //Inplace fills Array with normally distributed random numbers
            trainData1.SetRandNormal(new MCvScalar(200), new MCvScalar(50));

            //Return the matrix corresponding to a specified row span of the input array
            Matrix<float> trainData2 = trainData.GetRows(trainSampleCount >> 1, trainSampleCount, 1);

            //Inplace fills Array with normally distributed random numbers
            trainData2.SetRandNormal(new MCvScalar(300), new MCvScalar(50));

            Matrix<float> trainClasses1 = trainClasses.GetRows(0, trainSampleCount >> 1, 1); //as above
            //Set the element of the Array to value
            trainClasses1.SetValue(1); //1 : The value to be set for each element of the Array

            Matrix<float> trainClasses2 = trainClasses.GetRows(trainSampleCount >> 1, trainSampleCount, 1);

            trainClasses2.SetValue(2);
            #endregion

            Matrix<int> layerSize = new Matrix<int>(new int[] { 2, 5, 1 }); //Number of hidden nodez

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
                                             //The prediction results, should have the same # of rows as the samples
                        network.Predict(sample, prediction); //Saves the prediction to (prediction)

                        // estimates the response and get the neighbors' labels
                        //In case of classification the method returns (response) the class label, in case of regression - the output function value
                        float response = prediction.Data[0, 0];
                        
                        // highlight the pixel depending on the accuracy (or confidence)
                        img[i, j] = response < 1.5 ? new Bgr(90, 0, 0) : new Bgr(0, 90, 0);
                    }
                }
            }

            // display the original training samples
            for (int i = 0; i < (trainSampleCount >> 1); i++)
            {
                //Class 2
                PointF p1 = new PointF(trainData1[i, 0], trainData1[i, 1]);
                img.Draw(new CircleF(p1, 2), new Bgr(255, 100, 100), -1);
                //Class 2
                PointF p2 = new PointF((int)trainData2[i, 0], (int)trainData2[i, 1]);
                img.Draw(new CircleF(p2, 2), new Bgr(100, 255, 100), -1);
            }
            Emgu.CV.UI.ImageViewer.Show(img);
        }

        public Bitmap [] getImagePyramid(Bitmap input_image, int level)
        {
            Image<Bgr, byte> color = new Image<Bgr, byte>(input_image);
            Image<Bgr, byte>[] pyramid = color.BuildPyramid(level);
            Bitmap [] results = new Bitmap[level];
            for (int i = 0; i < level; i++)
                results[i] = pyramid[i].ToBitmap();
            return results;
        }

 

        public void IdentifyContours(Bitmap colorImage, int thresholdValue, bool invert, out Bitmap processedGray, out Bitmap processedColor)
        {

            //1. Convert the image to grayscale.

            #region Conversion To grayscale
            Image<Gray, byte> grayImage = new Image<Gray, byte>(colorImage);
            Image<Bgr, byte> color = new Image<Bgr, byte>(colorImage);

            #endregion

            //2. Threshold it.

            #region  Image normalization and inversion (if required)
            grayImage = grayImage.ThresholdBinary(new Gray(thresholdValue), new Gray(255));

            if (invert)
            {
                grayImage._Not();
            }
            #endregion

            //3. Extract the contours.

            #region Extracting the Contours
            using (MemStorage storage = new MemStorage())
            {

                for (Contour<Point> contours = grayImage.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_TREE, storage); contours != null; contours = contours.HNext)
                {

                    Contour<Point> currentContour = contours.ApproxPoly(contours.Perimeter * 0.015, storage);
                    if (currentContour.BoundingRectangle.Width > 20)
                    {
                        CvInvoke.cvDrawContours(color, contours, new MCvScalar(255), new MCvScalar(255), -1, 2, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, new Point(0, 0));
                        color.Draw(currentContour.BoundingRectangle, new Bgr(0, 255, 0), 1);
                        //To crop the image around the Region of Interest
                        //color.ROI = currentContour.BoundingRectangle;
                    }
                    
                }

            }
            #endregion

            //4. Setting the results to the output variables.

            #region Asigning output
            processedColor = color.ToBitmap();
            processedGray = grayImage.ToBitmap();
            #endregion
        }
    }
}
