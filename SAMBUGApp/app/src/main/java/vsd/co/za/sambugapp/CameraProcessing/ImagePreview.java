package vsd.co.za.sambugapp.CameraProcessing;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.ImageView;

import java.io.File;
import java.io.IOException;

import vsd.co.za.sambugapp.IdentificationActivity;
import vsd.co.za.sambugapp.R;

import java.io.InputStream;
import java.lang.reflect.Constructor;
import java.lang.reflect.Field;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;

import android.graphics.Matrix;
import android.os.Build;

public class ImagePreview extends AppCompatActivity {
    ImageView imageView;
    String fullPathName;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_image_preview);
        imageView = (ImageView) findViewById(R.id.ivImage);
        acceptImage(getIntent());
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_activity, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        return true;
    }

    /**
     * Accepts the image from Camera Preview.
     * @param intent
     */
    private void acceptImage(Intent intent){
        Bundle b=intent.getExtras();
        if (b!=null) {
            fullPathName = (String) b.get(CustomCamera.CAMERA);
            File imgFile = new File(fullPathName);

            if (imgFile.exists()) {
                Bitmap myBitmap = BitmapFactory.decodeFile(imgFile.getAbsolutePath());
                Bitmap rBitmap = rotateBitmap(fullPathName, myBitmap);
                imageView.setImageBitmap(rBitmap);
            }
        } else {
            Intent i = new Intent(getApplicationContext(),CustomCamera.class);
            startActivityForResult(i,0);
        }

    }

    /**
     * Deletes photo if cancel is selected.
     * @param v
     */
    public void deletePhoto(View v){
        File discardedPhoto = new File(fullPathName);
        discardedPhoto.delete();
        fullPathName="";
        imageView.setImageBitmap(null);
        Intent intent = new Intent(this,CustomCamera.class);
        startActivityForResult(intent,0);
    }

    /**
     * Rotates the image to display it in the correct orientation
     * @param src
     * @param bitmap
     * @return
     */

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

    /**
     * Sends it back to Identification Activity
     * @param v
     */
    public void SendToIdentificationActivity(View v){
            Intent intent = new Intent();
            Bundle b = new Bundle();
            b.putSerializable(CustomCamera.CAMERA, fullPathName);
            intent.setFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP
                    | Intent.FLAG_ACTIVITY_CLEAR_TOP);
            intent.putExtras(b);
            setResult(RESULT_OK,intent);
            finish();
        }

    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == 0) {
            if (resultCode == Activity.RESULT_OK){
                acceptImage(data);
            } else {
                finish();
            }
        }
    }
    }


