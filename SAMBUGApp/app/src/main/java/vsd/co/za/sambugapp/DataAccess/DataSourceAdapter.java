package vsd.co.za.sambugapp.DataAccess;

import android.content.Context;
import android.database.sqlite.SQLiteDatabase;

import java.sql.SQLException;


/**
 * Created by Aeolus on 2015-07-13.
 */
public class DataSourceAdapter {
    protected SQLiteDatabase database;
    protected DBHelper dbHelper;
    protected String[] allColumns;
    protected Context context;

    public DataSourceAdapter(Context context) {
        dbHelper = new DBHelper(context);
        this.context = context;
    }

    public void open() throws SQLException {
        database = dbHelper.getWritableDatabase();
    }

    public void close() {
        dbHelper.close();
    }
}
