package vsd.co.za.sambugapp.DomainModels;

import java.util.Date;
import java.util.HashSet;

/**
 * Created by Aeolus on 2015-07-13.
 */
public class Role {
    public int RoleID;
    public String RoleDescription;
    public int LastModifiedID;
    public Date TMStamp;

    public Role() {
        this.Users = new HashSet<User>();
    }

    public int getRoleID() {
        return RoleID;
    }

    public void setRoleID(int roleID) {
        RoleID = roleID;
    }

    public String getRoleDescription() {
        return RoleDescription;
    }

    public void setRoleDescription(String roleDescription) {
        RoleDescription = roleDescription;
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

    public HashSet<User> getUsers() {
        return Users;
    }

    public void setUsers(HashSet<User> users) {
        Users = users;
    }

    public HashSet<User> Users;
}
