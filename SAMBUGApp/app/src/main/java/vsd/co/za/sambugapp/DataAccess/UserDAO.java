package vsd.co.za.sambugapp.DataAccess;

import android.content.ContentValues;
import android.content.Context;

import java.util.List;

import vsd.co.za.sambugapp.DomainModels.User;

/**
 * Created by Aeolus on 2015-07-26.
 */
public class UserDAO extends DataSourceAdapter {
    //TODO: Implement fully
    public UserDAO(Context context) {
        super(context);
        allColumns = new String[]{
                DBHelper.COLUMN_USER_ID,
                DBHelper.COLUMN_ROLE_ID,
                DBHelper.COLUMN_EMAIL,
                DBHelper.COLUMN_PASSWORD,
                DBHelper.COLUMN_LAST_MODIFIED_ID,
                DBHelper.COLUMN_TIMESTAMP
        };
    }
        public long insert(User user){
        ContentValues values = new ContentValues();
        values.put(DBHelper.COLUMN_EMAIL,user.getEmail());


        return database.insert(DBHelper.TABLE_USER,null,values);
        }

    public List<User> getAllUsers(){
        return null;
    }

    public boolean isEmpty(){
        return true;
    }

    public void delete(User user){

    }

    public void clearTable(){

    }

    public User getUserByID(int id){
        return null;
    }
}

