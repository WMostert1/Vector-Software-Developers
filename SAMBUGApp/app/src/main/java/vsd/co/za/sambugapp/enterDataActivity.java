package vsd.co.za.sambugapp;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.NumberPicker;
import android.widget.Spinner;

import java.util.ArrayList;


public class enterDataActivity extends ActionBarActivity {
    ScoutStop stop;
    Spinner mySpin;
    NumberPicker npTrees;
    NumberPicker npBugs;
    ScoutBug currBug;
    // Spinner

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_enter_data);
        populateSpinner();
        initializeNumberPickers();
        receiveGeoLocation();
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

    private void populateSpinner(){
        mySpin = (Spinner) findViewById(R.id.spnBlocks);
        ArrayAdapter<String> dataAdapter;

        ArrayAdapter<CharSequence> adapter = ArrayAdapter.createFromResource(this,
                R.array.arrBlocks, android.R.layout.simple_spinner_item);
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        mySpin.setAdapter(adapter);

       // toList = (Spinner) findViewById(R.id.toList);
        //dataAdapter = new ArrayAdapter<String>(this,
         //       android.R.layout.simple_spinner_item, new ArrayList<String>());
        //dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
       // toList.setAdapter(dataAdapter);

       // Button add = (Button) findViewById(R.id.add);
       // add.setOnClickListener(this);
       // Button remove = (Button) findViewById(R.id.remove);
       // remove.setOnClickListener(this);
    }

    public void initializeNumberPickers(){
        npTrees = (NumberPicker)findViewById(R.id.npNumTrees);
        npBugs = (NumberPicker)findViewById(R.id.npNumBugs);

        npTrees.setMinValue(1);
        npTrees.setMaxValue(100);
        npTrees.setWrapSelectorWheel(false);

        npBugs.setMinValue(0);
        npBugs.setMaxValue(100);
        npBugs.setWrapSelectorWheel(false);

    }
    public void sendToScoutTripActivity(View view){

        stop.setBlockName(mySpin.getSelectedItem().toString());
        stop.setNumTrees(npTrees.getValue());
        stop.addBugEntry(currBug);
        Intent intent = new Intent(enterDataActivity.this, ScoutTripActivity.class);
        startActivity(intent);
    }

    public void sendToManualActivity(View view){

//        Intent intent = new Intent(enterDataActivity.this, ScoutTripActivity.class);
//        startActivity(intent);
      //  int numTrees =
       // stop.setNumTrees();
    }


    public void receiveGeoLocation(){

        LocationManager locationManager = (LocationManager)getSystemService(Context.LOCATION_SERVICE);

        MyLocationListener locationListener = new MyLocationListener();

        locationManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, 0, 0, locationListener);

        if (locationManager.isProviderEnabled(LocationManager.GPS_PROVIDER)){
            //Do what you need if enabled...
        }else{
            createErrorMessage();
        }

    }

    public void createErrorMessage(){
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

    public void moveGPSScreen(){
        Intent gpsOptionsIntent = new Intent(
                android.provider.Settings.ACTION_LOCATION_SOURCE_SETTINGS);
        startActivity(gpsOptionsIntent);
    }

    public void createScoutStop(){
        stop = new ScoutStop();

    }
}
