package vsd.co.za.sambugapp.DB;

import android.content.Context;
import android.database.DatabaseErrorHandler;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

/**
 * Created by Aeolus on 2015-07-10.
 */
public class DBHelper extends SQLiteOpenHelper {
    //Please increment the counter whenever you edit the database structure
    public static final int DATABASE_VERSION = 1;
    public static String DATABASE_NAME = "BugMobileDB.db";

    @Override
    public void onCreate(SQLiteDatabase db) {
        //TODO: Add scripts
    }

    public DBHelper(Context context) {
        super(context, DATABASE_NAME, null, DATABASE_VERSION);
    }

    @Override

    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        //TODO: Add upgrade policy
    }
}
