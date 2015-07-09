package vsd.co.za.sambugapp;

import android.content.Context;
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
import android.widget.Spinner;


public class enterDataActivity extends ActionBarActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_enter_data);
        populateSpinner();
       // receiveGeoLocation();

        doS();
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
        Spinner mySpin = (Spinner) findViewById(R.id.spnBlocks);
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

    public void sendToScoutTripActivity(View view){
        Intent intent = new Intent(enterDataActivity.this, ScoutTripActivity.class);
        startActivity(intent);
    }

    public void sendToManualActivity(View view){
//        Intent intent = new Intent(enterDataActivity.this, ScoutTripActivity.class);
//        startActivity(intent);
    }


    private void receiveGeoLocation(){
        LocationManager locationManager = (LocationManager) this.getSystemService(Context.LOCATION_SERVICE);
        LocationListener locationListener = new LocationListener() {
            public void onLocationChanged(Location location) {
                // Called when a new location is found by the network location provider.
                makeUseOfNewLocation(location);
            }

            public void onStatusChanged(String provider, int status, Bundle extras) {}

            public void onProviderEnabled(String provider) {}

            public void onProviderDisabled(String provider) {}
        };


        String locationProvider = LocationManager.GPS_PROVIDER;
// Register the listener with the Location Manager to receive location updates
        locationManager.requestLocationUpdates(locationProvider, 0, 0, locationListener);
       // locationManager.removeUpdates(locationListener);

    }

    private void makeUseOfNewLocation(Location location){
        Log.d("herer",location.toString() );
        System.out.println("Here " + location);
    }

    public void doS(){

        LocationManager locationManager = (LocationManager)getSystemService(Context.LOCATION_SERVICE);

        MyLocationListener locationListener = new MyLocationListener();

        locationManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, 0, 0, locationListener);

    }
}
