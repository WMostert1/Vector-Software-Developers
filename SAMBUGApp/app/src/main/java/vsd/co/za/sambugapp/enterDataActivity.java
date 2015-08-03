package vsd.co.za.sambugapp;

import android.app.ActionBar;
import android.app.AlertDialog;
import android.app.Dialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.provider.Settings;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.NumberPicker;
import android.widget.RelativeLayout;
import android.widget.Spinner;
import android.widget.TableLayout;
import android.widget.TableRow;
import android.widget.TextView;

import java.io.ByteArrayOutputStream;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashSet;
import java.util.Iterator;
import java.util.LinkedHashSet;
import java.util.List;

import vsd.co.za.sambugapp.DomainModels.Block;
import vsd.co.za.sambugapp.DomainModels.Farm;
import vsd.co.za.sambugapp.DomainModels.ScoutBug;
import vsd.co.za.sambugapp.DomainModels.ScoutStop;
import vsd.co.za.sambugapp.DomainModels.Species;


public class enterDataActivity extends ActionBarActivity {
    private final String BUG_COUNT="za.co.vsd.bug_count";
    private final String BUG_LIST="za.co.vsd.bug_list";
    private final String TREE_COUNT="za.co.vsd.tree_count";
    ScoutStop stop;
    Species species;
    Spinner mySpin;
    NumberPicker npTrees;
    NumberPicker npBugs;
    ScoutBug currBug;
    Farm farm;
    Block currBlock;
    Bitmap imageTaken;
    HashSet<ScoutBug> allBugs;
    TableLayout table;
    boolean first = true;
    // Spinner

    LocationManager mLocationManager;
    Location myLocation = null;

    public synchronized Location getMyLocation() {
        return myLocation;
    }

    public void setMyLocation(Location myLocation) {
        this.myLocation = myLocation;
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_enter_data);
        if (savedInstanceState!=null){
            allBugs=(HashSet<ScoutBug>)savedInstanceState.get(BUG_LIST);
            updateAddedBugsView();
        } else {
            allBugs = new HashSet<ScoutBug>();
        }
            Intent iReceive = getIntent();
            receiveGeoLocation();
            acceptBlocks(iReceive);
        acceptStop(iReceive);
        setTitle(farm.getFarmName());
            populateSpinner();
            initializeNumberPickers(savedInstanceState);

    }


    /**
     * Function was automatically added by the activity.
     * @param menu
     * @return
     */
    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_enter_data, menu);
        return true;
    }

    /**
     * Function was automatically added by the activity.
     * @param item
     * @return
     */
    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();
        if (id == R.id.action_settings) {
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    @Override
    public void onSaveInstanceState(Bundle savedInstanceState){
        super.onSaveInstanceState(savedInstanceState);
        savedInstanceState.putInt(BUG_COUNT, npBugs.getValue());
        savedInstanceState.putInt(TREE_COUNT,npTrees.getValue());
        savedInstanceState.putSerializable(BUG_LIST,allBugs);
    }

    /**
     * This function is called when the app returns from the IdentificationActivity.
     * It Saves the Species object and the imageTaken. All of this is received from the IdentificationActivity.
     * @param requestCode
     * @param resultCode
     * @param data
     */
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == 0 && resultCode == RESULT_OK && data != null) {
            Bundle speciesReceived = data.getExtras();
            species = (Species) speciesReceived.get(IdentificationActivity.IDENTIFICATION_SPECIES);
            Bitmap imageTaken2 = (Bitmap)speciesReceived.getParcelable("Image");
            imageTaken = imageTaken2;
        }
    }

    public Spinner getMySpin() {
        return mySpin;
    }

    public void setMySpin(Spinner mySpin) {
        this.mySpin = mySpin;
    }

    /**
     * Accepts the Stop Object. If no object is found, it creates one.
     * @param iReceive
     */
    public void acceptStop(Intent iReceive){

        Bundle scoutStop = iReceive.getExtras();
        ScoutStop sp = (ScoutStop) scoutStop.get(ScoutTripActivity.SCOUT_STOP);
        if(sp == null){
            createScoutStop();
        }
        else usePassedStop(sp);
    }

    /**
     * Gets the blocks from the farm object passed.
     * @param iReceive- the intent used to pass the farm.
     */
    public void acceptBlocks(Intent iReceive){
        Bundle scoutStop = iReceive.getExtras();
        Farm frm = (Farm) scoutStop.get(ScoutTripActivity.USER_FARM);
        if(frm != null) {
            setFarm(frm);
        }
        else Log.e("Error", "No block exists!");
    }

    /**
     * Populates the spinners with the appropriate blocks.
     */
    public void populateSpinner() {
        mySpin = (Spinner) findViewById(R.id.spnBlocks);
        ArrayAdapter<String> dataAdapter;

        HashSet<Block> blockArray = new HashSet<>();
        blockArray = farm.getBlocks();

        List<Block> list = new ArrayList<Block>(blockArray);

        ArrayAdapter<Block> adapter = new ArrayAdapter<Block>(this, android.R.layout.simple_spinner_item, list);
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        mySpin.setAdapter(adapter);
        int pos =0;
        for(int i =0; i <mySpin.getItemIdAtPosition(i);i++){
            if(mySpin.getItemAtPosition(i) == currBlock){
                pos = i;
                break;
            }
        }
        mySpin.setSelection(pos);
    }

    /**
     * Initializes the number pickers to the appropriate values.
     * @param savedInstanceState
     */
    public void initializeNumberPickers(Bundle savedInstanceState) {
        npTrees = (NumberPicker) findViewById(R.id.npNumTrees);
        npBugs = (NumberPicker) findViewById(R.id.npNumBugs);

        npTrees.setMinValue(1);
        npTrees.setMaxValue(100);
        npTrees.setWrapSelectorWheel(false);

        npBugs.setMinValue(0);
        npBugs.setMaxValue(100);
        npBugs.setWrapSelectorWheel(false);
        if (savedInstanceState!=null){
            npBugs.setValue(savedInstanceState.getInt(BUG_COUNT));
            npTrees.setValue(savedInstanceState.getInt(TREE_COUNT));
        }

    }

    
    public void sendToScoutTripActivity(View view) {

        String blockName=mySpin.getSelectedItem().toString();
        Block tempBlock=null;
        for (Block b:farm.getBlocks()){
            if (b.getBlockName().equals(blockName)){
                tempBlock=b;
                break;
            }
        }
        stop.setBlockID(tempBlock.getBlockID());
        stop.setBlock(tempBlock);
        stop.setNumberOfTrees(npTrees.getValue());

        Intent output = new Intent();
        Bundle b = new Bundle();
        stop.setScoutBugs(allBugs);
        b.putSerializable(ScoutTripActivity.SCOUT_STOP,stop);
        output.putExtras(b);
        setResult(RESULT_OK, output);
        finish();
    }

    public void sendToIdentificationActivity(View view) {

        Intent intent = new Intent(enterDataActivity.this, IdentificationActivity.class);
        startActivityForResult(intent, 0);
    }


    private void createBug(Species spec,int numBugs,Bitmap fieldImg){
        ScoutBug sb = new ScoutBug();
        sb.setSpecies(spec);
        sb.setSpeciesID(spec.getSpeciesID());
        sb.setNumberOfBugs(numBugs);
        ByteArrayOutputStream stream = new ByteArrayOutputStream();
        fieldImg.compress(Bitmap.CompressFormat.JPEG,100,stream);
        sb.setFieldPicture(stream.toByteArray());
        //TODO: change to user id eventually
        sb.setLastModifiedID(1);
        sb.setTMStamp(new Date());
        stop.ScoutBugs.add(sb); //addBugEntry(sb);
    }
    /**
     * Receives the Location Data from the device.
     */
    public Location receiveGeoLocation() {
        mLocationManager = (LocationManager)getSystemService(Context.LOCATION_SERVICE);
        LocationListener locationListener = new MyLocationListener();
        if (!CheckIfGPSON()){
            Intent gpsOptionsIntent = new Intent(
                    android.provider.Settings.ACTION_LOCATION_SOURCE_SETTINGS);
            startActivity(gpsOptionsIntent);
        } else {
            mLocationManager.requestSingleUpdate(LocationManager.GPS_PROVIDER,locationListener,null);
            myLocation = getLastKnownLocation(); //mLocationManager.getLastKnownLocation(LocationManager.GPS_PROVIDER);
        }
        return myLocation;
    }


    /**
     * Checks if GPS is on.
     * @return
     */
    public boolean CheckIfGPSON(){
        return mLocationManager.isProviderEnabled(LocationManager.GPS_PROVIDER);
    }


    /**
     * Gets the last known location if GPS is on. Else it goes to an error screen.
     * @return Location object.
     */
    private synchronized Location getLastKnownLocation() {
        mLocationManager = (LocationManager)getApplicationContext().getSystemService(LOCATION_SERVICE);
        List<String> providers = mLocationManager.getProviders(true);
        Location bestLocation = null;

        if (mLocationManager.isProviderEnabled(LocationManager.GPS_PROVIDER)) {
            for (String provider : providers) {
                Location l = mLocationManager.getLastKnownLocation(provider);
                if (l == null) {
                    continue;
                }
                else {
                    bestLocation = l;
                }
            }
        }
        else {
        //createErrorMessage();
        }


        return bestLocation;
    }

    public void setFarm(Farm farm) {
        this.farm = farm;
    }

    /**
     * Error message if gps is off.
     */
    public void createErrorMessage() {
        new AlertDialog.Builder(enterDataActivity.this)
                .setTitle("Switch on gps")
                .setMessage("Please ensure your gps is switched on.")
                .setPositiveButton(android.R.string.ok, new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int which) {
                        moveGPSScreen();
                    }
                })
                .setNegativeButton(android.R.string.cancel, new DialogInterface.OnClickListener() {
                    public void onClick(DialogInterface dialog, int which) {
                        // do nothing
                    }
                })
                .setIcon(android.R.drawable.ic_dialog_alert)
                .show();

    }

    /**
     * Moves to the GPS screen.
     */
    public synchronized void moveGPSScreen() {
        Intent gpsOptionsIntent = new Intent(
                android.provider.Settings.ACTION_LOCATION_SOURCE_SETTINGS);
        startActivity(gpsOptionsIntent);
    }

    /**
     * Creates a ScoutStop object.
     */
    public synchronized void createScoutStop() {
        stop = new ScoutStop();
        stop.setDate(new Date());
        stop.setLastModifiedID(farm.getUserID());
        stop.setTMStamp(new Date());
        stop.setUserID(farm.getUserID());
        if(myLocation != null){
            stop.setLatitude((float) myLocation.getLatitude());
            stop.setLongitude((float) myLocation.getLongitude());
        }
    }


    private void usePassedStop(ScoutStop sp){
        //stop.duplicateStop(sp);
        stop=sp;
        currBlock = stop.getBlock();
    }


    /**
     * Adds a bug
     * @param view
     */
    public void addBug(View view){
        // table = (TableLayout) findViewById(R.id.tblLayout);
        storeCurrentBug();
        //bugNumber++;
        updateAddedBugsView();

        addRowsDynamically();

    }

    /**
     * Adds rows to the the layout dynamically.
     */
    public void addRowsDynamically(){
//        TableRow delRow = (TableRow)findViewById(R.id.delRow);
//        table.removeView(delRow);
//
//        TableRow row= new TableRow(this);
//
//        TextView tv1 = new TextView(this);
//        tv1.setText(R.string.numBugs);
//
//        tv1.setTextSize(24);
//
//        TableRow.LayoutParams params = new TableRow.LayoutParams(TableRow.LayoutParams.WRAP_CONTENT, TableRow.LayoutParams.FILL_PARENT);
//        params.weight = 1.0f;
//        params.gravity = Gravity.TOP;
//        tv1.setMinWidth(220);
//
//        NumberPicker np1 = new NumberPicker(this);
//
//        np1.setMinValue(0);
//        np1.setMaxValue(100);
//        np1.setWrapSelectorWheel(false);
//        row.addView(tv1);
//
//        row.addView(np1);
//
//        TextView tv2 = new TextView(this);
//        tv2.setText(R.string.bugType);
//        tv2.setTextSize(24);
//        tv2.setMinWidth(220);
//        Button btnSelectBug = new Button(this);
//        btnSelectBug.setText(R.string.btnSelectBug);
//        btnSelectBug.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View v) {
//                sendToIdentificationActivity(v);
//            }
//        });
//        TableRow row2= new TableRow(this);
//        row2.addView(tv2);
//        row2.addView(btnSelectBug);
//
//
//        Bitmap takenImage =     BitmapFactory.decodeByteArray(currBug.getFieldPicture(),0,currBug.getFieldPicture().length);
//        ImageView im = new ImageView(this);
//        im.setImageBitmap(takenImage);
//
//        TableRow row3= new TableRow(this);
//        im.setLayoutParams(new TableRow.LayoutParams());
//        im.getLayoutParams().height = 350; // OR
//        im.getLayoutParams().width =  350;
//        //im.setLayoutParams(new TableLayout.LayoutParams(50, 50));
//        row3.addView(im);
//
//        TableRow emptyRow = new TableRow(this);
//
//        // table.addView(emptyRow);
//        table.addView(row3);
//        table.addView(row);
//        table.addView(row2);
//
//        table.addView(delRow);
    }

    public void updateAddedBugsView(){
        LinearLayout info=(LinearLayout)findViewById(R.id.addedBugsContent);
        info.removeAllViews();
        for (ScoutBug bug:allBugs) {
            View bugInfo = ((LayoutInflater) getSystemService(Context.LAYOUT_INFLATER_SERVICE)).inflate(R.layout.bug_info, null);
            ((ImageView) bugInfo.findViewById(R.id.bugInfoImage)).setImageBitmap(Bitmap.createScaledBitmap(BitmapFactory.decodeByteArray(bug.getFieldPicture(), 0, bug.getFieldPicture().length),150,150,true));
            ((TextView) bugInfo.findViewById(R.id.bugInfoText)).setText(bug.getNumberOfBugs() + "");
            info.addView(bugInfo);
        }
    }
    /**
     * Stores the current Bugs
     */
    public void storeCurrentBug(){
        currBug = new ScoutBug();
        if(species != null){
            currBug.setSpecies(species);
        }

        if(imageTaken != null){
            ByteArrayOutputStream stream = new ByteArrayOutputStream();
            imageTaken.compress(Bitmap.CompressFormat.JPEG,100,stream);
            currBug.setFieldPicture(stream.toByteArray());
        }

        // TableRow rowNumberPicker;
        // rowNumberPicker = (TableRow) table.getChildAt(table.getChildCount() - 3);

        // NumberPicker currNumberPicker = (NumberPicker) rowNumberPicker.getChildAt(1);

        //currBug.setNumberOfBugs(currNumberPicker.getValue());
        currBug.setNumberOfBugs(npBugs.getValue());
        currBug.setSpecies(species);
        currBug.setSpeciesID(species.getSpeciesID());
        ByteArrayOutputStream stream = new ByteArrayOutputStream();
        imageTaken.compress(Bitmap.CompressFormat.JPEG, 100, stream);
        currBug.setFieldPicture(stream.toByteArray());
        //TODO: change to user id eventually
        currBug.setLastModifiedID(1);
        currBug.setTMStamp(new Date());
        stop.ScoutBugs.add(currBug);
        allBugs.add(currBug);

    }

    public Farm getFarm() {
        return farm;
    }
}
