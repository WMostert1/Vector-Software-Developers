package vsd.co.za.sambugapp.DomainModels;

import java.io.Serializable;
import java.util.Date;
import java.util.HashSet;

/**
 * Created by Aeolus on 2015-07-13.
 */
public class User implements Serializable{
    public User() {
        this.Farms = new HashSet<Farm>();
        this.ScoutStops = new HashSet<ScoutStop>();
    }

    public int UserID;
    public int RoleID;
    public String Email;
    public String Password;
    public Integer LastModifiedID;
    public Date TMStamp;

    public HashSet<Farm> Farms;
    public Role Role;
    public HashSet<ScoutStop> ScoutStops;

    public int getUserID() {
        return UserID;
    }

    public void setUserID(int userID) {
        UserID = userID;
    }

    public int getRoleID() {
        return RoleID;
    }

    public void setRoleID(int roleID) {
        RoleID = roleID;
    }

    public String getEmail() {
        return Email;
    }

    public void setEmail(String email) {
        Email = email;
    }

    public String getPassword() {
        return Password;
    }

    public void setPassword(String password) {
        Password = password;
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

    public HashSet<Farm> getFarms() {
        return Farms;
    }

    public void setFarms(HashSet<Farm> farms) {
        Farms = farms;
    }

    public vsd.co.za.sambugapp.DomainModels.Role getRole() {
        return Role;
    }

    public void setRole(vsd.co.za.sambugapp.DomainModels.Role role) {
        Role = role;
    }

    public HashSet<ScoutStop> getScoutStops() {
        return ScoutStops;
    }

    public void setScoutStops(HashSet<ScoutStop> scoutStops) {
        ScoutStops = scoutStops;
    }
}
