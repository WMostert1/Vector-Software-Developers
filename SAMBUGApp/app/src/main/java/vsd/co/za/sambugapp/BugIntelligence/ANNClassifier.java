package vsd.co.za.sambugapp.BugIntelligence;

import android.content.Context;
import android.graphics.Bitmap;
import android.os.Environment;
import android.util.Log;

import org.opencv.android.Utils;
import org.opencv.core.Core;
import org.opencv.core.CvType;
import org.opencv.core.Mat;
import org.opencv.core.Point;
import org.opencv.core.Rect;
import org.opencv.core.Scalar;
import org.opencv.imgproc.Imgproc;

import static org.opencv.core.Core.bitwise_not;

/**
 * Created by Aeolus on 2015-10-26.
 */
public class ANNClassifier {
    static{ System.loadLibrary("opencv_java"); }

    public Mat getThresholdedImage(Mat source){
        Mat destination = new Mat(source.rows(),source.cols(),source.type());

        destination = source;
        Imgproc.threshold(source,destination,0,255,Imgproc.THRESH_OTSU);

        return destination;
    }


    public Bitmap doGrabCut(Bitmap bitmap){
        int g_cut_iterations = 3;

        bitmap = bitmap.copy(Bitmap.Config.ARGB_8888, true);

        Mat grayImage = new Mat();

        //GrabCut part
        Mat img = new Mat();
        Utils.bitmapToMat(bitmap, img);

        Imgproc.cvtColor(img, grayImage, Imgproc.COLOR_RGB2GRAY);

        grayImage = getThresholdedImage(grayImage);
        bitwise_not(grayImage, grayImage);

        int r = img.rows();
        int c = img.cols();

        Point p1 = new Point(c/14, r/14);
        Point p2 = new Point(c-c/14, r-r/14);
        Rect rect = new Rect(p1,p2);


        Mat mask = new Mat();
        Mat fgdModel = new Mat();
        Mat bgdModel = new Mat();

        Mat imgC3 = new Mat();
        Imgproc.cvtColor(img, imgC3, Imgproc.COLOR_RGBA2RGB);

        Imgproc.grabCut(imgC3, mask, rect, bgdModel, fgdModel, g_cut_iterations, Imgproc.GC_INIT_WITH_RECT);

        Imgproc.threshold(mask,mask,2,255,Imgproc.THRESH_BINARY);
        Mat result = new Mat();

        imgC3.copyTo(result, mask);
        //convert to Bitmap


        Utils.matToBitmap(result, bitmap);

        img.release();
        imgC3.release();
        mask.release();
        fgdModel.release();
        bgdModel.release();
        return bitmap;
    }


}
