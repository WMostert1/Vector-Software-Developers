package vsd.co.za.sambugapp.DataAccess;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.util.Log;

import java.text.DateFormat;
import java.text.ParseException;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import vsd.co.za.sambugapp.DomainModels.ScoutBug;

/**
 * Created by keaganthompson on 7/14/15.
 */
public class ScoutBugDAO extends DataSourceAdapter {

    public ScoutBugDAO(Context context) {
        super(context);
        allColumns = new String[]{
                DBHelper.COLUMN_SCOUT_BUG_ID,
                DBHelper.COLUMN_SCOUT_STOP_ID,
                DBHelper.COLUMN_SPECIES_ID,
                DBHelper.COLUMN_NUMBER_OF_BUGS,
                DBHelper.COLUMN_FIELD_PICTURE,
                DBHelper.COLUMN_COMMENTS,
                DBHelper.COLUMN_LAST_MODIFIED_ID,
                DBHelper.COLUMN_TIMESTAMP
        };
    }

    public void delete(ScoutBug scoutBug) {
        int id = scoutBug.getScoutBugID();
        database.delete(DBHelper.TABLE_SCOUT_BUG, DBHelper.COLUMN_SCOUT_BUG_ID + " = " + id, null);
    }

    public void update(ScoutBug scoutBug) {
        int id = scoutBug.getScoutBugID();
        ContentValues values = new ContentValues();
        values.put(DBHelper.COLUMN_SCOUT_STOP_ID,scoutBug.getScoutStopID());
        values.put(DBHelper.COLUMN_SPECIES_ID, scoutBug.getSpeciesID());
        values.put(DBHelper.COLUMN_NUMBER_OF_BUGS, scoutBug.getNumberOfBugs());
        values.put(DBHelper.COLUMN_FIELD_PICTURE, scoutBug.getFieldPicture());
        values.put(DBHelper.COLUMN_COMMENTS, scoutBug.getComments());
        database.update(DBHelper.TABLE_SCOUT_BUG, values, DBHelper.COLUMN_SCOUT_BUG_ID + " = " + id, null);
    }

    public long insert(ScoutBug scoutBug) {
        ContentValues values = new ContentValues();
        values.put(DBHelper.COLUMN_SCOUT_STOP_ID,scoutBug.getScoutStopID());
        values.put(DBHelper.COLUMN_SPECIES_ID, scoutBug.getSpeciesID());
        values.put(DBHelper.COLUMN_NUMBER_OF_BUGS, scoutBug.getNumberOfBugs());
        values.put(DBHelper.COLUMN_FIELD_PICTURE, scoutBug.getFieldPicture());
        values.put(DBHelper.COLUMN_COMMENTS, scoutBug.getComments());
        return database.insert(DBHelper.TABLE_SCOUT_BUG, null, values);
    }

    public List<ScoutBug> getAllScoutBugs() {
        List<ScoutBug> scoutBugList = new ArrayList<>();
        Cursor cursor = database.query(DBHelper.TABLE_SCOUT_BUG, allColumns, null, null, null, null, null);

        cursor.moveToFirst();
        while (!cursor.isAfterLast()) {
            ScoutBug scoutBug = cursorToScoutBug(cursor);
            scoutBugList.add(scoutBug);
            cursor.moveToNext();
        }
        //Remember to close the cursor
        cursor.close();
        return scoutBugList;
    }

    public void clearTable() {
        database.delete(DBHelper.TABLE_SCOUT_BUG, null, null);
    }

    public ScoutBug getScoutBugByID(int id) {
        Cursor cursor = database.query(DBHelper.TABLE_SCOUT_BUG, allColumns, DBHelper.COLUMN_SCOUT_BUG_ID + " = " + id, null, null, null, null);
        cursor.moveToFirst();
        if (cursor.isAfterLast()) {
            cursor.close();
            return null;
        }
        ScoutBug scoutBug = cursorToScoutBug(cursor);

        //Remember to close the cursor
        cursor.close();
        return scoutBug;
    }

    public boolean isEmpty() {
        Cursor cursor = database.query(DBHelper.TABLE_SCOUT_BUG, allColumns, null, null, null, null, null);
        cursor.moveToFirst();

        boolean result = cursor.isAfterLast();
        cursor.close();
        return result;
    }

    public ScoutBug cursorToScoutBug(Cursor cursor) {
        ScoutBug scoutBug = new ScoutBug();
        scoutBug.setScoutBugID(cursor.getInt(0));
        scoutBug.setScoutStopID(cursor.getInt(1));
        scoutBug.setSpeciesID(cursor.getInt(2));
        scoutBug.setNumberOfBugs(cursor.getInt(3));
        scoutBug.setFieldPicture(cursor.getBlob(4));
        scoutBug.setComments(cursor.getString(5));
        String date = cursor.getString(7);

//        try {
//            //TODO: Get this bloody thing to parse the date correctly
//
//            //scoutBug.setTMStamp(DateFormat.getDateTimeInstance().parse(date));
//        } catch (ParseException e) {
//            e.printStackTrace();
//            scoutBug.setTMStamp(null);
//        }
        return scoutBug;
    }

}
