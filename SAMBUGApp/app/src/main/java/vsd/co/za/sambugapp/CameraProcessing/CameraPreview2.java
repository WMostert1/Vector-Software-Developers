package vsd.co.za.sambugapp.CameraProcessing;

import java.io.IOException;

import android.content.Context;
import android.graphics.ImageFormat;
import android.hardware.Camera;
import android.util.Log;
import android.view.SurfaceHolder;
import android.view.SurfaceView;

public class CameraPreview2 extends SurfaceView implements SurfaceHolder.Callback {
	private SurfaceHolder mHolder;
	private Camera mCamera;
	boolean cameraConfigured;

	public CameraPreview2(Context context, Camera camera) {
		super(context);
		cameraConfigured = false;
		mCamera = camera;

		// Install a SurfaceHolder.Callback so we get notified when the
		// underlying surface is created and destroyed.
		mHolder = getHolder();
		mHolder.addCallback(this);
		// deprecated setting, but required on Android versions prior to 3.0
		mHolder.setType(SurfaceHolder.SURFACE_TYPE_PUSH_BUFFERS);

		//mHolder.setFixedSize(100, 100);
	}

	public void surfaceCreated(SurfaceHolder holder) {
		// The Surface has been created, now tell the camera where to draw the
		// preview.
		try {
			mCamera.setDisplayOrientation(90);
			mCamera.setPreviewDisplay(holder);
			mCamera.startPreview();
		} catch (IOException e) {
			Log.d("DG_DEBUG", "Error setting camera preview: " + e.getMessage());
		}

	}

	public void surfaceChanged(SurfaceHolder holder, int format, int width, int height) {
		// If your preview can change or rotate, take care of those events here.
		// Make sure to stop the preview before resizing or reformatting it.
		if (!cameraConfigured) {
			Camera.Parameters parameters=mCamera.getParameters();
			Camera.Size size=getBestPreviewSize(width, height, parameters);
			Camera.Size pictureSize=getSmallestPictureSize(parameters);

			if (size != null && pictureSize != null) {
				parameters.setPreviewSize(size.width, size.height);
				parameters.setPictureSize(pictureSize.width,
						pictureSize.height);
				parameters.setPictureFormat(ImageFormat.JPEG);
				mCamera.setParameters(parameters);
				cameraConfigured=true;
			}
		}
		if (mHolder.getSurface() == null) {
			// preview surface does not exist
			return;
		}

		// stop preview before making changes
		try {
			mCamera.stopPreview();
		} catch (Exception e) {
			// ignore: tried to stop a non-existent preview
		}

		// make any resize, rotate or reformatting changes here

		// start preview with new settings
		try {
			mCamera.setPreviewDisplay(mHolder);
			mCamera.startPreview();

		} catch (Exception e) {
			Log.d("DG_DEBUG", "Error starting camera preview: " + e.getMessage());
		}
	}

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

	private Camera.Size getSmallestPictureSize(Camera.Parameters parameters) {
		Camera.Size result=null;
		int maxWidth = 3264;
		int maxHeight = 2448;
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

		return(result);
	}

	public void surfaceDestroyed(SurfaceHolder holder) {
		// empty. Take care of releasing the Camera preview in your activity.
	}

}
