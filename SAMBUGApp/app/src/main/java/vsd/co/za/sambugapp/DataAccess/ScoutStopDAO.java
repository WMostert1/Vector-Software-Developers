package vsd.co.za.sambugapp.DataAccess;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.util.Log;

import java.sql.SQLException;
import java.text.DateFormat;
import java.text.ParseException;
import java.util.ArrayList;
import java.util.List;

import vsd.co.za.sambugapp.DomainModels.ScoutBug;
import vsd.co.za.sambugapp.DomainModels.ScoutStop;

/**
 * Created by keaganthompson on 7/14/15.
 */
public class ScoutStopDAO extends DataSourceAdapter{

    public ScoutStopDAO(Context context) {
        super(context);
        allColumns = new String[]{
            DBHelper.COLUMN_SCOUT_STOP_ID,
            DBHelper.COLUMN_USER_ID,
            DBHelper.COLUMN_BLOCK_ID,
            DBHelper.COLUMN_NUMBER_OF_TREES,
            DBHelper.COLUMN_LATITUDE,
            DBHelper.COLUMN_LONGITUDE,
            DBHelper.COLUMN_DATE,
            DBHelper.COLUMN_LAST_MODIFIED_ID,
            DBHelper.COLUMN_TIMESTAMP
        };
    }

    public void delete(ScoutStop scoutStop) {
        int id = scoutStop.getScoutStopID();
        database.delete(DBHelper.TABLE_SCOUT_STOP, DBHelper.COLUMN_SCOUT_STOP_ID + " = " + id, null);
    }

    public void update(ScoutStop scoutStop) {
        int id = scoutStop.getScoutStopID();
        ContentValues values = new ContentValues();
        values.put(DBHelper.COLUMN_USER_ID,scoutStop.getUserID());
        values.put(DBHelper.COLUMN_BLOCK_ID,scoutStop.getBlockID());
        values.put(DBHelper.COLUMN_NUMBER_OF_TREES, scoutStop.getNumberOfTrees());
        values.put(DBHelper.COLUMN_LATITUDE, scoutStop.getLatitude());
        values.put(DBHelper.COLUMN_LONGITUDE, scoutStop.getLongitude());
        values.put(DBHelper.COLUMN_DATE, scoutStop.getDate().toString());
        values.put(DBHelper.COLUMN_LAST_MODIFIED_ID, scoutStop.getLastModifiedID());
        values.put(DBHelper.COLUMN_TIMESTAMP, scoutStop.getTMStamp().toString());
        database.update(DBHelper.TABLE_SCOUT_STOP, values, DBHelper.COLUMN_SCOUT_STOP_ID + " = " + id, null);
    }

    public long insert(ScoutStop scoutStop) throws SQLException{
        ContentValues values = new ContentValues();
        values.put(DBHelper.COLUMN_USER_ID, scoutStop.getUserID());
        values.put(DBHelper.COLUMN_BLOCK_ID,scoutStop.getBlockID());
        values.put(DBHelper.COLUMN_NUMBER_OF_TREES, scoutStop.getNumberOfTrees());
        values.put(DBHelper.COLUMN_LATITUDE, scoutStop.getLatitude());
        values.put(DBHelper.COLUMN_LONGITUDE, scoutStop.getLongitude());
        values.put(DBHelper.COLUMN_DATE, scoutStop.getDate().toString());
        values.put(DBHelper.COLUMN_LAST_MODIFIED_ID, scoutStop.getLastModifiedID());
        values.put(DBHelper.COLUMN_TIMESTAMP, scoutStop.getTMStamp().toString());
        long scoutStopID = database.insert(DBHelper.TABLE_SCOUT_STOP, null, values);

            ScoutBugDAO scoutBugDAO = new ScoutBugDAO(context);
            scoutBugDAO.open();
                for(ScoutBug bug: scoutStop.getScoutBugs()){
                    bug.setScoutStopID((int)scoutStopID);
                    scoutBugDAO.insert(bug);
                }
            scoutBugDAO.close();

      return scoutStopID;
    }

    public List<ScoutStop> getAllScoutStops() {
        List<ScoutStop> scoutStopList = new ArrayList<>();
        Cursor cursor = database.query(DBHelper.TABLE_SCOUT_STOP, allColumns, null, null, null, null, null);
        //String [] args = {DBHelper.TABLE_SCOUT_STOP,DBHelper.TABLE_SCOUT_BUG,DBHelper.COLUMN_SCOUT_STOP_ID,DBHelper.COLUMN_SCOUT_STOP_ID};
        //Cursor cursor = database.rawQuery("SELECT * FROM ? a INNER JOIN ? b ON a. INNER JOIN ? c",null);
        cursor.moveToFirst();
        while (!cursor.isAfterLast()) {
            ScoutStop scoutStop = cursorToScoutStop(cursor);
            scoutStopList.add(scoutStop);
            cursor.moveToNext();
        }
        //Remember to close the cursor
        cursor.close();
        return scoutStopList;
    }

    public void clearTable() {
        database.delete(DBHelper.TABLE_SCOUT_STOP, null, null);
    }

    public ScoutStop getScoutStopByID(int id) {
        Cursor cursor = database.query(DBHelper.TABLE_SCOUT_STOP, allColumns, DBHelper.COLUMN_SCOUT_STOP_ID + " = " + id, null, null, null, null);
        cursor.moveToFirst();
        if (cursor.isAfterLast()) {
            cursor.close();
            return null;
        }
        ScoutStop scoutStop = cursorToScoutStop(cursor);

        //Remember to close the cursor
        cursor.close();
        return scoutStop;
    }

    public boolean isEmpty() {
        Cursor cursor = database.query(DBHelper.TABLE_SCOUT_STOP, allColumns, null, null, null, null, null);
        cursor.moveToFirst();

        boolean result = cursor.isAfterLast();
        cursor.close();
        return result;
    }

    public ScoutStop cursorToScoutStop(Cursor cursor) {
        ScoutStop scoutStop = new ScoutStop();
        scoutStop.setScoutStopID(cursor.getInt(0));
        scoutStop.setUserID(cursor.getInt(1));
        scoutStop.setBlockID(cursor.getInt(2));
        scoutStop.setNumberOfTrees(cursor.getInt(3));
        scoutStop.setLatitude(cursor.getInt(4));
        scoutStop.setLongitude(cursor.getInt(5));
        try {
            String date = cursor.getString(6);
            Log.e("DAO", date);
            scoutStop.setDate(DateFormat.getDateTimeInstance().parse(date));
        } catch (ParseException e) {
            e.printStackTrace();
            scoutStop.setDate(null);
        }
        scoutStop.setLastModifiedID(cursor.getInt(7));
        String date = cursor.getString(8);
        try {
            //TODO: Get this bloody thing to parse the date correctly
            scoutStop.setTMStamp(DateFormat.getDateTimeInstance().parse(date));
        } catch (ParseException e) {
            e.printStackTrace();
            scoutStop.setTMStamp(null);
        }
        return scoutStop;
    }
    
}