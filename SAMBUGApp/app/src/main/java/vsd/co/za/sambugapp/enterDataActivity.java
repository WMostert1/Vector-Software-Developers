package vsd.co.za.sambugapp;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Bitmap;
import android.location.Location;
import android.location.LocationManager;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.Gravity;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.NumberPicker;
import android.widget.Spinner;
import android.widget.TableLayout;
import android.widget.TableRow;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;

import vsd.co.za.sambugapp.DomainModels.Block;
import vsd.co.za.sambugapp.DomainModels.Farm;
import vsd.co.za.sambugapp.DomainModels.ScoutBug;
import vsd.co.za.sambugapp.DomainModels.ScoutStop;
import vsd.co.za.sambugapp.DomainModels.Species;


public class enterDataActivity extends ActionBarActivity {
    ScoutStop stop;
    Species species;
    Spinner mySpin;
    NumberPicker npTrees;
    NumberPicker npBugs;
    ScoutBug currBug;
    Farm farm;
    Block currBlock;
    Bitmap imageTaken;
    int bugNumber;
    ArrayList<ScoutBug> allBugs;
    TableLayout table;
    // Spinner

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_enter_data);
        bugNumber = 1;
        allBugs = new ArrayList<ScoutBug>();
        Intent iReceive = getIntent();
       // Bundle scoutStop = iReceive.getExtras();
        acceptStop(iReceive);
        acceptBlocks(iReceive);
        populateSpinner();
        initializeNumberPickers();
       // receiveGeoLocation();
        createScoutStop();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_enter_data, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    private void populateSpinner() {
        mySpin = (Spinner) findViewById(R.id.spnBlocks);
        ArrayAdapter<String> dataAdapter;

        HashSet<Block> blockArray = new HashSet<>();
        blockArray = farm.getBlocks();
       // Iterator iterator = blockArray.iterator();

//        while(iterator.hasNext()){
//            mySpin.add
//        }
        List<Block> list = new ArrayList<Block>(blockArray);

        ArrayAdapter<Block> adapter = new ArrayAdapter<Block>(this, android.R.layout.simple_spinner_item, list);// (this, android.R.layout.simple_spinner_item,blockArray);
        //ArrayAdapter.createFromResource(this,
                //R.array.arrBlocks, android.R.layout.simple_spinner_item);
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

    public void initializeNumberPickers() {
        npTrees = (NumberPicker) findViewById(R.id.npNumTrees);
        npBugs = (NumberPicker) findViewById(R.id.npNumBugs1);

        npTrees.setMinValue(1);
        npTrees.setMaxValue(100);
        npTrees.setWrapSelectorWheel(false);

        npBugs.setMinValue(0);
        npBugs.setMaxValue(100);
        npBugs.setWrapSelectorWheel(false);

    }

    public void sendToScoutTripActivity(View view) {

        stop.Block.setBlockName(mySpin.getSelectedItem().toString());
        stop.setNumberOfTrees(npTrees.getValue());
//       // stop.
////        Intent intent = new Intent(enterDataActivity.this, ScoutTripActivity.class);
////        startActivity(intent);
//
//        Intent output = new Intent();
//        Bundle b = new Bundle();
//        b.putSerializable("ScoutStop",stop);
//        output.putExtras(b);
//        setResult(RESULT_OK, output);
//        finish();

        Intent output = new Intent();
        Bundle b = new Bundle();
        b.putSerializable(ScoutTripActivity.SCOUT_STOP,stop);
        output.putExtras(b);
        setResult(RESULT_OK, output);
        finish();
    }

    public void sendToIdentificationActivity(View view) {

        Intent intent = new Intent(enterDataActivity.this, IdentificationActivity.class);
        startActivityForResult(intent, 0);
        //startActivity(intent);
        //  int numTrees =
        // stop.setNumTrees();
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        Log.e("Look", "here1");
        if (requestCode == 0 && resultCode == RESULT_OK && data != null) {
            Log.e("Look", "here2");
            Bundle speciesReceived = data.getExtras();
            Species species = (Species) speciesReceived.get("Species");
            Bitmap imageTaken2 = (Bitmap)speciesReceived.getParcelable("Image");
            imageTaken = imageTaken2;
          //  addImage(imageTaken);
           // createBug(species);
            Log.e("Look", species.getSpeciesName());
        }
    }

//    //public void addImage(Bitmap im){
//        stop.setImageCaptured(im);
//    }
    private void createBug(Species spec){
        ScoutBug sb = new ScoutBug();
        sb.setSpecies(spec);
        stop.ScoutBugs.add(sb); //addBugEntry(sb);
    }

    LocationManager mLocationManager;
    Location myLocation = null;//= getLastKnownLocation();
    public void receiveGeoLocation() {
        myLocation = getLastKnownLocation();
        String sLocation = "Latitude = " + myLocation.getLatitude() + " Longitude = " + myLocation.getLongitude();
        Log.d("MY CURRENT LOCATION", sLocation);

//

    }
  //  LocationManager mLocationManager;
   // Location myLocation = getLastKnownLocation();
    private Location getLastKnownLocation() {
        mLocationManager = (LocationManager)getApplicationContext().getSystemService(LOCATION_SERVICE);
        List<String> providers = mLocationManager.getProviders(true);
        if (mLocationManager.isProviderEnabled(LocationManager.GPS_PROVIDER)) {
            //Do what you need if enabled...
        } else {
            createErrorMessage();
        }
        Location bestLocation = null;
        for (String provider : providers) {
            Location l = mLocationManager.getLastKnownLocation(provider);
            if (l == null) {
                continue;
            }
            else {
                // Found best last known location: %s", l);
                bestLocation = l;
            }
        }
        return bestLocation;
    }

    public void createErrorMessage() {
        new AlertDialog.Builder(this)
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

    public void moveGPSScreen() {
        Intent gpsOptionsIntent = new Intent(
                android.provider.Settings.ACTION_LOCATION_SOURCE_SETTINGS);
        startActivity(gpsOptionsIntent);
    }

    public void createScoutStop() {
        stop = new ScoutStop();


    }

    private void usePassedStop(ScoutStop sp){
        //stop.duplicateStop(sp);
        stop=sp;
        currBlock = stop.getBlock();
    }

    private void acceptStop(Intent iReceive){
        Bundle scoutStop = iReceive.getExtras();
        ScoutStop sp = (ScoutStop) scoutStop.get(ScoutTripActivity.SCOUT_STOP);
        if(sp == null){
            createScoutStop();
        }
        else usePassedStop(sp);
       // Log.e("Look",stop.getBlockName() );
    }

    private void acceptBlocks(Intent iReceive){
        Bundle scoutStop = iReceive.getExtras();
        Farm frm = (Farm) scoutStop.get(ScoutTripActivity.USER_FARM);
        farm = frm;
    }

    public void sendResultBack(View view) {
        Intent output = new Intent();
        Bundle b = new Bundle();
        b.putSerializable("ScoutStop",stop);
        output.putExtras(b);
        setResult(RESULT_OK, output);
        finish();
    }

    public void addBug(View view){
        storeCurrentBug();
        bugNumber++;
        table = (TableLayout) findViewById(R.id.tblLayout);
        TableRow delRow = (TableRow)findViewById(R.id.delRow);
        table.removeView(delRow);

        TableRow row= new TableRow(this);

        TextView tv1 = new TextView(this);
        tv1.setText(R.string.bugType);
        tv1.setTextSize(24);
        tv1.setGravity(Gravity.CENTER_VERTICAL);

        TableRow.LayoutParams params = new TableRow.LayoutParams(TableRow.LayoutParams.WRAP_CONTENT, TableRow.LayoutParams.FILL_PARENT);
        params.weight = 1.0f;
        params.gravity = Gravity.TOP;
        tv1.setMinWidth(220);

        NumberPicker np1 = new NumberPicker(this);

        np1.setMinValue(0);
        np1.setMaxValue(100);
        np1.setWrapSelectorWheel(false);
//        np1.getId();
//        np1.setId(bugNumber);
        row.addView(tv1);

        row.addView(np1);

        TextView tv2 = new TextView(this);
        tv2.setText(R.string.numBugs);
        tv2.setTextSize(24);
        tv2.setGravity(Gravity.CENTER_VERTICAL);
        tv2.setMinWidth(220);
        Button btnSelectBug = new Button(this);
        btnSelectBug.setText( R.string.btnSelectBug);
        TableRow row2= new TableRow(this);
        row2.addView(tv2);
        row2.addView(btnSelectBug);


        TableRow emptyRow = new TableRow(this);
       // table.addView(emptyRow);
        table.addView(row);
        table.addView(row2);
        table.addView(delRow);
    }

    public void storeCurrentBug(){
        ScoutBug currBug = new ScoutBug();
        if(species != null){
            currBug.setFieldPicture(imageTaken);
        }
        //currBug.setSpecies(species);
        if(imageTaken != null){
            currBug.setFieldPicture(imageTaken);
        }

//        TableRow rowNumberPicker = (TableRow) table.getChildAt(table.getChildCount() - 3);
//        NumberPicker currNumberPicker = (NumberPicker) rowNumberPicker.getChildAt(1);
//        //NumberPicker currNumberPicker = (NumberPicker)findViewById(R.id.npNumBugs1);
//        currBug.setNumberOfBugs(currNumberPicker.getValue());
        allBugs.add(currBug);
       // currBug
    }
}
