package vsd.co.za.sambugapp;

import android.content.Context;
import android.database.DatabaseErrorHandler;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.support.v7.app.AppCompatActivity;


import java.util.ArrayList;


/**
 * Created by Aeolus on 2015-07-10.
 */
public class DBHelper extends SQLiteOpenHelper {
    //Please increment the counter whenever you edit the database structure
    public static final int DATABASE_VERSION = 1;
    public static String DATABASE_NAME = "BugMobileDB.db";
    private ArrayList<String> sqlScripts;

    @Override
    public void onCreate(SQLiteDatabase db) {
        for (String script : sqlScripts)
            db.execSQL(script);

    }

    public void showDB() {

    }

    public DBHelper(Context context) {
        super(context, DATABASE_NAME, null, DATABASE_VERSION);
        sqlScripts = new ArrayList<String>();
        sqlScripts.add(context.getResources().getString(R.string.create_block));
        sqlScripts.add(context.getResources().getString(R.string.create_farm));
        sqlScripts.add(context.getResources().getString(R.string.create_role));
        sqlScripts.add(context.getResources().getString(R.string.create_scoutbug));
        sqlScripts.add(context.getResources().getString(R.string.create_scoutstop));
        sqlScripts.add(context.getResources().getString(R.string.create_species));
        sqlScripts.add(context.getResources().getString(R.string.create_treatment));
        sqlScripts.add(context.getResources().getString(R.string.create_user));

    }

    @Override

    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        //TODO: Add upgrade policy
        onCreate(db);
    }
}
