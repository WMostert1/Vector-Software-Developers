using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;

namespace BugBusiness.BugIntelligence
{
    class ImageProcessing
    {
        public Bitmap[] getImagePyramid(Bitmap input_image, int level)
        {
            Image<Bgr, byte> color = new Image<Bgr, byte>(input_image);
            Image<Bgr, byte>[] pyramid = color.BuildPyramid(level);
            Bitmap[] results = new Bitmap[level];
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
                        color.ROI = currentContour.BoundingRectangle;
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
