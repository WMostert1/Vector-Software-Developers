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
        public Matrix<byte> floatToByteMatrix(Matrix<float> matrix)
        {
            Matrix<byte> result = new Matrix<byte>(matrix.Width, matrix.Height);
            for (int x = 0; x < matrix.Width; x++)
                for (int y = 0; y < matrix.Height; y++)
                    result[x, y] = (byte)matrix[x, y];

            return result;
        }

        public Matrix<float> byteToFloatMatrix(Matrix<byte> matrix)
        {
            Matrix<float> result = new Matrix<float>(matrix.Width, matrix.Height);
            for (int x = 0; x < matrix.Width; x++)
                for (int y = 0; y < matrix.Height; y++)
                    result[x, y] = (float)matrix[x, y];

            return result;
        }
        

        public void analyseConfusionMatrix(Matrix<int> matrix)
        {
            if(matrix.Height != matrix.Height)
                throw new Exception("Not a square matrix");

            int total = 0;
            for (int x = 0; x < matrix.Width; x++)
            {
                for (int y = 0; y < matrix.Height; y++)
                {
                    total += matrix[x, y];
                    Console.Write(matrix[x, y]+" ");
                }
                Console.WriteLine("");
            }

            int correct = 0;
            for (int i = 0; i < matrix.Height; i++)
                correct += matrix[i, i];

            
            Console.WriteLine("Accuracy : "+correct/(double)total*100.0+"%");
           // Console.ReadLine();
        }

        public Image<Bgr, byte> preProcessImage(Image<Bgr, byte> image)
        {
            while (image.Width > 1000 || image.Height > 1000)
                image = image.Resize(0.5, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            return doGrabCut(image);
        }

        public void preProcessTrainingData()
        {
                restructureTrainingData("C:\\Users\\Aeolus\\Pictures\\SAMBUG\\ANN\\Training");
                FileInfo[] files = new DirectoryInfo("C:\\Users\\Aeolus\\Pictures\\SAMBUG\\ANN\\Training").GetFiles();
                foreach (FileInfo file in files)
                {
                    if (file.Extension.Equals(".db")) continue;
                    Image<Bgr, byte> image = new Image<Bgr, byte>(file.FullName);
                    while (image.Width > 1000 || image.Height > 1000)
                        image = image.Resize(0.5, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    doGrabCut(image).Save(file.FullName);
                }
        }

        protected int restructureTrainingData(string root_folder) //returns number of classes as int
        {
            
            DirectoryInfo root = new DirectoryInfo(root_folder);
            DirectoryInfo[] directories_of_classes = root.GetDirectories();
          
            foreach (var directory in directories_of_classes)
            {
                int number = 0;
                foreach (var file in directory.GetFiles())
                {
                    string path = root_folder + "\\" + Path.GetFileName(directory.FullName) + "--" + (number++) + file.Extension;
                    if(!File.Exists(path))
                        file.CopyTo(path);
                }
            }
            return directories_of_classes.Length;
        }

        protected int computeTrainingAndTestingMatrices(String path, out Matrix<float> training_data,out Matrix<float> training_classifications,out Matrix<float> testing_data,out Matrix<float> testing_classifications)
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
                    string class_label = fileName.Substring(0,fileName.IndexOf("--"));
                    if(!class_labels_unique.Contains(class_label))
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
                foreach(var class_label in class_labels_testing)
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
                                flattened.Data[0, c++] = img.Data[i, j, 0]/255F;
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

        protected Image<Bgr, byte> doGrabCut(Image<Bgr, byte> image)
        {

            //1. Convert the image to grayscale.

            int numberOfIterations = 4;
            Image<Gray, byte> grayImage = image.Convert<Gray, Byte>();

            //2. Threshold it using otsu.


            grayImage = getThresholdedImage(grayImage);
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

        protected Image<Gray, byte> getThresholdedImage(Image<Gray, byte> Img_Org_Gray)
        {
            bool useDapative = false;
            if (useDapative)
            {
                Image<Gray, Byte> dst = new Image<Gray, byte>(new Size(Img_Org_Gray.Width, Img_Org_Gray.Height));

                System.IntPtr srcPtr = Img_Org_Gray.Ptr;
                System.IntPtr dstPtr = dst.Ptr;

                CvInvoke.cvAdaptiveThreshold(srcPtr, dstPtr, 255, Emgu.CV.CvEnum.ADAPTIVE_THRESHOLD_TYPE.CV_ADAPTIVE_THRESH_MEAN_C, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY_INV, 3, 5);
                return dst;
            }
            else
            {
                Image<Gray, byte> Img_Source_Gray = Img_Org_Gray.Copy();
                Image<Gray, byte> Img_Otsu_Gray = Img_Org_Gray.CopyBlank();

                CvInvoke.cvThreshold(Img_Source_Gray.Ptr, Img_Otsu_Gray.Ptr, 0, 255, Emgu.CV.CvEnum.THRESH.CV_THRESH_OTSU | Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY);

                return Img_Otsu_Gray;
            }

            
        }

        protected Matrix<float> convertBinaryDescriptorsToBase10Float(Matrix<byte> desc) //TODO: Fix overflow problem
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

        
        protected Matrix<float> ConcatDescriptors(IList<Matrix<float>> descriptors)
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

       
        protected IList<Matrix<float>> ComputeMultipleDescriptors(string[] fileNames, out IList<IndecesMapping> imap)
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

        protected Matrix<float> ComputeSingleDescriptors(string fileName)
        {
            Matrix<float> descs;

            using (Image<Gray, Byte> img = new Image<Gray, byte>(fileName))
            {
               
                descs = getSURFFeatureDescriptorMatrix(img,300,true);
            }

            return descs;
        } 

        protected Matrix<byte> getORBDescriptors(Image<Gray, byte> image)
        {
            const int NumberOfFeatures = 256; 
            ORBDetector orbCPU = new ORBDetector(NumberOfFeatures);
            VectorOfKeyPoint modelKeyPoints;

            modelKeyPoints = orbCPU.DetectKeyPointsRaw(image, null);
            //var features = orbCPU.DetectFeatures(image, null);
            return orbCPU.ComputeDescriptorsRaw(image, null, modelKeyPoints);
        }


        protected Matrix<float> getSURFFeatureDescriptorMatrix(Image<Gray, byte> modelImage,int hessian, bool extended)
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
       
    }
}
