package vsd.co.za.sambugapp.BugIntelligence;

import android.content.Context;
import android.content.res.Resources;
import android.graphics.Bitmap;
import android.os.Environment;
import android.util.Log;

import com.google.gson.Gson;
import com.thoughtworks.xstream.XStream;

import org.opencv.android.Utils;
import org.opencv.core.Core;
import org.opencv.core.CvType;
import org.opencv.core.Mat;
import org.opencv.core.Point;
import org.opencv.core.Rect;
import org.opencv.core.Scalar;
import org.opencv.features2d.DescriptorMatcher;
import org.opencv.features2d.FeatureDetector;
import org.opencv.imgproc.Imgproc;
import org.opencv.ml.CvANN_MLP;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.lang.reflect.Array;
import java.util.ArrayList;

import vsd.co.za.sambugapp.R;

import static org.opencv.core.Core.bitwise_not;

/**
 * Created by Aeolus on 2015-10-26.
 * Does classification of stink bugs
 */
public class ANNClassifier {
    static{ System.loadLibrary("opencv_java"); }

    private class ANNConfig
    {
        public ANNConfig(int[] _layers, double _alpha, double _beta)
        {
            layers = _layers;
            alpha = _alpha;
            beta = _beta;
        }

        public int[] layers;
        public double alpha;
        public double beta;
    }

    private CvANN_MLP network;
    private ArrayList<String> classes;
    private Context _context;
    private final int DIMENSIONS = 64;
    private static ANNClassifier mInstance;

    private ArrayList<String> restoreClasses(){
        Resources res = _context.getResources();
        InputStream in_s = res.openRawResource(R.raw.classes);
        BufferedReader reader = new BufferedReader(new InputStreamReader(in_s));
        ArrayList<String> classes = new ArrayList<>();
        try {
            String line = reader.readLine();
            while (line != null) {
                classes.add(line);
            }
        }catch(IOException e){
            e.printStackTrace();
        }
        return classes;
    }

    private ANNConfig restoreNetworkConfig(){
        final Gson gson = new Gson();
        Resources res = _context.getResources();
        InputStream in_s = res.openRawResource(R.raw.config);
        BufferedReader reader = new BufferedReader(new InputStreamReader(in_s));
        String json = "";

        try {
            String line = reader.readLine();
            while (line != null){
                json += line;
            }
        }catch(IOException e){
            e.printStackTrace();
        }

        return gson.fromJson(json, ANNConfig.class);
    }

    private static XStream getXstreamObject() {
         return new XStream(); // DomDriver and StaxDriver instances also can be used with constructor
    }

    private Mat restoreDictionary(){
        XStream xStream = getXstreamObject();
        Resources res = _context.getResources();
        InputStream in_s = res.openRawResource(R.raw.dictionary);
        BufferedReader reader = new BufferedReader(new InputStreamReader(in_s));
        String xml = "";

        try {
            String line = reader.readLine();
            while (line != null){
                xml += line;
            }
        }catch(IOException e){
            e.printStackTrace();
        }
        Mat dictionary = (Mat)xStream.fromXML(xml);

        return dictionary;
    }

    private CvANN_MLP restoreNetwork(){
        ANNConfig config = restoreNetworkConfig();
        Mat layerSizes = new Mat();

        for(int i = 0; i < config.layers.length ; i++)
            layerSizes.put(i,0,config.layers[i]);

        CvANN_MLP net = new CvANN_MLP(layerSizes, CvANN_MLP.SIGMOID_SYM,config.alpha,config.beta);

        String networkWeightsFileName = Environment.getExternalStorageDirectory() + "/network.stat";

        Resources res = _context.getResources();
        InputStream in_s = res.openRawResource(R.raw.network);
        BufferedReader reader = new BufferedReader(new InputStreamReader(in_s));
        String data = "";

        try {
            String line = reader.readLine();
            while (line != null) {
                data += line;
            }

            BufferedWriter output;
            File file = new File(networkWeightsFileName);
            output = new BufferedWriter(new FileWriter(file));
            output.write(data);
        }catch(IOException e){
            e.printStackTrace();
        }

        net.load(networkWeightsFileName);

        return net;
    }

    private ANNClassifier(Context context){
        _context = context;
        network = restoreNetwork();
        classes = restoreClasses();

    }

    public static synchronized ANNClassifier getInstance(Context context) {
        if (mInstance == null) {
            mInstance = new ANNClassifier(context);
        }
        return mInstance;
    }

    public String classify(Bitmap bitmap){
        Mat classification_result = new Mat();
        FeatureDetector detector = FeatureDetector.create(FeatureDetector.SURF);
        DescriptorMatcher matcher = DescriptorMatcher.create(DescriptorMatcher.BRUTEFORCE_L1);

        


        return null;
    }

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
