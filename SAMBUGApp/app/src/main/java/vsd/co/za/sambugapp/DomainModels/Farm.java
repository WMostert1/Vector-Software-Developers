package vsd.co.za.sambugapp.DomainModels;

import java.io.Serializable;
import java.util.HashSet;

/**
 * Created by Aeolus on 2015-07-13.
 *
 */
public class Farm implements Serializable {
    public int FarmID;
    public int UserID;
    public String FarmName;

    public HashSet<Block> Blocks;
    public User User;
    
    public Farm() {
        this.Blocks = new HashSet<Block>();
    }

    public int getFarmID() {
        return FarmID;
    }

    public void setFarmID(int farmID) {
        FarmID = farmID;
    }

    public int getUserID() {
        return UserID;
    }

    public void setUserID(int userID) {
        UserID = userID;
    }

    public String getFarmName() {
        return FarmName;
    }

    public void setFarmName(String farmName) {
        FarmName = farmName;
    }

    public HashSet<Block> getBlocks() {
        return Blocks;
    }

    public void setBlocks(HashSet<Block> blocks) {
        Blocks = blocks;
    }

    public vsd.co.za.sambugapp.DomainModels.User getUser() {
        return User;
    }

    public void setUser(vsd.co.za.sambugapp.DomainModels.User user) {
        User = user;
    }

    public String toString() {
        return getFarmName();
    }

}
