package vsd.co.za.sambugapp.CameraProcessing;

import android.app.Dialog;
import android.content.Intent;
import android.os.Environment;
import android.os.Bundle;
import android.util.Log;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
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
    FileOutputStream outStream = null;
    public static final String CAMERA="za.co.vsd.camera";

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
                Bitmap b = null;
                try {
//                    b = cameraPreview.getBitmap(peephole.getPeepSideX(),
//                            peephole.getPeepSideY(), peephole.getPeepSideX()
//                                    + peephole.getPeepSide(),
//                            peephole.getPeepSideY() + peephole.getPeepSide());
                    b = cameraPreview.getBitmap(peephole.getBoxX1(),
                            peephole.getBoxY1(), peephole.getBoxX2()
                                   ,
                            peephole.getBoxY2());
                    //b = cropBitmap()
                } catch (IOException e) {
                    // TODO Auto-generated catch block
                    e.printStackTrace();
                }
                if (b != null) {

                    ByteArrayOutputStream stream = new ByteArrayOutputStream();
                    b.compress(Bitmap.CompressFormat.JPEG, 100, stream);
                    byte[] byteArray = stream.toByteArray();
                    sendToCameraPreview(byteArray);
//                    File pictureFileDir = getDir();

//                    if (!pictureFileDir.exists() && !pictureFileDir.mkdirs()) {
//
//                        Log.e("Here", "Can't create directory to save image.");
//                        Toast.makeText(getApplicationContext(), "Can't create directory to save image.",
//                                Toast.LENGTH_LONG).show();
//                        return;
//
//                    }
//
//                    SimpleDateFormat dateFormat = new SimpleDateFormat("yyyymmddhhmmss");
//                    String date = dateFormat.format(new Date());
//                    String photoFile = "Picture_" + date + ".jpg";
//
//                    String filename = pictureFileDir.getPath() + File.separator + photoFile;
//
//                    File pictureFile = new File(filename);
//
//                    try {
//                        FileOutputStream fos = new FileOutputStream(pictureFile);
//                        fos.write(byteArray);
//                        fos.close();
//                        Toast.makeText(getApplicationContext(), "New Image saved:" + photoFile,
//                                Toast.LENGTH_LONG).show();
//                    } catch (Exception error) {
//                        Log.e("Here", "File");// + filename + "not saved: "
//                        //+ error.getMessage());
//                        Toast.makeText(getApplicationContext(), "Image could not be saved.",
//                                Toast.LENGTH_LONG).show();
//                    }

                   // ImageProcessor ip = new ImageProcessor(this, b);

               //    peephole.setBitmap(ip.process());
               //     peephole.invalidate();
                }
                break;

        }

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




}