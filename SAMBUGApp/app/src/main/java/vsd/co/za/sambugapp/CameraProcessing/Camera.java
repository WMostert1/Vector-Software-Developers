package vsd.co.za.sambugapp.CameraProcessing;

import android.app.Dialog;
import android.content.Context;
import android.content.Intent;
import android.database.Cursor;
import android.graphics.BitmapFactory;
import android.graphics.Matrix;
import android.media.ExifInterface;
import android.net.Uri;
import android.os.Environment;
import android.os.Bundle;
import android.provider.MediaStore;
import android.util.Log;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.text.SimpleDateFormat;
import java.util.Date;

import vsd.co.za.sambugapp.EnterDataActivity;
import vsd.co.za.sambugapp.R;

import android.app.Activity;
import android.graphics.Bitmap;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;
import android.widget.Button;
import android.view.View.OnClickListener;
import android.widget.ImageButton;
import android.widget.Toast;

public class Camera extends Activity implements OnClickListener {
    private CameraPreview cameraPreview;
    private Peephole peephole;
    private ImageButton buttonGo;
    private Button buttonClear;
    int rotation;
    FileOutputStream outStream = null;
    public static final String CAMERA="za.co.vsd.camera";
    Uri picUri;
    private Bitmap takenImage;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        requestWindowFeature(Window.FEATURE_NO_TITLE);

        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN,

                WindowManager.LayoutParams.FLAG_FULLSCREEN);
        setContentView(R.layout.activity_camera);

        cameraPreview = (CameraPreview) findViewById(R.id.cameraPreview1);
        peephole = (Peephole) findViewById(R.id.peephole1);
        buttonGo = (ImageButton) findViewById(R.id.imgbCamera);
        buttonGo.setOnClickListener(this);
       // buttonClear = (Button) findViewById(R.id.buttonClear);
       // buttonClear.setOnClickListener(this);
    }

    @Override
    public void onResume() {
        super.onResume();
        cameraPreview.resumePreview();
    }

    @Override
    public void onPause() {
        super.onPause();
       // cameraPreview.releaseCamera();
        super.onStop();
    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.imgbCamera:
                takenImage = null;
                try {
//                    b = cameraPreview.getBitmap(peephole.getPeepSideX(),
//                            peephole.getPeepSideY(), peephole.getPeepSideX()
//                                    + peephole.getPeepSide(),
//                            peephole.getPeepSideY() + peephole.getPeepSide());
                    takenImage = cameraPreview.getBitmap(peephole.getBoxX1(),
                            peephole.getBoxY1(), peephole.getBoxX2()
                                   ,
                            peephole.getBoxY2());
                    //b = cropBitmap()
                } catch (IOException e) {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                }

                int k = cameraPreview.getOrientation();
                if (takenImage != null) {
                    picUri = getImageUri(this,takenImage);
                    try {
                        ExifInterface exif = new ExifInterface(picUri.getPath());

                       // rotation = exif.getAttributeInt(ExifInterface.TAG_ORIENTATION, ExifInterface.ORIENTATION_NORMAL);
                    }
                    catch(Exception e){
                        Log.d("ds","sdsd");
                    }
                    Rotate(this, picUri);
//                    try {
//  //                      scaleImage(this,picUri);
//                    } catch (IOException e) {
//                        e.printStackTrace();
//                    }
                    int hello = rotation;
                    ByteArrayOutputStream stream = new ByteArrayOutputStream();
                    takenImage.compress(Bitmap.CompressFormat.JPEG, 100, stream);
                    byte[] byteArray = stream.toByteArray();

//                    /////////////////////////////////////////////////////
////                    String timeStamp = new SimpleDateFormat( "yyyyMMdd_HHmmss").format( new Date( ));
////                    output_file_name = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DCIM) + File.separator + timeStamp + ".jpeg";
//
//                    File pictureFileDir = getDir();
//                    SimpleDateFormat dateFormat = new SimpleDateFormat("yyyymmddhhmmss");
//                    String date = dateFormat.format(new Date());
//                    String photoFile = "Picture_" + date + ".jpg";
//
//                    String filename = pictureFileDir.getPath() + File.separator + photoFile;
//
//                    File pictureFile = new File(filename);
//
//
//                    try {
//                        FileOutputStream fos = new FileOutputStream(pictureFile);
//
//                        Bitmap realImage = BitmapFactory.decodeByteArray(byteArray, 0, byteArray.length);
//
//                        ExifInterface exif=new ExifInterface(pictureFile.toString());
//
//                        Log.d("EXIF value", exif.getAttribute(ExifInterface.TAG_ORIENTATION));
//                        if(exif.getAttribute(ExifInterface.TAG_ORIENTATION).equalsIgnoreCase("6")){
//                            realImage= rotate(realImage, 90);
//                        } else if(exif.getAttribute(ExifInterface.TAG_ORIENTATION).equalsIgnoreCase("8")){
//                            realImage= rotate(realImage, 270);
//                        } else if(exif.getAttribute(ExifInterface.TAG_ORIENTATION).equalsIgnoreCase("3")){
//                            realImage= rotate(realImage, 180);
//                        } else if(exif.getAttribute(ExifInterface.TAG_ORIENTATION).equalsIgnoreCase("0")){
//                            realImage= rotate(realImage, 90);
//                        }
//
//                       // boolean bo = realImage.compress(Bitmap.CompressFormat.JPEG, 100, fos);
//
//                        fos.close();
//
//                        //((ImageView) findViewById(R.id.imageview)).setImageBitmap(realImage);
//
//                      //  Log.d("Info", bo + "");
//
//                    } catch (FileNotFoundException e) {
//                        Log.d("Info", "File not found: " + e.getMessage());
//                    } catch (IOException e) {
//                        Log.d("TAG", "Error accessing file: " + e.getMessage());
//                    }///////////////////////////////

                    File pictureFileDir = getDir();
                    Bitmap realImage = BitmapFactory.decodeByteArray(byteArray, 0, byteArray.length);

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
                        FileOutputStream fos = new FileOutputStream(pictureFile);
                        ExifInterface exif=new ExifInterface(pictureFile.toString());

                       // Log.d("EXIF value", exif.getAttribute(ExifInterface.TAG_ORIENTATION));
                        if(exif.getAttribute(ExifInterface.TAG_ORIENTATION).equalsIgnoreCase("6")){
                            realImage= rotate(realImage, 90);
                        } else if(exif.getAttribute(ExifInterface.TAG_ORIENTATION).equalsIgnoreCase("8")){
                            realImage= rotate(realImage, 270);
                        } else if(exif.getAttribute(ExifInterface.TAG_ORIENTATION).equalsIgnoreCase("3")){
                            realImage= rotate(realImage, 180);
                        } else if(exif.getAttribute(ExifInterface.TAG_ORIENTATION).equalsIgnoreCase("0")){
                            realImage= rotate(realImage, 90);
                        }
                        fos.write(byteArray);
                        fos.close();
                        Toast.makeText(getApplicationContext(), "New Image saved:" + photoFile,
                                Toast.LENGTH_LONG).show();
                    } catch (Exception error) {
                        Log.e("Here", "File");// + filename + "not saved: "
                        //+ error.getMessage());
                        Toast.makeText(getApplicationContext(), "Image could not be saved.",
                                Toast.LENGTH_LONG).show();
                    }

                   // ImageProcessor ip = new ImageProcessor(this, b);

               //    peephole.setBitmap(ip.process());
               //     peephole.invalidate();
                }
                break;

        }

    }

    public Bitmap rotate(Bitmap b, int deg){
        Bitmap ret = null;
        Matrix matrix = new Matrix();
        matrix.postRotate(deg);
        b = Bitmap.createBitmap(b, 0, 0,
                b.getWidth(), b.getHeight(),
                matrix, true);
        return ret;
    }

    public Uri getImageUri(Context inContext, Bitmap inImage) {
        ByteArrayOutputStream bytes = new ByteArrayOutputStream();
        inImage.compress(Bitmap.CompressFormat.JPEG, 100, bytes);
        String path = MediaStore.Images.Media.insertImage(inContext.getContentResolver(), inImage, "Title", null);
        return Uri.parse(path);
    }

    public Bitmap Rotate(Context context, Uri photoUri) {
    /* it's on the external media. */
        Bitmap b = null;
        Cursor cursor = context.getContentResolver().query(photoUri,
                new String[] { MediaStore.Images.ImageColumns.ORIENTATION }, null, null, null);

        if (cursor.getCount() != 1) {
            return null;
        }

        cursor.moveToFirst();
        int orientation= cursor.getInt(0);
        if(orientation > 0){
            Matrix matrix = new Matrix();
            matrix.postRotate(orientation);

            b = Bitmap.createBitmap(b, 0, 0, b.getWidth(),
                    b.getHeight(), matrix, true);
        }
        return b;
    }
    private void sendToCameraPreview(byte [] img){
        Intent intent=new Intent(this,ImagePreview.class);
        Bundle b = new Bundle();
        b.putSerializable(CAMERA, img);
       // b.putSerializable(USER_FARM,farm);
        intent.putExtras(b);
        startActivity(intent);
    }

    private File getDir() {
        File sdDir = Environment
                .getExternalStoragePublicDirectory(Environment.DIRECTORY_PICTURES);
        return new File(sdDir, "CameraAPIDemo");
    }

    /* Rotates the image accordingly.
            */
    public void Rotate2(Context context, Uri photoUri) {
    /* it's on the external media. */
       // Bitmap bitmap = null;
        Cursor cursor = context.getContentResolver().query(photoUri,
                new String[] { MediaStore.Images.ImageColumns.ORIENTATION }, null, null, null);

        if (cursor.getCount() != 1) {
            return;// null;
        }

        cursor.moveToFirst();
        int orientation= cursor.getInt(0);
        if(orientation > 0){
            Matrix matrix = new Matrix();
            matrix.postRotate(orientation);

            takenImage = Bitmap.createBitmap(takenImage, 0, 0, takenImage.getWidth(),
                    takenImage.getHeight(), matrix, true);
        }
      //  return bitmap;
    }

    public static void scaleImage(Context context, Uri photoUri) throws IOException {
        InputStream is = context.getContentResolver().openInputStream(photoUri);
        BitmapFactory.Options dbo = new BitmapFactory.Options();
        dbo.inJustDecodeBounds = true;
        BitmapFactory.decodeStream(is, null, dbo);
        is.close();

        int rotatedWidth, rotatedHeight;
        int orientation = getOrientation(context, photoUri);



        if (orientation == 90 || orientation == 270) {
            rotatedWidth = dbo.outHeight;
            rotatedHeight = dbo.outWidth;
        } else {
            rotatedWidth = dbo.outWidth;
            rotatedHeight = dbo.outHeight;
        }

        Bitmap srcBitmap;
        is = context.getContentResolver().openInputStream(photoUri);
//        if (rotatedWidth > MAX_IMAGE_DIMENSION || rotatedHeight > MAX_IMAGE_DIMENSION) {
//            float widthRatio = ((float) rotatedWidth) / ((float) MAX_IMAGE_DIMENSION);
//            float heightRatio = ((float) rotatedHeight) / ((float) MAX_IMAGE_DIMENSION);
//            float maxRatio = Math.max(widthRatio, heightRatio);
//
//            // Create the bitmap from file
//            BitmapFactory.Options options = new BitmapFactory.Options();
//            options.inSampleSize = (int) maxRatio;
//            srcBitmap = BitmapFactory.decodeStream(is, null, options);
//        } else {
//            srcBitmap = BitmapFactory.decodeStream(is);
//        }
        is.close();

        /*
         * if the orientation is not 0 (or -1, which means we don't know), we
         * have to do a rotation.
         */
//        if (orientation > 0) {
//            Matrix matrix = new Matrix();
//            matrix.postRotate(orientation);
//
//            srcBitmap = Bitmap.createBitmap(srcBitmap, 0, 0, srcBitmap.getWidth(),
//                    srcBitmap.getHeight(), matrix, true);
//        }
//
//        String type = context.getContentResolver().getType(photoUri);
//        ByteArrayOutputStream baos = new ByteArrayOutputStream();
//        if (type.equals("image/png")) {
//            srcBitmap.compress(Bitmap.CompressFormat.PNG, 100, baos);
//        } else if (type.equals("image/jpg") || type.equals("image/jpeg")) {
//            srcBitmap.compress(Bitmap.CompressFormat.JPEG, 100, baos);
//        }
//        byte[] bMapArray = baos.toByteArray();
//        baos.close();
//        return BitmapFactory.decodeByteArray(bMapArray, 0, bMapArray.length);
    }

    public static int getOrientation(Context context, Uri photoUri) {
        /* it's on the external media. */
        Cursor cursor = context.getContentResolver().query(photoUri,
                new String[]{MediaStore.Images.ImageColumns.ORIENTATION}, null, null, null);

        if (cursor.getCount() != 1) {
            return -1;
        }

       // cursor.moveToFirst();

        if (cursor.moveToFirst()){
            do{
              //  String data = cursor.getColumnNames();
                // do what ever you want here
            }while(cursor.moveToNext());
        }
       // cursor.close();
        return cursor.getInt(0);
      //  return 0;
    }
}