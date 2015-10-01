package vsd.co.za.sambugapp.CameraProcessing;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.ImageFormat;
import android.graphics.Matrix;
import android.graphics.Rect;
import android.graphics.YuvImage;
import android.hardware.Camera;
import android.hardware.Camera.Parameters;
import android.hardware.Camera.PreviewCallback;
import android.hardware.Camera.Size;
import android.os.Environment;
import android.util.AttributeSet;
import android.util.Log;
import android.view.SurfaceHolder;
import android.view.SurfaceView;
import android.widget.Toast;


public class CameraPreview extends SurfaceView implements
        SurfaceHolder.Callback {
    private SurfaceHolder cameraPreviewHolder;
    private Camera camera;
    private Context context;
    private boolean cameraConfigured = false;
    private boolean inPreview = false;
    private Size previewSize;
    private byte yuv[];
    private int previewHeight = 0;

    private int previewWidth = 0;

    private int previewFormat = 0;
    Rect r;

    public CameraPreview(Context context, AttributeSet attrs) {
        super(context, attrs);
        this.context = context;
        cameraPreviewHolder = getHolder();
        cameraPreviewHolder.addCallback(this);
        // we need this for compatibility
        cameraPreviewHolder.setType(SurfaceHolder.SURFACE_TYPE_PUSH_BUFFERS);
    }

    public void releaseCamera() {
        if (inPreview) {
            camera.stopPreview();
            camera.release();
            camera = null;
            inPreview = false;
        }
    }

    public void resumePreview(){
        if (!inPreview) {
            camera = Camera.open();
            camera.setDisplayOrientation(90);
        }
    }

    private void initPreview(int width, int height) {
        if (camera != null && cameraPreviewHolder.getSurface() != null) {
            try {
                camera.setPreviewDisplay(cameraPreviewHolder);
            } catch (Throwable t) {
                Log.e("CameraPreview", "Exception in setPreviewDisplay()", t);
                Toast.makeText(context, t.getMessage(), Toast.LENGTH_LONG)
                        .show();
            }
            if (!cameraConfigured) {
                Parameters parameters = camera.getParameters();
                previewSize = getBestPreviewSize(width, height, parameters);
               // pre
                if (previewSize != null) {
                    parameters.setPreviewSize(previewSize.width,
                            previewSize.height);
                    camera.setParameters(parameters);
                    cameraConfigured = true;
                }
            }
            Parameters params = camera.getParameters();
            previewHeight = params.getPreviewSize().height;
            previewWidth = params.getPreviewSize().width;
            previewFormat = params.getPreviewFormat();


            ///New
            
        }
    }

    private Size getBestPreviewSize(int width, int height, Parameters parameters) {
        Camera.Size result = null;
        for (Camera.Size size : parameters.getSupportedPreviewSizes()) {
            if (size.width <= width && size.height <= height) {
                if (result == null) {
                    result = size;
                } else {
                    int resultArea = result.width * result.height;
                    int newArea = size.width * size.height;
                    if (newArea > resultArea) {
                        result = size;
                    }
                }

            }
        }
//        Size s = new Size();
//        s.width = 720;
//        s.height = 1280;

        //result = s;
        return result;
    }

    private void startPreview() {
        if (cameraConfigured && camera != null) {
            camera.startPreview();
            yuv = new byte[getBufferSize()];
            camera.addCallbackBuffer(yuv);
            camera.setPreviewCallbackWithBuffer(new PreviewCallback() {
                public synchronized void onPreviewFrame(byte[] data, Camera c) {
                    if (camera != null) {
                        camera.addCallbackBuffer(yuv);
                    }
                }
            });
            inPreview = true;
        }
    }

    @Override
    public void surfaceChanged(SurfaceHolder holder, int format, int width,
                               int height) {
        initPreview(width, height);
        startPreview();
    }

    @Override
    public void surfaceCreated(SurfaceHolder holder) {
    }

    @Override
    public void surfaceDestroyed(SurfaceHolder holder) {

    }

    private int getBufferSize() {
        int pixelformat = ImageFormat.getBitsPerPixel(camera.getParameters()
                .getPreviewFormat());
        int bufSize = previewWidth* previewHeight * pixelformat / 8;
        return bufSize;
    }

    public Bitmap getBitmap(int left, int top, int right, int bottom)
            throws IOException {
        Bitmap bitmap = null;
        ByteArrayOutputStream outStream = new ByteArrayOutputStream();
        YuvImage yuvImage = new YuvImage(
                yuv, previewFormat, previewWidth, previewHeight, null);
       // r = new Rect(80, 20, previewWidth - 80, previewHeight - 20);
        int padX = previewWidth/10;
        int padY = previewHeight/10;
        r = new Rect(padX,padY,previewWidth-padX,previewHeight-padY);
       // r = new Rect(60,40,640-60,480-40);
        yuvImage.compressToJpeg(r, 100, outStream);
        bitmap = BitmapFactory.decodeByteArray(outStream.toByteArray(), 0,
                        outStream.size());
        //Bitmap bitmap = null;
        //YuvImage yuvimage = new YuvImage(yuv, ImageFormat.NV21,720,
        //        1280, null);
//        ByteArrayOutputStream outStream = new ByteArrayOutputStream();
       // yuvimage.compressToJpeg(new Rect(left+10, top+10, right+10, bottom+10), 100, outStream);
       // yuvimage.compressToJpeg(new Rect(180, 100, 540, 400), 100, outStream);
       // bitmap = BitmapFactory.decodeByteArray(outStream.toByteArray(), 0,
       //         outStream.size());
       // yuvimage = null;
        outStream = null;
        return bitmap;
    }
    public static Bitmap cropBitmap(Bitmap origBmp, int targetSize) {
        final int w = origBmp.getWidth();
        final int h = origBmp.getHeight();

        float scale = ((float) targetSize) / (w < h ? w : h);

        Matrix matrix = new Matrix();
        if (w > h) {
            matrix.postRotate(90);
        }
        matrix.postScale(scale, scale);

        //if (DEBUG)
        //  Log.d(TAG, "origBmp: width=" + w + ", height=" + h);

        int pad = (int) ((float) (w > h ? w : h) - ((float) targetSize) / scale) / 2;
        //if (DEBUG)
        // Log.d(TAG, "pad=" + pad);

        int new_w = w - (w > h ? 2 * pad : 0);
        int new_h = h - (w > h ? 0 : 2 * pad);
        //if (DEBUG)
        //  Log.d(TAG, "new_w=" + new_w + ", new_h=" + new_h);

        Bitmap thumb = Bitmap.createBitmap(origBmp, w > h ? pad : 0, w > h ? 0 : pad, new_w, new_h,
                matrix, true);

        //if (DEBUG) {
       // Log.d(TAG, "tumb dim:" + thumb.getWidth() + "x" + thumb.getHeight());
        //}

        return thumb;
    }

}