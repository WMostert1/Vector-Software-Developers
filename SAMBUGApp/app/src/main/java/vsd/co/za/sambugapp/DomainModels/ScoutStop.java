package vsd.co.za.sambugapp.DomainModels;

import java.io.Serializable;
import java.util.Date;
import java.util.HashSet;

/**
 * Created by keaganthompson on 7/8/15.
 */
public class ScoutStop implements Serializable{

    public int ScoutStopID;
    public int UserID;
    public int BlockID;
    public int NumberOfTrees;
    public float Latitude;
    public float Longitude;

    public java.util.Date Date;
    public Integer LastModifiedID;
    public Date TMStamp;

    public Block Block;
    public HashSet<ScoutBug> ScoutBugs;
    public User User;

    public ScoutStop() {
        NumberOfTrees=0;
        Latitude=0;
        Longitude=0;
        Block.setBlockName("Piesang");
    }

    public double getPestsPerTree(){
        double average=0;
        for (ScoutBug bug : ScoutBugs){
            if (bug.getSpecies().isPest()){
                average+=bug.getNumberOfBugs();
            }
        }
        average/=getNumberOfTrees();
        return average;
    }

    public int getScoutStopID() {
        return ScoutStopID;
    }

    public void setScoutStopID(int scoutStopID) {
        ScoutStopID = scoutStopID;
    }

    public int getUserID() {
        return UserID;
    }

    public void setUserID(int userID) {
        UserID = userID;
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

    public void setLatitude(float latitude) {
        Latitude = latitude;
    }

    public float getLongitude() {
        return Longitude;
    }

    public void setLongitude(float longitude) {
        Longitude = longitude;
    }

    public java.util.Date getDate() {
        return Date;
    }

    public void setDate(java.util.Date date) {
        Date = date;
    }

    public Integer getLastModifiedID() {
        return LastModifiedID;
    }

    public void setLastModifiedID(Integer lastModifiedID) {
        LastModifiedID = lastModifiedID;
    }

    public java.util.Date getTMStamp() {
        return TMStamp;
    }

    public void setTMStamp(java.util.Date TMStamp) {
        this.TMStamp = TMStamp;
    }

    public vsd.co.za.sambugapp.DomainModels.Block getBlock() {
        return Block;
    }

    public void setBlock(vsd.co.za.sambugapp.DomainModels.Block block) {
        Block = block;
    }

    public HashSet<ScoutBug> getScoutBugs() {
        return ScoutBugs;
    }

    public void setScoutBugs(HashSet<ScoutBug> scoutBugs) {
        ScoutBugs = scoutBugs;
    }

    public vsd.co.za.sambugapp.DomainModels.User getUser() {
        return User;
    }

    public void setUser(vsd.co.za.sambugapp.DomainModels.User user) {
        User = user;
    }
}
