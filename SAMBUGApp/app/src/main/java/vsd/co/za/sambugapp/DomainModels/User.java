package vsd.co.za.sambugapp.DomainModels;

import java.io.Serializable;
import java.util.HashSet;

/**
 * Created by Aeolus on 2015-07-13.
 *
 */
public class User implements Serializable {
    public User() {
        this.Farms = new HashSet<>();
    }

    public int UserId;
    public HashSet<Farm> Farms;


    public int getUserID() {
        return UserId;
    }

    public void setUserID(int userID) {
        UserId = userID;
    }

    public HashSet<Farm> getFarms() {
        return Farms;
    }

    public void setFarms(HashSet<Farm> farms) {
        Farms = farms;
    }


}
