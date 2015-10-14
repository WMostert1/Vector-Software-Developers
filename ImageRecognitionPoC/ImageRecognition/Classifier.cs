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
        public void saveDescriptorsToFile(Matrix<float> descriptors)
        {

        }

        public Matrix<byte> floatToByteMatrix(Matrix<float> matrix)
        {
            Matrix<byte> result = new Matrix<byte>(matrix.Width, matrix.Height);
            for (int x = 0; x < matrix.Rows; x++)
                for (int y = 0; y < matrix.Cols; y++)
                    result[x, y] = (byte)matrix[x, y];

            return result;
        }

        public Matrix<float> byteToFloatMatrix(Matrix<byte> matrix)
        {
            Matrix<float> result = new Matrix<float>(matrix.Width, matrix.Height);
            for (int x = 0; x < matrix.Rows; x++)
                for (int y = 0; y < matrix.Cols; y++)
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
                Console.WriteLine("Processing images...");
                int file_count = 0;
                foreach (FileInfo file in files)
                {
                    
                    Console.WriteLine((int)((file_count++)/(double)file.Length*100.0)+"%");
                    if (file.Extension.Equals(".db")) continue;
                    Image<Bgr, byte> image = new Image<Bgr, byte>(file.FullName);
                    while (image.Width > 1000 || image.Height > 1000)
                        image = image.Resize(0.5, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                    image = doGrabCut(image);
                    image.Save(file.FullName);

                    string fileName = Path.GetFileName(file.FullName);
                    fileName = fileName.Substring(0, fileName.IndexOf("."));
                    int x = 0, y = 0;
                    for (double Y_Scale = 1.2; Y_Scale <= 2.0; Y_Scale += 0.2)
                    {
                        image.Resize(image.Width, (int)(image.Height * Y_Scale), INTER.CV_INTER_CUBIC, false).Resize(256, 256, INTER.CV_INTER_CUBIC, true).Save(file.DirectoryName + "\\" + fileName + "Y" + (y++) + file.Extension);
                    }                 
                        
                    for (double X_Scale = 1.2; X_Scale <= 2.0; X_Scale += 0.2 ){
                  
                        image.Resize((int)(image.Width * X_Scale), image.Height, INTER.CV_INTER_CUBIC, false).Resize(256,256,INTER.CV_INTER_CUBIC,true).Save(file.DirectoryName + "\\" + fileName+"X"+(x++) + file.Extension);
                    }

                    int g = 0;
                    for (double gamma = 0.4; gamma <= 1.8; gamma += 0.2)
                    {
                        Image<Bgr, byte> input_image = image.Copy();
                        input_image._GammaCorrect(gamma);
                        doGrabCut(input_image).Save(file.DirectoryName + "\\" + fileName + "G" + (g++) + file.Extension); ;
                    }
                        
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

        

        protected Image<Bgr, byte> doGrabCut(Image<Bgr, byte> image)
        {

            //1. Convert the image to grayscale.

            int numberOfIterations = 5;
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
            for (int x = 0; x < result.Rows; x++)
                for (int y = 0; y < result.Cols; y++)
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
