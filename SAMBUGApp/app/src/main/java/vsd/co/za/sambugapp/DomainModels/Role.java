package vsd.co.za.sambugapp.DomainModels;

import java.util.Date;
import java.util.HashSet;

/**
 * Created by Aeolus on 2015-07-13.
 */
public class Role {
    public int RoleID;
    public int Type;
    public String Description;

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
        return Description;
    }

    public void setRoleDescription(String roleDescription) {
        Description = roleDescription;
    }

    public HashSet<User> getUsers() {
        return Users;
    }

    public void setUsers(HashSet<User> users) {
        Users = users;
    }

    public HashSet<User> Users;
}
