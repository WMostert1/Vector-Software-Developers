package vsd.co.za.sambugapp.CameraProcessing;

import android.hardware.*;
import android.net.Uri;
import android.os.Environment;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.FrameLayout;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;

import vsd.co.za.sambugapp.R;

public class Camera3 extends AppCompatActivity {

    private android.hardware.Camera mCamera;
    private CameraPreview2 mPreview;
    private static String TAG = "vsd";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_camera3);

        mCamera = getCameraInstance();

        // Create our Preview view and set it as the content of our activity.
        mPreview = new CameraPreview2(this, mCamera);
        FrameLayout preview = (FrameLayout) findViewById(R.id.camera_preview);
        preview.addView(mPreview);

    }

    /**
     * A safe way to get an instance of the Camera object.
     */
    public static android.hardware.Camera getCameraInstance() {
        android.hardware.Camera c = null;
        try {
            // attempt to get a Camera instance
            c = android.hardware.Camera.open();
        } catch (Exception e) {
            // Camera is not available (in use or does not exist)
        }

        // returns null if camera is unavailable
        return c;
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_camera3, menu);
        return true;
    }

    private android.hardware.Camera.PictureCallback mPicture = new android.hardware.Camera.PictureCallback() {

        @Override
        public void onPictureTaken(byte[] data, android.hardware.Camera camera) {
            File pictureFile = getOutputMediaFile();
            if (pictureFile == null){
               // Log.d(TAG, "Error creating media file, check storage permissions: " +
               //         e.getMessage());
                return;
            }

            try {
                FileOutputStream fos = new FileOutputStream(pictureFile);
                fos.write(data);
                fos.close();
            } catch (FileNotFoundException e) {
                Log.d(TAG, "File not found: " + e.getMessage());
            } catch (IOException e) {
                Log.d(TAG, "Error accessing file: " + e.getMessage());
            }
        }
    };

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

//    // Add a listener to the Capture button
//    Button captureButton = (Button) findViewById(R.id.button_capture);
//    captureButton.setOnClickListener(
//            new View.OnClickListener() {
//        @Override
//        public void onClick(View v) {
//            // get an image from the camera
//            mCamera.takePicture(null, null, mPicture);
//        }
//    }
//    );

    public void onClick2(View v) {
        // get an image from the camera
        mCamera.takePicture(null, null, mPicture);
    }


    /** Create a file Uri for saving an image or video */
    private static Uri getOutputMediaFileUri(int type){
        return Uri.fromFile(getOutputMediaFile());
    }

    /** Create a File for saving an image or video */
    private static File getOutputMediaFile(){
        // To be safe, you should check that the SDCard is mounted
        // using Environment.getExternalStorageState() before doing this.

        File mediaStorageDir = new File(Environment.getExternalStoragePublicDirectory(
                Environment.DIRECTORY_PICTURES), "MyCameraApp");
        // This location works best if you want the created images to be shared
        // between applications and persist after your app has been uninstalled.

        // Create the storage directory if it does not exist
        if (! mediaStorageDir.exists()){
            if (! mediaStorageDir.mkdirs()){
                Log.d("MyCameraApp", "failed to create directory");
                return null;
            }
        }

        // Create a media file name
        String timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss").format(new Date());
        File mediaFile;

     //   if (type == MEDIA_TYPE_IMAGE){
            mediaFile = new File(mediaStorageDir.getPath() + File.separator +
                    "IMG_"+ timeStamp + ".jpg");
        //} else if(type == MEDIA_TYPE_VIDEO) {
        //    mediaFile = new File(mediaStorageDir.getPath() + File.separator +
        //            "VID_"+ timeStamp + ".mp4");
        //} else {
        //    return null;
       // }

        return mediaFile;
    }
}
