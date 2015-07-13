package vsd.co.za.sambugapp.DataAccess;

import android.content.Context;
import android.database.DatabaseErrorHandler;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.support.v7.app.AppCompatActivity;


import java.util.ArrayList;
import java.util.List;

import vsd.co.za.sambugapp.R;


/**
 * Created by Aeolus on 2015-07-10.
 */
public class DBHelper<T> extends SQLiteOpenHelper {
    public static final String COLUMN_LAST_MODIFIED_ID = "LastModifiedID";
    public static final String COLUMN_TIMESTAMP = "TMStamp";

    public static final String TABLE_BLOCK = "Block";
    public static final String COLUMN_BLOCK_ID = "BlockID";
    public static final String COLUMN_BLOCK_NAME = "BlockName";

    public static final String TABLE_FARM = "Farm";
    public static final String COLUMN_FARM_ID = "FarmID";
    public static final String COLUMN_FARM_NAME = "FarmName";

    public static final String TABLE_USER = "User";
    public static final String COLUMN_USER_ID = "UserID";
    public static final String COLUMN_EMAIL = "Email";
    public static final String COLUMN_PASSWORD = "Password";

    public static final String TABLE_ROLE = "Role";
    public static final String COLUMN_ROLE_ID = "RoleID";
    public static final String COLUMN_ROLE_DESCRIPTION = "RoleDescription";

    public static final String TABLE_SCOUT_BUG = "ScoutBug";
    public static final String COLUMN_SCOUT_BUG_ID = "ScoutBugID";
    public static final String COLUMN_NUMBER_OF_BUGS = "NumberOfBugs";
    public static final String COLUMN_FIELD_PICTURE = "FieldPicture";
    public static final String COLUMN_COMMENTS = "Comments";

    public static final String TABLE_SCOUT_STOP = "ScoutStop";
    public static final String COLUMN_SCOUT_STOP_ID = "ScoutStopID";
    public static final String COLUMN_NUMBER_OF_TREES = "NumberOfTrees";
    public static final String COLUMN_LATITUDE = "Latitude";
    public static final String COLUMN_LONGITUDE = "Longitude";
    public static final String COLUMN_DATE = "Date";

    public static final String TABLE_SPECIES = "Species";
    public static final String COLUMN_SPECIES_ID = "SpeciesID";
    public static final String COLUMN_SPECIES_NAME = "SpeciesName";
    public static final String COLUMN_LIFESTAGE = "Lifestage";
    public static final String COLUMN_IDEAL_PICTURE = "IdealPicture";
    public static final String COLUMN_IS_PEST = "IsPest";

    public static final String TABLE_TREATMENT = "Treatment";
    public static final String COLUMN_TREATMENT_ID = "TreatmentID";

    //Please increment the counter whenever you edit the database structure
    public static final int DATABASE_VERSION = 1;
    public static String DATABASE_NAME = "BugMobileDB.db";
    private ArrayList<String> sqlScripts;

    @Override
    public void onCreate(SQLiteDatabase db) {
        for (String script : sqlScripts)
            db.execSQL(script);
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
