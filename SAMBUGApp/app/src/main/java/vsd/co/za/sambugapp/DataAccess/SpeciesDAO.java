package vsd.co.za.sambugapp.DataAccess;

import android.content.ContentValues;
import android.content.Context;
import android.content.res.Resources;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.graphics.drawable.BitmapDrawable;
import android.graphics.drawable.Drawable;

import java.io.ByteArrayOutputStream;
import java.sql.SQLException;
import java.text.DateFormat;
import java.text.ParseException;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import vsd.co.za.sambugapp.DomainModels.Species;
import vsd.co.za.sambugapp.R;



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
                DBHelper.COLUMN_IS_PEST
        };
    }

    /**
     * This is only for phase 1 of the project to get some pre-loaded data into the database without
     * needing the sync service from BugCentral
     *
     * @throws Resources.NotFoundException This is thrown when the drawables are missing
     */
    public void loadPresets() throws Resources.NotFoundException {
        //These strings story the species name
        String[] speciesNames = {"Coconut Bug", "Green Vegetable Bug", "Two Spotted Bug", "Yellow Edged Bug"};

        //These array lists hold the integers that reference the drawable images
        ArrayList<Integer> cRef = new ArrayList<>();
        ArrayList<Integer> gvRef = new ArrayList<>();
        ArrayList<Integer> tsRef = new ArrayList<>();
        ArrayList<Integer> yeRef = new ArrayList<>();

        ArrayList<ArrayList<Integer>> references = new ArrayList<>();

        references.add(cRef);
        references.add(gvRef);
        references.add(tsRef);
        references.add(yeRef);

        cRef.add(R.drawable.coconut_inst_1);
        cRef.add(R.drawable.coconut_inst_2);
        cRef.add(R.drawable.coconut_inst_3);
        cRef.add(R.drawable.coconut_inst_4);

        gvRef.add(R.drawable.green_veg_inst_1);
        gvRef.add(R.drawable.green_veg_inst_2);
        gvRef.add(R.drawable.green_veg_inst_3);
        gvRef.add(R.drawable.green_veg_inst_4);

        tsRef.add(R.drawable.two_spot_inst_1);
        tsRef.add(R.drawable.two_spot_inst_2);
        tsRef.add(R.drawable.two_spot_inst_3);
        tsRef.add(R.drawable.two_spot_inst_4);

        yeRef.add(R.drawable.yellow_edged_inst_1);
        yeRef.add(R.drawable.yellow_edged_inst_2);
        yeRef.add(R.drawable.yellow_edged_inst_3);
        yeRef.add(R.drawable.yellow_edged_inst_4);

        for (int bugType = 0; bugType < references.size(); bugType++) {
            ArrayList<Integer> specificBugReferences = references.get(bugType);
            for (int instarNo = 0; instarNo < specificBugReferences.size(); instarNo++) {
                Species speciesEntry = new Species();
                speciesEntry.setSpeciesName(speciesNames[bugType]);
                speciesEntry.setLifestage(instarNo);

                Drawable drawable = context.getResources().getDrawable(specificBugReferences.get(instarNo)); //Only deprecated because our target SDK is 22
                if (drawable == null) throw new Resources.NotFoundException();
                Bitmap bitmap = ((BitmapDrawable) drawable).getBitmap();

                ByteArrayOutputStream stream = new ByteArrayOutputStream();
                bitmap.compress(Bitmap.CompressFormat.JPEG, 100, stream);
                speciesEntry.setIdealPicture(stream.toByteArray());

                speciesEntry.setIsPest(true);
                speciesEntry.setLifestage(instarNo + 1);
                insert(speciesEntry);
            }
        }

    }

    public void delete(Species species) {
        int id = species.getSpeciesID();
        database.delete(DBHelper.TABLE_SPECIES, DBHelper.COLUMN_SPECIES_ID + " = " + id, null);
    }

    public void update(Species species) {
        int id = species.getSpeciesID();
        ContentValues values = new ContentValues();
        values.put(DBHelper.COLUMN_SPECIES_NAME, species.getSpeciesName());
        values.put(DBHelper.COLUMN_LIFESTAGE, species.getLifestage());
        values.put(DBHelper.COLUMN_IDEAL_PICTURE, species.getIdealPicture());
        values.put(DBHelper.COLUMN_IS_PEST, species.isPest());

        database.update(DBHelper.TABLE_SPECIES, values, DBHelper.COLUMN_SPECIES_ID + " = " + id, null);
    }

    public long insert(Species species) {
        ContentValues values = new ContentValues();
        values.put(DBHelper.COLUMN_SPECIES_NAME, species.getSpeciesName());
        values.put(DBHelper.COLUMN_LIFESTAGE, species.getLifestage());
        values.put(DBHelper.COLUMN_IDEAL_PICTURE, species.getIdealPicture());
        values.put(DBHelper.COLUMN_IS_PEST, species.isPest());

        return database.insert(DBHelper.TABLE_SPECIES, null, values);
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

    public void clearTable() {
        database.delete(DBHelper.TABLE_SPECIES, null, null);
    }

    public Species getSpeciesByID(int id) {
        Cursor cursor = database.query(DBHelper.TABLE_SPECIES, allColumns, DBHelper.COLUMN_SPECIES_ID + " = " + id, null, null, null, null);
        cursor.moveToFirst();
        if (cursor.isAfterLast()) {
            cursor.close();
            return null;
        }
        Species species = cursorToSpecies(cursor);

        //Remember to close the cursor
        cursor.close();
        return species;
    }

    public boolean isEmpty() {
        Cursor cursor = database.query(DBHelper.TABLE_SPECIES, allColumns, null, null, null, null, null);
        cursor.moveToFirst();

        boolean result = cursor.isAfterLast();
        cursor.close();
        return result;
    }

    public Species cursorToSpecies(Cursor cursor) {
        Species species = new Species();
        species.setSpeciesID(cursor.getInt(0));
        species.setSpeciesName(cursor.getString(1));
        species.setLifestage(cursor.getInt(2));
        species.setIdealPicture(cursor.getBlob(3));
        species.setIsPest(cursor.getInt(4) == 1);

        return species;
    }
}
