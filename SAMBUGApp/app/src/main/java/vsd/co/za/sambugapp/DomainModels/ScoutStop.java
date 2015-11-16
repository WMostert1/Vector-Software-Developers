package vsd.co.za.sambugapp.DomainModels;

import java.io.Serializable;
import java.util.ArrayList;

/**
 * Created by keaganthompson on 7/8/15.
 *
 */
public class ScoutStop implements Serializable {

    public int ScoutStopID;
    public int BlockID;
    public int NumberOfTrees;
    public float Latitude;
    public float Longitude;

    public java.sql.Date Date;

    public Block Block;
    public ArrayList<ScoutBug> ScoutBugs;

    public ScoutStop() {
        NumberOfTrees = 0;
        Latitude = 0;
        Longitude = 0;
        Block = new Block();
        ScoutBugs = new ArrayList<>();
    }

    public double getPestsPerTree() {
        double average = 0;
        for (ScoutBug bug : ScoutBugs) {
            if (bug.getSpecies().isPest()) {
                average += bug.getNumberOfBugs();
            }
        }
        average /= getNumberOfTrees();
        return average;
    }

    public int getScoutStopID() {
        return ScoutStopID;
    }

    public void setScoutStopID(int scoutStopID) {
        ScoutStopID = scoutStopID;
    }

    public int getBlockID() {
        return BlockID;
    }

    public void setBlockID(int blockID) {
        BlockID = blockID;
    }

    public int getNumberOfTrees() {
        return NumberOfTrees;
    }

    public void setNumberOfTrees(int numberOfTrees) {
        NumberOfTrees = numberOfTrees;
    }

    public float getLatitude() {
        return Latitude;
    }

    public void setLatitude(String latitude){
        Latitude = Float.valueOf(latitude);
    }

    public void setLongitude(String longitude){
        Longitude = Float.valueOf(longitude);
    }

    public void setLatitude(float latitude) {
        Latitude = latitude;
    }

    public float getLongitude() {
        return Longitude;
    }

    public void setLongitude(float longitude) {
        Longitude = longitude;
    }

    public java.sql.Date getDate() {
        return Date;
    }

    public void setDate(java.sql.Date date) {
        Date = date;
    }

    public vsd.co.za.sambugapp.DomainModels.Block getBlock() {
        return Block;
    }

    public void setBlock(vsd.co.za.sambugapp.DomainModels.Block block) {
        Block = block;
    }

    public ArrayList<ScoutBug> getScoutBugs() {
        return ScoutBugs;
    }

    public void setScoutBugs(ArrayList<ScoutBug> scoutBugs) {
        ScoutBugs = scoutBugs;
    }
}
