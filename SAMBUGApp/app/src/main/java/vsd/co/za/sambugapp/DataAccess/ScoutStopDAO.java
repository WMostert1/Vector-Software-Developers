package vsd.co.za.sambugapp.DataAccess;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;

import java.sql.SQLException;
import java.text.SimpleDateFormat;
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
            DBHelper.COLUMN_BLOCK_ID,
            DBHelper.COLUMN_NUMBER_OF_TREES,
            DBHelper.COLUMN_LATITUDE,
            DBHelper.COLUMN_LONGITUDE,
            DBHelper.COLUMN_DATE
        };
    }

    public void delete(ScoutStop scoutStop) {
        int id = scoutStop.getScoutStopID();
        database.delete(DBHelper.TABLE_SCOUT_STOP, DBHelper.COLUMN_SCOUT_STOP_ID + " = " + id, null);
    }

    public void update(ScoutStop scoutStop) {
        int id = scoutStop.getScoutStopID();
        ContentValues values = new ContentValues();
        values.put(DBHelper.COLUMN_BLOCK_ID,scoutStop.getBlockID());
        values.put(DBHelper.COLUMN_NUMBER_OF_TREES, scoutStop.getNumberOfTrees());
        values.put(DBHelper.COLUMN_LATITUDE, scoutStop.getLatitude());
        values.put(DBHelper.COLUMN_LONGITUDE, scoutStop.getLongitude());
        values.put(DBHelper.COLUMN_DATE, scoutStop.getDate().toString());
        database.update(DBHelper.TABLE_SCOUT_STOP, values, DBHelper.COLUMN_SCOUT_STOP_ID + " = " + id, null);
    }

    public long insert(ScoutStop scoutStop) throws SQLException{
        ContentValues values = new ContentValues();
        values.put(DBHelper.COLUMN_BLOCK_ID,scoutStop.getBlockID());
        values.put(DBHelper.COLUMN_NUMBER_OF_TREES, scoutStop.getNumberOfTrees());
        values.put(DBHelper.COLUMN_LATITUDE, scoutStop.getLatitude());
        values.put(DBHelper.COLUMN_LONGITUDE, scoutStop.getLongitude());
        values.put(DBHelper.COLUMN_DATE, scoutStop.getDate().toString());
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
        scoutStop.setBlockID(cursor.getInt(1));
        scoutStop.setNumberOfTrees(cursor.getInt(2));
        scoutStop.setLatitude(cursor.getFloat(3));
        scoutStop.setLongitude(cursor.getFloat(4));
        try {
//            String strDate = cursor.getString(5);
//            Log.e("DAO", strDate);
//            String inputPattern = "EEE MMM d HH:mm:ss zzz yyyy";
//
//            String outputPattern = "dd-MM-yyyy";
//
//            SimpleDateFormat inputFormat = new SimpleDateFormat(inputPattern);
//            SimpleDateFormat outputFormat = new SimpleDateFormat(outputPattern);
//
//            Date date = null;
//            String str = null;
//
//            try {
//                date = inputFormat.parse(strDate);
//                str = outputFormat.format(date);
//
//                Log.i("mini", "Converted Date Today:" + str);
//            } catch (ParseException e) {
//                e.printStackTrace();
//            }

           // scoutStop.setDate(outputFormat.parse(str));
//            SimpleDateFormat sdf1 = new SimpleDateFormat("dd-mm-yyyy");
//            scoutStop.setDate(sdf1.parse(cursor.getString(5)));
            String startDate=cursor.getString(5);
            SimpleDateFormat sdf1 = new SimpleDateFormat("yyyy-MM-dd");
            java.util.Date date = sdf1.parse(startDate);
            java.sql.Date sqlStartDate = new java.sql.Date(date.getTime());
            scoutStop.setDate(sqlStartDate);

        } catch(Exception k){

        }

        return scoutStop;
    }
    
}