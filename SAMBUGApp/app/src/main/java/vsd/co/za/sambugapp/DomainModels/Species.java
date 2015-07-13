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
    public int IdealPicture;
    public boolean IsPest;
    public Integer LastModifiedID;
    public Date TMStamp;

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

    public int getIdealPicture() {
        return IdealPicture;
    }

    public void setIdealPicture(int idealPicture) {
        IdealPicture = idealPicture;
    }

    public boolean isPest() {
        return IsPest;
    }

    public void setIsPest(boolean isPest) {
        IsPest = isPest;
    }

    public Integer getLastModifiedID() {
        return LastModifiedID;
    }

    public void setLastModifiedID(Integer lastModifiedID) {
        LastModifiedID = lastModifiedID;
    }

    public Date getTMStamp() {
        return TMStamp;
    }

    public void setTMStamp(Date TMStamp) {
        this.TMStamp = TMStamp;
    }

    public HashSet<ScoutBug> getScoutBugs() {
        return ScoutBugs;
    }

    public void setScoutBugs(HashSet<ScoutBug> scoutBugs) {
        ScoutBugs = scoutBugs;
    }
}
