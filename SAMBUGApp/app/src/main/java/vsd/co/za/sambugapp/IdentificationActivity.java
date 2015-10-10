package vsd.co.za.sambugapp;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Matrix;
import android.os.Build;
import android.provider.MediaStore;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;

import android.widget.GridView;
import android.widget.ImageView;
import android.widget.Toast;


import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.lang.reflect.Constructor;
import java.lang.reflect.Field;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;


import vsd.co.za.sambugapp.CameraProcessing.CustomCamera;
import vsd.co.za.sambugapp.DataAccess.SpeciesDAO;
import vsd.co.za.sambugapp.DomainModels.Species;

/**
 * This activity class is the third viewable screen when interacting with the App in order
 * to capture scouting data.
 * <p/>
 * This activity makes use of the default camera handler app on the device.
 */
public class IdentificationActivity extends AppCompatActivity {

    public static final int REQUEST_TAKE_PHOTO = 89;
    private static final String FIRST_TIME_INDEX = "za.co.vsd.firs_activity";
    private static final String FIELD_BITMAP = "za.co.vsd.field_bitmap";
    public static final String IDENTIFICATION_SPECIES="za.co.vsd.identification_species";
    private ImageView mImageView = null;
    private Bitmap bitmap = null;
    private Species currentEntry = null;
    private int createCounter = 0;
    private String fullPathName;


    public void doAutomaticClassification(View view) {
        Toast.makeText(getApplicationContext(), "This feature is currently in development", Toast.LENGTH_SHORT).show();
    }

    /**
     * Saves the current state of the activity for future activities being restarted
     * @param savedInstanceState The saved activity state of the currently running activity
     */
    @Override
    public void onSaveInstanceState(Bundle savedInstanceState) {
        super.onSaveInstanceState(savedInstanceState);

        //createCounter is used to only start the camera once
        savedInstanceState.putInt(FIRST_TIME_INDEX, createCounter);
        savedInstanceState.putParcelable(FIELD_BITMAP, bitmap);

    }

    /**
     * Main responsibilities:
     * ------------------------------------------
     * Handles the saved state bundle
     * Loads the gallery by initialising the grid and loading compressed images
     * Checks on first startup if the database Species
     * @param savedInstanceState State of the previous running version of the activity
     */
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

            if (savedInstanceState != null) {
                bitmap = savedInstanceState.getParcelable(FIELD_BITMAP);
                createCounter = savedInstanceState.getInt(FIRST_TIME_INDEX);
              //  getPicture(getIntent());
            }
        else {
                dispatchTakePictureIntent();

            }

        //Checks/loads species data into the Species table of the database
            if (createCounter == 0) {
                SpeciesDAO speciesDAO = new SpeciesDAO(getApplicationContext());
                try {
                    speciesDAO.open();
                    if (speciesDAO.isEmpty()) {
                        speciesDAO.loadPresets();
                    }

                    speciesDAO.close();

                } catch (Exception e) {
                    e.printStackTrace();
                }

                //dispatchTakePictureIntent();
            }
            if (createCounter == 0) createCounter++;


            setContentView(R.layout.activity_identification);

            GridView gridview = (GridView) findViewById(R.id.gvIdentification_gallery);
            gridview.setAdapter(new ImageAdapter(this));

            gridview.setOnItemClickListener(new AdapterView.OnItemClickListener() {
                public void onItemClick(AdapterView<?> parent, View v,
                                        int position, long id) {
                    SpeciesDAO speciesDAO = new SpeciesDAO(getApplicationContext());
                    try {
                        speciesDAO.open();
                        currentEntry = speciesDAO.getSpeciesByID(position + 1);
                        Toast.makeText(getApplicationContext(), "You chose " + currentEntry.getSpeciesName() + " at instar " + currentEntry.getLifestage(), Toast.LENGTH_SHORT).show();
                        ImageView comparisonImage = (ImageView) findViewById(R.id.ivCompare);
                        byte[] imgData = currentEntry.getIdealPicture();
                        comparisonImage.setImageBitmap(BitmapFactory.decodeByteArray(imgData, 0,
                                imgData.length));
                        speciesDAO.close();
                    } catch (Exception e) {
                        e.printStackTrace();
                    }

                }
            });

            mImageView = (ImageView) findViewById(R.id.ivFieldPicture);

            if (bitmap != null) mImageView.setImageBitmap(bitmap);
          //
    }

    public Species getCurrentEntry() {
        return currentEntry;
    }



    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_identification, menu);
        return true;
    }

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
    @Override
    protected void onNewIntent(Intent intent) {
        super.onNewIntent(intent);
        setIntent(intent);//must store the new intent unless getIntent() will return the old one
        getPicture(getIntent());

    }
    /**
     * The case where the photo is returned from the external camera app is handled here
     * @param requestCode The identification code of a specific
     * @param resultCode The code indicating the outcome of the request
     * @param data Data received from another activity
     */
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
     //   getPicture(getIntent());
//       InputStream stream = null;
//        if (requestCode == REQUEST_TAKE_PHOTO && resultCode == Activity.RESULT_OK)
//
////            try {
////                //Recycle unused bitmaps
////                if (bitmap != null) {
////                    bitmap.recycle();
////                }
////                stream = getContentResolver().openInputStream(data.getData());
////                final BitmapFactory.Options options = new BitmapFactory.Options();
////                options.inJustDecodeBounds = true;
////                BitmapFactory.decodeStream(stream, null, options);
////
////                // Calculate inSampleSize
////                options.inSampleSize = ImageAdapter.calculateInSampleSize(options, 150, 150);
////
////                // Decode bitmap with inSampleSize set
////                options.inJustDecodeBounds = false;
////                stream = getContentResolver().openInputStream(data.getData());
////                bitmap = BitmapFactory.decodeStream(stream, null, options);
////
////                //TODO: Rotate the image
////
////                mImageView.setImageBitmap(bitmap);
//
//            } catch (FileNotFoundException e) {
//                e.printStackTrace();
//            } finally {
//                if (stream != null)
//                    try {
//                        stream.close();
//                    } catch (IOException e) {
//                        e.printStackTrace();
//                    }
//        }
    }

    /**
     * Starts a new intent to take a picture with the device's camera
     */
    private void dispatchTakePictureIntent(){
        Intent takePictureIntent = new Intent(this,CustomCamera.class);
        //takePictureIntent.setFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP|Intent.FLAG_ACTIVITY_NEW_TASK);
        startActivityForResult(takePictureIntent, 0);

        //MediaStore.ACTION_IMAGE_CAPTURE);
//        if (takePictureIntent.resolveActivity(getPackageManager()) != null) {
//            startActivityForResult(takePictureIntent, REQUEST_TAKE_PHOTO);
//        }

    }


    /**
     * This function puts the current Species entry as well as the field picture taken
     * into a bundle which is then returned to the EnterDataActivity
     * @param view THe button that was clicked
     */
    public void sendResultBack(View view) {
        Intent output = new Intent();
        Bundle bundle = new Bundle();

        currentEntry.setIdealPicture(null);
        bundle.putSerializable(IDENTIFICATION_SPECIES, currentEntry
        );
        if (bitmap==null){
            Log.e("PROB","IT~S NULL HERE");
        }
        Bitmap currentPicture = bitmap;
        currentPicture = Bitmap.createScaledBitmap(currentPicture, 50, 50, true);
        bundle.putParcelable("Image", currentPicture);
        output.putExtras(bundle);
        setResult(RESULT_OK, output);
        finish();
    }

    public void getPicture(Intent intent){
        Bundle b=intent.getExtras();

        fullPathName= (String)b.get(CustomCamera.CAMERA);


        File imgFile = new File(fullPathName);


        if(imgFile.exists()){

            Bitmap myBitmap = BitmapFactory.decodeFile(imgFile.getAbsolutePath());
            Bitmap rBitmap = rotateBitmap(fullPathName,myBitmap);
            //ImageView myImage = (ImageView) findViewById(R.id.ivImage);

            mImageView.setImageBitmap(rBitmap);

        }
    }

    public static Bitmap rotateBitmap(String src, Bitmap bitmap) {
        try {
            int orientation = getExifOrientation(src);

            if (orientation == 1) {
                return bitmap;
            }

            Matrix matrix = new Matrix();
            switch (orientation) {
                case 2:
                    matrix.setScale(-1, 1);
                    break;
                case 3:
                    matrix.setRotate(180);
                    break;
                case 4:
                    matrix.setRotate(180);
                    matrix.postScale(-1, 1);
                    break;
                case 5:
                    matrix.setRotate(90);
                    matrix.postScale(-1, 1);
                    break;
                case 6:
                    matrix.setRotate(90);
                    break;
                case 7:
                    matrix.setRotate(-90);
                    matrix.postScale(-1, 1);
                    break;
                case 8:
                    matrix.setRotate(-90);
                    break;
                default:
                    return bitmap;
            }

            try {
                Bitmap oriented = Bitmap.createBitmap(bitmap, 0, 0, bitmap.getWidth(), bitmap.getHeight(), matrix, true);
                bitmap.recycle();
                return oriented;
            } catch (OutOfMemoryError e) {
                e.printStackTrace();
                return bitmap;
            }
        } catch (IOException e) {
            e.printStackTrace();
        }

        return bitmap;
    }


    private static int getExifOrientation(String src) throws IOException {
        int orientation = 1;

        try {
            /**
             * if your are targeting only api level >= 5
             * ExifInterface exif = new ExifInterface(src);
             * orientation = exif.getAttributeInt(ExifInterface.TAG_ORIENTATION, 1);
             */
            if (Build.VERSION.SDK_INT >= 5) {
                Class<?> exifClass = Class.forName("android.media.ExifInterface");
                Constructor<?> exifConstructor = exifClass.getConstructor(new Class[] { String.class });
                Object exifInstance = exifConstructor.newInstance(new Object[] { src });
                Method getAttributeInt = exifClass.getMethod("getAttributeInt", new Class[] { String.class, int.class });
                Field tagOrientationField = exifClass.getField("TAG_ORIENTATION");
                String tagOrientation = (String) tagOrientationField.get(null);
                orientation = (Integer) getAttributeInt.invoke(exifInstance, new Object[] { tagOrientation, 1});
            }
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        } catch (SecurityException e) {
            e.printStackTrace();
        } catch (NoSuchMethodException e) {
            e.printStackTrace();
        } catch (IllegalArgumentException e) {
            e.printStackTrace();
        } catch (InstantiationException e) {
            e.printStackTrace();
        } catch (IllegalAccessException e) {
            e.printStackTrace();
        } catch (InvocationTargetException e) {
            e.printStackTrace();
        } catch (NoSuchFieldException e) {
            e.printStackTrace();
        }

        return orientation;
    }


}
