package vsd.co.za.sambugapp.CameraProcessing;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.IntBuffer;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.ImageFormat;
import android.graphics.PixelFormat;
import android.hardware.Camera;
import android.hardware.Camera.PictureCallback;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.media.ExifInterface;
import android.os.Bundle;
import android.os.Environment;
import android.util.Log;
import android.view.Display;
import android.view.View;
import android.view.WindowManager;
import android.view.animation.Animation;
import android.view.animation.RotateAnimation;
import android.widget.Button;
import android.widget.FrameLayout;
import android.widget.ImageButton;
import android.widget.RelativeLayout;
import android.widget.Toast;

import vsd.co.za.sambugapp.R;

public class CustomCamera extends Activity implements SensorEventListener {
    private android.hardware.Camera mCamera;
    public static final String CAMERA="za.co.vsd.camera";
    private CameraPreview mPreview;
    private SensorManager sensorManager = null;
    private int orientation;
    private ExifInterface exif;
    private int deviceHeight;
    private int deviceWidth;
    private FrameLayout flBtnContainer;
    private String fileName;
    private ImageButton rotatingImage;
    private int degrees = -1;
    private String fullPathName;
    int width,height;
    boolean cameraConfigured;
    private boolean pictureTaken;
    public static final double SQUARERATIO =0.75;


    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.test);

        // Getting all the needed elements from the layout
        rotatingImage = (ImageButton) findViewById(R.id.imgbCamera);
        flBtnContainer = (FrameLayout) findViewById(R.id.flBtnContainer);

        // Getting the sensor service.
        sensorManager = (SensorManager) getSystemService(SENSOR_SERVICE);

        // Selecting the resolution of the Android device so we can create a
        // proportional preview
        Display display = ((WindowManager) getSystemService(Context.WINDOW_SERVICE)).getDefaultDisplay();
        deviceHeight = display.getHeight();
        deviceWidth = display.getWidth();

        // Add a listener to the Capture button
        rotatingImage.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                if (!pictureTaken) {
                    pictureTaken=true;
                    mCamera.takePicture(null, null, mPicture);
                }
            }
        });
        cameraConfigured = false;
        pictureTaken = false;
    }

    /**
     * Creates the camera and adjusts parameters.
     */
    private void createCamera() {

        mCamera = getCameraInstance();
        if (mCamera == null) {
            Log.e("BLAH", "NULL CAMERA");
        }
        //Configuring the camera
      //  if (!cameraConfigured) {
            Camera.Parameters parameters=mCamera.getParameters();
            Camera.Size size=getBestPreviewSize(deviceWidth, deviceHeight, parameters);
            Camera.Size pictureSize=getBestPictureSize(parameters);


            if (size != null && pictureSize != null) {
                width = pictureSize.width;
                height = pictureSize.height;
                Camera.Size size2 = getOptimalSize(parameters.getSupportedPreviewSizes(),deviceWidth,deviceHeight);
                parameters.setPreviewSize(size2.width, size2.height);
                parameters.setPictureSize(pictureSize.width,
                        pictureSize.height);
                parameters.setPictureFormat(ImageFormat.JPEG);
                parameters.setPictureFormat(PixelFormat.JPEG);
                parameters.setJpegQuality(100);
                if (parameters.getSupportedFocusModes().contains(Camera.Parameters.FOCUS_MODE_CONTINUOUS_PICTURE))
                    parameters.setFocusMode(Camera.Parameters.FOCUS_MODE_CONTINUOUS_PICTURE);
                mCamera.setParameters(parameters);

                cameraConfigured=true;
            }
       // }



        // Create our Preview view and set it as the content of our activity.
        mPreview = new CameraPreview(this, mCamera);
        FrameLayout preview = (FrameLayout) findViewById(R.id.camera_preview);

        //Creating the view param to display the preview
        RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(deviceWidth, deviceHeight);
        preview.setLayoutParams(layoutParams);

        // Adding the camera preview after the FrameLayout and before the button
        // as a separated element.
        preview.addView(mPreview, 0);
    }

    /**
     * Gets the best preview size for the camera.
     * @param width
     * @param height
     * @param parameters
     * @return
     */
    private Camera.Size getBestPreviewSize(int width, int height,
                                           Camera.Parameters parameters) {
        Camera.Size result=null;


        for (Camera.Size size : parameters.getSupportedPreviewSizes()) {
            if (size.width <= width && size.height <= height) {
                if (result == null) {
                    result=size;
                }
                else {
                    int resultArea=result.width * result.height;
                    int newArea=size.width * size.height;

                    if ((newArea > resultArea) ) {
                        result=size;
                    }
                }
            }
        }

        return(result);
    }

    /**
     * Sets the image quality.
     * @param parameters
     * @return
     */
    private Camera.Size getBestPictureSize(Camera.Parameters parameters) {
        Camera.Size result=null;
        //Best Picture Size Below the Specified MaxWidth and MaxLength
        int maxWidth = 1000;
        int maxHeight = 1000;
        int maxArea = maxHeight * maxWidth;

        for (Camera.Size size : parameters.getSupportedPictureSizes()) {
            if (result == null) {
                result=size;
            }
            else {
                int resultArea=result.width * result.height;
                int newArea=size.width * size.height;

                if ((newArea > resultArea) && (newArea <= maxArea)) {
                    result=size;
                }
            }
        }

        return result;
    }

    @Override
    protected void onResume() {
        super.onResume();
        createCamera();

        // Register this class as a listener for the accelerometer sensor
        sensorManager.registerListener(this, sensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER), SensorManager.SENSOR_DELAY_NORMAL);
    }

    @Override
    protected void onPause() {
        super.onPause();
        // release the camera immediately on pause event
        releaseCamera();

        // removing the inserted view - so when we come back to the app we
        // won't have the views on top of each other.
        FrameLayout preview = (FrameLayout) findViewById(R.id.camera_preview);
        preview.removeViewAt(0);
    }

    private void releaseCamera() {
        if (mCamera != null) {
            mCamera.release(); // release the camera for other applications
            mCamera = null;
        }
    }

    /**
     * Check if User has a camera
     * @param context
     * @return
     */
    private boolean checkCameraHardware(Context context) {
        if (context.getPackageManager().hasSystemFeature(PackageManager.FEATURE_CAMERA)) {
            // this device has a camera
            return true;
        } else {
            // no camera on this device
            return false;
        }
    }

    /**
     * Check if User has SD card
     * @return
     */
    private boolean checkSDCard() {
        boolean state = false;

        String sd = Environment.getExternalStorageState();
        if (Environment.MEDIA_MOUNTED.equals(sd)) {
            state = true;
        }

        return state;
    }

    /**
     * A safe way to get an instance of the CustomCamera object.
     */
    public static android.hardware.Camera getCameraInstance() {
        android.hardware.Camera c = null;
        try {
            // attempt to get a CustomCamera instance
            c = android.hardware.Camera.open();
        } catch (Exception e) {
            e.printStackTrace();
            // CustomCamera is not available (in use or does not exist)
        }

        // returns null if camera is unavailable
        return c;
    }

    /**
     * When we have taken an image.
     */
    private PictureCallback mPicture = new PictureCallback() {

        public void onPictureTaken(byte[] data, android.hardware.Camera camera) {

            byte[] croppedData = null;
            try {
                data = getBitmap(data);
            }
            catch(IOException e){
                e.printStackTrace();
            }

            //Saving the image in a Folder called Sambug - specified in getDir()
            File pictureFileDir = getDir();
            if (!pictureFileDir.exists() && !pictureFileDir.mkdirs()) {

                Log.e("Here", "Can't create directory to save image.");
                Toast.makeText(getApplicationContext(), "Can't create directory to save image.",
                        Toast.LENGTH_LONG).show();
                return;

            }

            SimpleDateFormat dateFormat = new SimpleDateFormat("yyyymmddhhmmss");
            String date = dateFormat.format(new Date());
            String photoFile = "Picture_" + date + ".jpg";
            String filename = pictureFileDir.getPath() + File.separator + photoFile;

            File pictureFile = new File(filename);


            try {
                FileOutputStream purge = new FileOutputStream(pictureFile);
                purge.write(data);
                purge.close();
            } catch (FileNotFoundException e) {
                Log.d("DG_DEBUG", "File not found: " + e.getMessage());
            } catch (IOException e) {
                Log.d("DG_DEBUG", "Error accessing file: " + e.getMessage());
            }

            // Adding Exif data for the orientation.
            fullPathName = pictureFile.getAbsolutePath();
            try {
                exif = new ExifInterface(pictureFile.getAbsolutePath());
                exif.setAttribute(ExifInterface.TAG_ORIENTATION, "" + orientation);
                exif.saveAttributes();
            } catch (IOException e) {
                e.printStackTrace();
            }
            //Sending fileName to ImagePreview Class
            sendToCameraPreview(fullPathName);
        }
    };

    /**
     * Getting the directory where we can store the image.
     * @return
     */
    private File getDir() {
        File sdDir = Environment
                .getExternalStoragePublicDirectory(Environment.DIRECTORY_PICTURES);
        return new File(sdDir, "Sambug");
    }

    /**
     * Cropping the image accordingly
     * @param data - byte[] containing the image.
     * @return
     * @throws IOException
     */
    public byte[] getBitmap(byte[] data)
            throws IOException {

        //Square length is set to smallest of width/height *0.75 (squareRatio)

        int minX,maxX,minY,maxY;
        int padding;
        int Xcentre = (int)(width /2);
        int Ycentre = (int)(height /2);
        if(width > height){
            padding = (int)(SQUARERATIO * Ycentre);
        }
        else padding = (int)(SQUARERATIO * Xcentre);


        minX = Xcentre - padding;
        minY = Ycentre - padding;
        maxX =  Xcentre + padding;
        maxY = Ycentre + padding;

        int squareLength = 2*padding;
        int[] pixels = new int[squareLength*squareLength];
        Bitmap bitmap = BitmapFactory.decodeByteArray(data , 0, data.length);



        bitmap.getPixels(pixels, 0, squareLength,minX,  minY,maxX-minX, maxY-minY);
        bitmap = Bitmap.createBitmap(pixels, 0, squareLength,2*padding, 2*padding, Bitmap.Config.ARGB_8888);//ARGB_8888 is a good quality configuration


        ByteArrayOutputStream bos = new ByteArrayOutputStream();
        bitmap.compress(Bitmap.CompressFormat.JPEG, 100, bos);//100 is the best quality possible
        bitmap.recycle();
        byte[] square = bos.toByteArray();
        return square;
    }

    /**
     * Sending the image file name to ImagePreview
     * @param p
     */
    private void sendToCameraPreview(String p){
        Intent intent=new Intent(this,ImagePreview.class);
        Bundle b = new Bundle();
        b.putSerializable(CAMERA, p);
        intent.putExtras(b);
        startActivityForResult(intent,0);
    }
    /**
     * Putting in place a listener so we can get the sensor data only when
     * something changes.
     */
    public void onSensorChanged(SensorEvent event) {
        int oofset = 90;
        synchronized (this) {
            if (event.sensor.getType() == Sensor.TYPE_ACCELEROMETER) {
                RotateAnimation animation = null;
                if (event.values[0] < 4 && event.values[0] > -4) {
                    if (event.values[1] > 0 && orientation != ExifInterface.ORIENTATION_ROTATE_90) {
                        // UP
                        orientation = ExifInterface.ORIENTATION_ROTATE_90;
                        animation = getRotateAnimation(270+oofset);
                        degrees = 270+oofset;
                    } else if (event.values[1] < 0 && orientation != ExifInterface.ORIENTATION_ROTATE_270) {
                        // UP SIDE DOWN
                        orientation = ExifInterface.ORIENTATION_ROTATE_270;
                        animation = getRotateAnimation(90+oofset);
                        degrees = 90+oofset;
                    }
                } else if (event.values[1] < 4 && event.values[1] > -4) {
                    if (event.values[0] > 0 && orientation != ExifInterface.ORIENTATION_NORMAL) {
                        // LEFT
                        orientation = ExifInterface.ORIENTATION_NORMAL;
                        animation = getRotateAnimation(0+oofset);
                        degrees = 0+oofset;
                    } else if (event.values[0] < 0 && orientation != ExifInterface.ORIENTATION_ROTATE_180) {
                        // RIGHT
                        orientation = ExifInterface.ORIENTATION_ROTATE_180;
                        animation = getRotateAnimation(180+oofset);
                        degrees = 180+oofset;
                    }
                }
                if (animation != null) {
                    rotatingImage.startAnimation(animation);
                }
            }

        }
    }

    /**
     * Calculating the degrees needed to rotate the image imposed on the button
     * so it is always facing the user in the right direction
     *
     * @param toDegrees
     * @return
     */
    private RotateAnimation getRotateAnimation(float toDegrees) {
        float compensation = 0;

        if (Math.abs(degrees - toDegrees) > 180) {
            compensation = 360;
        }

        // When the device is being held on the left side (default position for
        // a camera) we need to add, not subtract from the toDegrees.
        if (toDegrees == 0) {
            compensation = -compensation;
        }

        // Creating the animation and the RELATIVE_TO_SELF means that he image
        // will rotate on it center instead of a corner.
        RotateAnimation animation = new RotateAnimation(degrees, toDegrees - compensation, Animation.RELATIVE_TO_SELF, 0.5f, Animation.RELATIVE_TO_SELF, 0.5f);

        // Adding the time needed to rotate the image
        animation.setDuration(250);

        // Set the animation to stop after reaching the desired position. With
        // out this it would return to the original state.
        animation.setFillAfter(true);

        return animation;
    }

    /**
     * STUFF THAT WE DON'T NEED BUT MUST BE HEAR FOR THE COMPILER TO BE HAPPY.
     */
    public void onAccuracyChanged(Sensor sensor, int accuracy) {
    }


    private Camera.Size getOptimalSize(List<Camera.Size> sizes, int w, int h) {

        final double ASPECT_TOLERANCE = 0.2;
        double targetRatio = (double) w / h;
        if (sizes == null)
            return null;
        Camera.Size optimalSize = null;
        double minDiff = Double.MAX_VALUE;
        int targetHeight = h;
        // Try to find an size match aspect ratio and size
        for (Camera.Size size : sizes)
        {
//          Log.d("CameraActivity", "Checking size " + size.width + "w " + size.height + "h");
            double ratio = (double) size.width / size.height;
            if (Math.abs(ratio - targetRatio) > ASPECT_TOLERANCE)
                continue;
            if (Math.abs(size.height - targetHeight) < minDiff)
            {
                optimalSize = size;
                minDiff = Math.abs(size.height - targetHeight);
            }
        }
        // Cannot find the one match the aspect ratio, ignore the requirement

        if (optimalSize == null)
        {
            minDiff = Double.MAX_VALUE;
            for (Camera.Size size : sizes) {
                if (Math.abs(size.height - targetHeight) < minDiff)
                {
                    optimalSize = size;
                    minDiff = Math.abs(size.height - targetHeight);
                }
            }
        }
        return optimalSize;
    }
}