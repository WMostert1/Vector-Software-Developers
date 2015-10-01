package vsd.co.za.sambugapp.CameraProcessing;

import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.ImageView;
import android.widget.RelativeLayout;

import vsd.co.za.sambugapp.DomainModels.Farm;
import vsd.co.za.sambugapp.LoginActivity;
import vsd.co.za.sambugapp.R;

public class ImagePreview extends AppCompatActivity {
    byte [] image;
    ImageView imageView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_image_preview);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_image_preview, menu);
        acceptImage(getIntent());
        imageView = (ImageView)findViewById(R.id.ivImage);
       // setContentView(R.layout.);
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

    private void acceptImage(Intent intent){
        Bundle b=intent.getExtras();
        image= (byte[])b.get(Camera.CAMERA);
        Bitmap bmp = BitmapFactory.decodeByteArray(image, 0, image.length);
        imageView = (ImageView)findViewById(R.id.ivImage);
        imageView.setImageBitmap(bmp);

    }
}
