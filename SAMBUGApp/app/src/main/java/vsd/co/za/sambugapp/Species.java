package vsd.co.za.sambugapp;

import android.graphics.Bitmap;

/**
 * Created by keaganthompson on 7/8/15.
 */
public class Species {
    private Bitmap fieldPic;
    private boolean isPest;
    private String speciesName;
    private int lifestage;

    public Bitmap getFieldPic() {
        return fieldPic;
    }

    public boolean isPest() {
        return isPest;
    }

    public void setIsPest(boolean isPest) {
        this.isPest = isPest;
    }

    public void setSpeciesName(String speciesName) {
        this.speciesName = speciesName;
    }

    public String getSpeciesName() {
        return speciesName;
    }


    public int getLifestage() {
        return lifestage;
    }

    public void setFieldPic(Bitmap fieldPic) {
        this.fieldPic = fieldPic;
    }


    public void setLifestage(int lifestage) {
        this.lifestage = lifestage;
    }


}
