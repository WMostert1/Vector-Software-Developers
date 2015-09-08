package vsd.co.za.sambugapp.DomainModels;

import java.io.Serializable;
import java.util.Date;
import java.util.HashSet;

/**
 * Created by Aeolus on 2015-07-13.
 */
public class User implements Serializable {
    public User() {
        this.Farms = new HashSet<Farm>();

    }

    public int UserID;
    public String Email;

    public HashSet<Farm> Farms;
    public HashSet<Role> Roles;


    public int getUserID() {
        return UserID;
    }

    public void setUserID(int userID) {
        UserID = userID;
    }

    public String getEmail() {
        return Email;
    }

    public void setEmail(String email) {
        Email = email;
    }

    public HashSet<Farm> getFarms() {
        return Farms;
    }

    public void setFarms(HashSet<Farm> farms) {
        Farms = farms;
    }

    public HashSet<Role> getRoles() {
        return Roles;
    }

    public void setRoles(HashSet<Role> roles) {
        Roles = roles;
    }

}
