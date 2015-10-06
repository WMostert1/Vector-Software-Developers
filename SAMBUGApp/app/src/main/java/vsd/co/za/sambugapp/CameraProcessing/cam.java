package vsd.co.za.sambugapp.CameraProcessing;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Matrix;
import android.graphics.PixelFormat;
import android.graphics.Rect;
import android.graphics.RectF;
import android.graphics.YuvImage;
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
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;

import vsd.co.za.sambugapp.R;

public class cam extends Activity implements SensorEventListener {
    private Camera mCamera;
    public static final String CAMERA="za.co.vsd.camera";
    private CameraPreview2 mPreview;
    private SensorManager sensorManager = null;
    private int orientation;
    private ExifInterface exif;
    private int deviceHeight;
    private int deviceWidth;
    private Button ibRetake;
    private Button ibUse;
    private Button ibCapture;
    private FrameLayout flBtnContainer;
    private File sdRoot;
    private String dir;
    private String fileName;
    private ImageButton rotatingImage;
    private int degrees = -1;
    private String fullPathName;


    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
       // setContentView(R.layout.activity_cam);
        setContentView(R.layout.test);

        // Setting all the path for the image
        sdRoot = Environment.getExternalStorageDirectory();
        dir = "/DCIM/Camera/";

        // Getting all the needed elements from the layout
        rotatingImage = (ImageButton) findViewById(R.id.imgbCamera);
        //ibRetake = (Button) findViewById(R.id.ibRetake);
        //ibUse = (Button) findViewById(R.id.ibUse);
       // ibCapture = (Button) findViewById(R.id.ibCapture2);
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
                mCamera.takePicture(null, null, mPicture);
            }
        });
 //       cameraConfigured = false;
        //getRotateAnimation(-90);
        // Add a listener to the Retake button
//        ibRetake.setOnClickListener(new View.OnClickListener() {
//            public void onClick(View v) {
//                // Deleting the image from the SD card/
//                File discardedPhoto = new File(sdRoot, dir + fileName);
//                discardedPhoto.delete();
//
//                // Restart the camera preview.
//                mCamera.startPreview();
//
//                // Reorganize the buttons on the screen
//                flBtnContainer.setVisibility(LinearLayout.VISIBLE);
//                ibRetake.setVisibility(LinearLayout.GONE);
//                ibUse.setVisibility(LinearLayout.GONE);
//            }
//        });

        // Add a listener to the Use button
//        ibUse.setOnClickListener(new View.OnClickListener() {
//            public void onClick(View v) {
//                // Everything is saved so we can quit the app.
//                finish();
//            }
//        });
    }


    private void createCamera() {
        /////////////////////////////////////////
        // Create an instance of Camera
        mCamera = getCameraInstance();

        // Setting the right parameters in the camera

        Camera.Parameters params = mCamera.getParameters();
     //   params.setPictureSize(360, 600);
        params.setPictureFormat(PixelFormat.JPEG);
        params.setJpegQuality(100);
        mCamera.setParameters(params);

        // Create our Preview view and set it as the content of our activity.
        mPreview = new CameraPreview2(this, mCamera);
        FrameLayout preview = (FrameLayout) findViewById(R.id.camera_preview);

        // Calculating the width of the preview so it is proportional.
       // float widthFloat = (float) (deviceHeight) * 4 / 3;
       // int width = Math.round(widthFloat);

        // Resizing the LinearLayout so we can make a proportional preview. This
        // approach is not 100% perfect because on devices with a really small
        // screen the the image will still be distorted - there is place for
        // improvment.
        RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(deviceWidth, deviceHeight);
        preview.setLayoutParams(layoutParams);

        // Adding the camera preview after the FrameLayout and before the button
        // as a separated element.
        preview.addView(mPreview, 0);
    }

    @Override
    protected void onResume() {
        super.onResume();

        // Test if there is a camera on the device and if the SD card is
        // mounted.
//        if (!checkCameraHardware(this)) {
//            Intent i = new Intent(this, NoCamera.class);
//            startActivity(i);
//            finish();
//        } else if (!checkSDCard()) {
//            Intent i = new Intent(this, NoSDCard.class);
//            startActivity(i);
//            finish();
//        }

        // Creating the camera
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

    /** Check if this device has a camera */
    private boolean checkCameraHardware(Context context) {
        if (context.getPackageManager().hasSystemFeature(PackageManager.FEATURE_CAMERA)) {
            // this device has a camera
            return true;
        } else {
            // no camera on this device
            return false;
        }
    }

    private boolean checkSDCard() {
        boolean state = false;

        String sd = Environment.getExternalStorageState();
        if (Environment.MEDIA_MOUNTED.equals(sd)) {
            state = true;
        }

        return state;
    }

    /**
     * A safe way to get an instance of the Camera object.
     */
    public static Camera getCameraInstance() {
        Camera c = null;
        try {
            // attempt to get a Camera instance
            c = Camera.open();
        } catch (Exception e) {
            // Camera is not available (in use or does not exist)
        }

        // returns null if camera is unavailable
        return c;
    }

    private PictureCallback mPicture = new PictureCallback() {

        public void onPictureTaken(byte[] data, Camera camera) {

            // Replacing the button after a photho was taken.
          //  flBtnContainer.setVisibility(View.GONE);
            //ibRetake.setVisibility(View.VISIBLE);
            //ibUse.setVisibility(View.VISIBLE);
            byte[] croppedData = null;
            try {
                croppedData = getBitmap(data);
            }
            catch(IOException e){

            }
            // File name of the image that we just took.
            fileName = "IMG_" + new SimpleDateFormat("yyyyMMdd_HHmmss").format(new Date()).toString() + ".jpg";

            // Creating the directory where to save the image. Sadly in older
            // version of Android we can not get the Media catalog name
            File mkDir = new File(sdRoot, dir);
            mkDir.mkdirs();

            // Main file where to save the data that we recive from the camera
            File pictureFile = new File(sdRoot, dir + fileName);

            try {
                FileOutputStream purge = new FileOutputStream(pictureFile);
                purge.write(croppedData);
                purge.close();
            } catch (FileNotFoundException e) {
                Log.d("DG_DEBUG", "File not found: " + e.getMessage());
            } catch (IOException e) {
                Log.d("DG_DEBUG", "Error accessing file: " + e.getMessage());
            }

            // Adding Exif data for the orientation. For some strange reason the
            // ExifInterface class takes a string instead of a file.
            //Fixining weird orientation bug
//            if(orientation == 6) {
//                orientation = 1;
//            }
            fullPathName = "/sdcard/" + dir + fileName;
            try {
               // exif = new ExifInterface("/sdcard/" + dir + fileName);
                exif = new ExifInterface(fullPathName);
                exif.setAttribute(ExifInterface.TAG_ORIENTATION, "" + orientation);
                exif.saveAttributes();
            } catch (IOException e) {
                e.printStackTrace();
            }
            sendToCameraPreview(fullPathName);
        }
    };

    public byte[] getBitmap(byte[] data)
            throws IOException {
//
        Camera.Parameters params = mCamera.getParameters();
        int width = params.getPictureSize().width;
        int height = params.getPictureSize().height;


        Matrix matrix = new Matrix();
        matrix.postScale(1f, 1f);
        Bitmap bitmap = BitmapFactory.decodeByteArray(data, 0, data.length);

        Bitmap resizedBitmap = Bitmap.createBitmap(bitmap, width*1/8,  height*1/8,width*7/8-width*1/8, height*7/8-height*1/8, matrix, true);

        bitmap.recycle();
        //Bitmap bitmap = BitmapFactory.decodeFile("/path/images/image.jpg");
        ByteArrayOutputStream blob = new ByteArrayOutputStream();

        resizedBitmap.compress(Bitmap.CompressFormat.PNG, 0 /*ignored for PNG*/, blob);
        resizedBitmap.recycle();
        byte[] bitmapdata = blob.toByteArray();
        return bitmapdata;
//        int previewHeight,previewWidth,previewFormat;
//
//        Camera.Parameters params = mCamera.getParameters();
//        previewHeight = 640;//200;//params.getPreviewSize().height;
//        previewWidth = 480;//200;//params.getPreviewSize().width;
//        previewFormat = params.getPreviewFormat();
//        /////////////////////
//        //camera.setDisplayOrientation(90);
//        Bitmap bitmap = null;
//        ByteArrayOutputStream outStream = new ByteArrayOutputStream();
//        YuvImage yuvImage = new YuvImage(
//                data, previewFormat, previewWidth, previewHeight, null);
//        // r = new Rect(80, 20, previewWidth - 80, previewHeight - 20);
//        int padX = previewWidth/10;
//        int padY = previewHeight/10;
//        Rect r = new Rect(padX,padY,previewWidth-padX,previewHeight-padY);
//        // r = new Rect(60,40,640-60,480-40);
//        yuvImage.compressToJpeg(r, 100, outStream);
//        bitmap = BitmapFactory.decodeByteArray(outStream.toByteArray(), 0,
//                outStream.size());
//        byte[] bitmapdata = outStream.toByteArray();
//        //Bitmap bitmap = null;
//        //YuvImage yuvimage = new YuvImage(yuv, ImageFormat.NV21,720,
//        //        1280, null);
////        ByteArrayOutputStream outStream = new ByteArrayOutputStream();
//        // yuvimage.compressToJpeg(new Rect(left+10, top+10, right+10, bottom+10), 100, outStream);
//        // yuvimage.compressToJpeg(new Rect(180, 100, 540, 400), 100, outStream);
//        // bitmap = BitmapFactory.decodeByteArray(outStream.toByteArray(), 0,
//        //         outStream.size());
//        // yuvimage = null;
//        outStream = null;
//        return bitmapdata;
    }

    private void sendToCameraPreview(String p){
        Intent intent=new Intent(this,ImagePreview.class);
        Bundle b = new Bundle();
        b.putSerializable(CAMERA, p);
        // b.putSerializable(USER_FARM,farm);
        intent.putExtras(b);
        startActivity(intent);
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
}