package vsd.co.za.sambugapp.DomainModels;

import java.io.Serializable;
import java.security.SecureRandom;
import java.util.Date;
import java.util.HashSet;
import java.util.List;

/**
 * Created by Aeolus on 2015-07-13.
 */
public class Block implements Serializable{
    public Block() {
        this.ScoutStops = new HashSet<ScoutStop>();
        this.Treatments = new HashSet<Treatment>();
    }

    public int BlockID;
    public int FarmID;
    public String BlockName;

    public Farm Farm;
    public HashSet<ScoutStop> ScoutStops;
    public HashSet<Treatment> Treatments;

    public void setBlockID(int blockID) {
        BlockID = blockID;
    }

    public void setFarmID(int farmID) {
        FarmID = farmID;
    }

    public void setBlockName(String blockName) {
        BlockName = blockName;
    }

    public void setFarm(Farm farm) {
        this.Farm = farm;
    }

    public void setScoutStops(HashSet<ScoutStop> scoutStops) {
        ScoutStops = scoutStops;
    }

    public void setTreatments(HashSet<Treatment> treatments) {
        Treatments = treatments;
    }

    public int getBlockID() {
        return BlockID;
    }

    public int getFarmID() {
        return FarmID;
    }

    public String getBlockName() {
        return BlockName;
    }

    public Farm getFarm() {
        return Farm;
    }

    public HashSet<ScoutStop> getScoutStops() {
        return ScoutStops;
    }

    public HashSet<Treatment> getTreatments() {
        return Treatments;
    }

    @Override
    public String toString(){
        return getBlockName();
    }
}
