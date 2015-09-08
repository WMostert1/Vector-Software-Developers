package vsd.co.za.sambugapp.DomainModels;

import android.graphics.Bitmap;

import java.io.Serializable;
import java.util.Date;
import java.util.HashSet;

/**
 * Created by keaganthompson on 7/8/15.
 */
public class Species implements Serializable {
    public Species() {
        this.ScoutBugs = new HashSet<ScoutBug>();
    }

    public int SpeciesID;
    public String SpeciesName;
    public int Lifestage;
    public byte[] IdealPicture;
    public boolean IsPest;

    public HashSet<ScoutBug> ScoutBugs;

    public int getSpeciesID() {
        return SpeciesID;
    }

    public void setSpeciesID(int speciesID) {
        SpeciesID = speciesID;
    }

    public String getSpeciesName() {
        return SpeciesName;
    }

    public void setSpeciesName(String speciesName) {
        SpeciesName = speciesName;
    }

    public int getLifestage() {
        return Lifestage;
    }

    public void setLifestage(int lifestage) {
        Lifestage = lifestage;
    }

    public byte[] getIdealPicture() {
        return IdealPicture;
    }

    public void setIdealPicture(byte[] idealPicture) {
        IdealPicture = idealPicture;
    }

    public boolean isPest() {
        return IsPest;
    }

    public void setIsPest(boolean isPest) {
        IsPest = isPest;
    }


    public HashSet<ScoutBug> getScoutBugs() {
        return ScoutBugs;
    }

    public void setScoutBugs(HashSet<ScoutBug> scoutBugs) {
        ScoutBugs = scoutBugs;
    }
}
