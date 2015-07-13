package vsd.co.za.sambugapp.DomainModels;

import java.util.Date;

/**
 * Created by Aeolus on 2015-07-13.
 */
public class Treatment {
    public int TreatmentID;
    public int BlockID;
    public Date Date;
    public String Comments;
    public Integer LastModifiedID;
    public Date TMStamp;

    public Block Block;

    public int getTreatmentID() {
        return TreatmentID;
    }

    public void setTreatmentID(int treatmentID) {
        TreatmentID = treatmentID;
    }

    public int getBlockID() {
        return BlockID;
    }

    public void setBlockID(int blockID) {
        BlockID = blockID;
    }

    public java.util.Date getDate() {
        return Date;
    }

    public void setDate(java.util.Date date) {
        Date = date;
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
}
