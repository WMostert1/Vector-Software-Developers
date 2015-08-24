package vsd.co.za.sambugapp.DomainModels;

import java.io.Serializable;
import java.util.Date;

/**
 * Created by Kale-ab on 2015-07-09.
 *
 */
public class ScoutBug implements Serializable{
    public int ScoutBugID;
    public int ScoutStopID;
    public int SpeciesID;
    public int NumberOfBugs;
    public byte[] FieldPicture;
    public String Comments;
    public int LastModifiedID;
    public Date TMStamp;

    public ScoutStop ScoutStop;
    public Species Species;

    public int getScoutBugID() {
        return ScoutBugID;
    }

    public void setScoutBugID(int scoutBugID) {
        ScoutBugID = scoutBugID;
    }

    public int getScoutStopID() {
        return ScoutStopID;
    }

    public void setScoutStopID(int scoutStopID) {
        ScoutStopID = scoutStopID;
    }

    public int getSpeciesID() {
        return SpeciesID;
    }

    public void setSpeciesID(int speciesID) {
        SpeciesID = speciesID;
    }

    public int getNumberOfBugs() {
        return NumberOfBugs;
    }

    public void setNumberOfBugs(int numberOfBugs) {
        NumberOfBugs = numberOfBugs;
    }

    public byte[] getFieldPicture() {
        return FieldPicture;
    }

    public void setFieldPicture(byte[] fieldPicture) {
        FieldPicture = fieldPicture;
    }

    public String getComments() {
        return Comments;
    }

    public void setComments(String comments) {
        Comments = comments;
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

    public vsd.co.za.sambugapp.DomainModels.ScoutStop getScoutStop() {
        return ScoutStop;
    }

    public void setScoutStop(vsd.co.za.sambugapp.DomainModels.ScoutStop scoutStop) {
        ScoutStop = scoutStop;
    }

    public vsd.co.za.sambugapp.DomainModels.Species getSpecies() {
        return Species;
    }

    public void setSpecies(vsd.co.za.sambugapp.DomainModels.Species species) {
        Species = species;
    }
}
