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
    Species species;
    Spinner mySpin;
    NumberPicker npTrees;
    NumberPicker npBugs;
    ScoutBug currBug;
    // Spinner

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_enter_data);
        acceptStop();
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

    private void populateSpinner() {
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

    public void initializeNumberPickers() {
        npTrees = (NumberPicker) findViewById(R.id.npNumTrees);
        npBugs = (NumberPicker) findViewById(R.id.npNumBugs);

        npTrees.setMinValue(1);
        npTrees.setMaxValue(100);
        npTrees.setWrapSelectorWheel(false);

        npBugs.setMinValue(0);
        npBugs.setMaxValue(100);
        npBugs.setWrapSelectorWheel(false);

    }

    public void sendToScoutTripActivity(View view) {

        stop.setBlockName(mySpin.getSelectedItem().toString());
        stop.setNumTrees(npTrees.getValue());
        stop.addBugEntry(currBug);
        Intent intent = new Intent(enterDataActivity.this, ScoutTripActivity.class);
        startActivity(intent);
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
        if (requestCode == 0 && resultCode == RESULT_OK && data != null) {
            Bundle speciesReceived = data.getExtras();
            Species species = (Species) speciesReceived.get("Species");
            createBug(species);
            Log.e("Look", species.getSpeciesName());
        }
    }

    private void createBug(Species spec){
        ScoutBug sb = new ScoutBug();
        sb.setSpecies(spec);
        stop.addBugEntry(sb);
    }
    public void receiveGeoLocation() {

        LocationManager locationManager = (LocationManager) getSystemService(Context.LOCATION_SERVICE);

        MyLocationListener locationListener = new MyLocationListener();

        locationManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, 0, 0, locationListener);

        if (locationManager.isProviderEnabled(LocationManager.GPS_PROVIDER)) {
            //Do what you need if enabled...
        } else {
            createErrorMessage();
        }

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
    }

    private void acceptStop(){
        Intent iReceive = getIntent();
        Bundle scoutStop = iReceive.getExtras();
        ScoutStop sp = (ScoutStop) scoutStop.get("ScoutStop");
        if(sp == null){
            createScoutStop();
        }
        else usePassedStop(sp);
        Log.e("Look",stop.getBlockName() );
    }

    public void sendResultBack(View view) {
        Intent output = new Intent();
        Bundle b = new Bundle();
        b.putSerializable("ScoutStop",stop);
        output.putExtras(b);
        setResult(RESULT_OK, output);
        finish();

    }
}
