package vsd.co.za.sambugapp;

import android.graphics.drawable.Drawable;

import java.io.Serializable;

/**
 * Created by Kale-ab on 2015-07-10.
 */
public class CapturedImage implements Serializable{
    public Drawable getImage() {
        return image;
    }

    public void setImage(Drawable image) {
        this.image = image;
    }

    Drawable image;
}
