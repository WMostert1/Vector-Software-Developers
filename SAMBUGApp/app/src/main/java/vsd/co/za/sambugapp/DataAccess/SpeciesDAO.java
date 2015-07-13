package vsd.co.za.sambugapp.DataAccess;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashSet;
import java.util.List;

import vsd.co.za.sambugapp.DomainModels.Species;


/**
 * Created by Aeolus on 2015-07-13.
 */
public class SpeciesDAO extends DataSourceAdapter {

    public SpeciesDAO(Context context) {
        super(context);
        allColumns = new String[]{
                DBHelper.COLUMN_SPECIES_ID,
                DBHelper.COLUMN_SPECIES_NAME,
                DBHelper.COLUMN_LIFESTAGE,
                DBHelper.COLUMN_IDEAL_PICTURE,
                DBHelper.COLUMN_IS_PEST,
                DBHelper.COLUMN_LAST_MODIFIED_ID,
                DBHelper.COLUMN_TIMESTAMP
        };
    }

    public void insert(Species species) {
        ContentValues values = new ContentValues();
        values.put(DBHelper.COLUMN_SPECIES_NAME, species.getSpeciesName());
        values.put(DBHelper.COLUMN_LIFESTAGE, species.getLifestage());
        values.put(DBHelper.COLUMN_IDEAL_PICTURE, species.getIdealPicture());
        values.put(DBHelper.COLUMN_IS_PEST, species.isPest());
        values.put(DBHelper.COLUMN_LAST_MODIFIED_ID, species.getLastModifiedID());
        values.put(DBHelper.COLUMN_TIMESTAMP, species.getTMStamp().toString());

        database.insert(DBHelper.TABLE_SPECIES, null, values);
    }

    public List<Species> getAllSpecies() {
        List<Species> speciesList = new ArrayList<>();
        Cursor cursor = database.query(DBHelper.TABLE_SPECIES, allColumns, null, null, null, null, null);

        cursor.moveToFirst();
        while (!cursor.isAfterLast()) {
            Species species = cursorToSpecies(cursor);
            speciesList.add(species);
            cursor.moveToNext();
        }
        //Remember to close the cursor
        cursor.close();
        return speciesList;
    }

    public Species cursorToSpecies(Cursor cursor) {
        //TODO: Do the queries propery
        Species species = new Species();
        species.setSpeciesID(cursor.getInt(0));
        species.setSpeciesName(cursor.getString(0));
        species.setLifestage(cursor.getInt(1));
        species.setIdealPicture(cursor.getBlob(0));
        return species;
    }
}
