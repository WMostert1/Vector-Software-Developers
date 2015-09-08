package vsd.co.za.sambugapp;

import android.content.Context;
import android.content.res.Resources;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.GridView;
import android.widget.ImageView;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.InputStream;

/**
 * Created by Aeolus on 2015-07-14.
 */
public class ImageAdapter extends BaseAdapter {
    private Context mContext;

    public ImageAdapter(Context c) {
        mContext = c;
    }

    public int getCount() {
        return mThumbIds.length;
    }

    public Object getItem(int position) {
        return null;
    }

    public long getItemId(int position) {
        return 0;
    }

    // create a new ImageView for each item referenced by the Adapter
    public View getView(int position, View convertView, ViewGroup parent) {
        ImageView imageView;
        if (convertView == null) {
            // if it's not recycled, initialize some attributes
            imageView = new ImageView(mContext);
            int columnWidth = ((GridView) parent).getColumnWidth();
            imageView.setLayoutParams(new GridView.LayoutParams(columnWidth, columnWidth));
            imageView.setScaleType(ImageView.ScaleType.CENTER_CROP);
            imageView.setPadding(1, 1, 1, 1);

        } else {
            imageView = (ImageView) convertView;
        }

        imageView.setImageBitmap(decodeSampledBitmapFromResource(mContext.getResources(), mThumbIds[position], 60, 60));
        return imageView;
    }

    // references to our images
    private Integer[] mThumbIds = {
            R.drawable.coconut_inst_1,
            R.drawable.coconut_inst_2,
            R.drawable.coconut_inst_3,
            R.drawable.coconut_inst_4,

            R.drawable.green_veg_inst_1,
            R.drawable.green_veg_inst_2,
            R.drawable.green_veg_inst_3,
            R.drawable.green_veg_inst_4,

            R.drawable.two_spot_inst_1,
            R.drawable.two_spot_inst_2,
            R.drawable.two_spot_inst_3,
            R.drawable.two_spot_inst_4,

            R.drawable.yellow_edged_inst_1,
            R.drawable.yellow_edged_inst_2,
            R.drawable.yellow_edged_inst_3,
            R.drawable.yellow_edged_inst_4,
    };

    public static Bitmap compressBitmap(InputStream in, int reqWidth, int reqHeight) {
        final BitmapFactory.Options options = new BitmapFactory.Options();
        options.inJustDecodeBounds = true;
        BitmapFactory.decodeStream(in, null, options);

        // Calculate inSampleSize
        options.inSampleSize = calculateInSampleSize(options, reqWidth, reqHeight);

        // Decode bitmap with inSampleSize set
        options.inJustDecodeBounds = false;
        Bitmap result = BitmapFactory.decodeStream(in, null, options);
        return result;
    }


    public static Bitmap decodeSampledBitmapFromResource(Resources res, int resId,
                                                         int reqWidth, int reqHeight) {

        // First decode with inJustDecodeBounds=true to check dimensions
        final BitmapFactory.Options options = new BitmapFactory.Options();
        options.inJustDecodeBounds = true;
        BitmapFactory.decodeResource(res, resId, options);

        // Calculate inSampleSize
        options.inSampleSize = calculateInSampleSize(options, reqWidth, reqHeight);

        // Decode bitmap with inSampleSize set
        options.inJustDecodeBounds = false;
        return BitmapFactory.decodeResource(res, resId, options);
    }

    public static int calculateInSampleSize(
            BitmapFactory.Options options, int reqWidth, int reqHeight) {
        // Raw height and width of image
        final int height = options.outHeight;
        final int width = options.outWidth;
        int inSampleSize = 1;

        if (height > reqHeight || width > reqWidth) {

            final int halfHeight = height / 2;
            final int halfWidth = width / 2;

            // Calculate the largest inSampleSize value that is a power of 2 and keeps both
            // height and width larger than the requested height and width.
            while ((halfHeight / inSampleSize) > reqHeight
                    && (halfWidth / inSampleSize) > reqWidth) {
                inSampleSize *= 2;
            }
        }

        return inSampleSize;
    }
}