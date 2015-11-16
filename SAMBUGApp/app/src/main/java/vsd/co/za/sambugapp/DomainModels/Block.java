package vsd.co.za.sambugapp.DomainModels;

import java.io.Serializable;

/**
 * Created by Aeolus on 2015-07-13.
 */
public class Block implements Serializable{

    public int BlockID;
    public int FarmID;
    public String BlockName;

    public Farm Farm;

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

    @Override
    public String toString(){
        return getBlockName();
    }
}
