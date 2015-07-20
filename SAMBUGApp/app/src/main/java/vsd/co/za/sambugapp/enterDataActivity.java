package vsd.co.za.sambugapp;

import android.app.ActionBar;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
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
import java.util.List;
import vsd.co.za.sambugapp.DomainModels.Block;
import vsd.co.za.sambugapp.DomainModels.Farm;
import vsd.co.za.sambugapp.DomainModels.ScoutBug;
import vsd.co.za.sambugapp.DomainModels.ScoutStop;
import vsd.co.za.sambugapp.DomainModels.Species;


public class enterDataActivity extends ActionBarActivity {
    private final String BUG_COUNT="za.co.vsd.bug_count";
    private final String BUG_LIST="za.co.vsd.bug_list";
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
    HashSet<ScoutBug> allBugs;
    TableLayout table;
    boolean first = true;
    // Spinner
    LocationManager mLocationManager;
    Location myLocation = null;//= getLastKnownLocation();

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

        bugNumber = 1;

        Intent iReceive = getIntent();
        // Bundle scoutStop = iReceive.getExtras();
        acceptStop(iReceive);
        acceptBlocks(iReceive);

        populateSpinner();
        initializeNumberPickers(savedInstanceState);
        //receiveGeoLocation();
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

    @Override
    public void onSaveInstanceState(Bundle savedInstanceState){
        super.onSaveInstanceState(savedInstanceState);
        savedInstanceState.putInt(BUG_COUNT, npBugs.getValue());
        savedInstanceState.putSerializable(BUG_LIST,allBugs);
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
        }

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
        stop.setScoutBugs(allBugs);
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
            species = (Species) speciesReceived.get(IdentificationActivity.IDENTIFICATION_SPECIES);
            //currBug.setSpecies(species);
            //createBug(species,);
            Bitmap imageTaken2 = (Bitmap)speciesReceived.getParcelable("Image");
            imageTaken = imageTaken2;
          //  addImage(imageTaken);
           // createBug(species);
            Log.e("Look", species.getSpeciesName());
        }
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
        stop.setDate(new Date());
        //TODO:change to user id eventually
        stop.setLastModifiedID(1);
        stop.setTMStamp(new Date());
        stop.setLatitude(12);
        stop.setLongitude(12);
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
       // stop.setScoutBugs(allBugs);
        //convertArrayListToHashSet(allBugs);
        b.putSerializable(ScoutTripActivity.SCOUT_STOP, stop);
        output.putExtras(b);
        setResult(RESULT_OK, output);
        finish();
    }



    public void addBug(View view){
        storeCurrentBug();
        bugNumber++;
        updateAddedBugsView();

        //TableRow delRow = (TableRow)findViewById(R.id.delRow);
        //table.removeView(delRow);

        //TableRow row= new TableRow(this);


        //TextView tv1 = new TextView(this);
        //tv1.setText(R.string.numBugs);

        //tv1.setTextSize(24);
       // tv1.setGravity(Gravity.CENTER_VERTICAL);

//        TableRow.LayoutParams params = new TableRow.LayoutParams(TableRow.LayoutParams.WRAP_CONTENT, TableRow.LayoutParams.FILL_PARENT);
//        params.weight = 1.0f;
//        params.gravity = Gravity.TOP;
//        tv1.setMinWidth(220);

        //NumberPicker np1 = new NumberPicker(this);

//        np1.setMinValue(0);
//        np1.setMaxValue(100);
//        np1.setWrapSelectorWheel(false);
//        np1.getId();
//        np1.setId(bugNumber);
        //row.addView(tv1);

        //row.addView(np1);

//        TextView tv2 = new TextView(this);
//        tv2.setText(R.string.bugType);
//        tv2.setTextSize(24);
       // tv2.setGravity(Gravity.CENTER_VERTICAL);
//        tv2.setMinWidth(220);
//        Button btnSelectBug = new Button(this);
//        btnSelectBug.setText(R.string.btnSelectBug);
//        btnSelectBug.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View v) {
//                sendToIdentificationActivity(v);
//                //addBug(v);
//            }
//        });
//        TableRow row2= new TableRow(this);
//        row2.addView(tv2);
//        row2.addView(btnSelectBug);


//        Bitmap takenImage =     BitmapFactory.decodeByteArray(currBug.getFieldPicture(),0,currBug.getFieldPicture().length);
//        ImageView im = new ImageView(this);
//        im.setImageBitmap(takenImage);

//        TableRow row3= new TableRow(this);
//        im.setLayoutParams(new TableRow.LayoutParams());
//        im.getLayoutParams().height = 350; // OR
//        im.getLayoutParams().width =  350;
//        //im.setLayoutParams(new TableLayout.LayoutParams(50, 50));
//        row3.addView(im);

        //TableRow emptyRow = new TableRow(this);

       // table.addView(emptyRow);
//        table.addView(row3);
//        table.addView(row);
//        table.addView(row2);
//
//        table.addView(delRow);
    }

    public void updateAddedBugsView(){
        LinearLayout info=(LinearLayout)findViewById(R.id.addedBugsContent);
        for (ScoutBug bug:allBugs) {
            View bugInfo = ((LayoutInflater) getSystemService(Context.LAYOUT_INFLATER_SERVICE)).inflate(R.layout.bug_info, null);
            ((ImageView) bugInfo.findViewById(R.id.bugInfoImage)).setImageBitmap(Bitmap.createScaledBitmap(BitmapFactory.decodeByteArray(bug.getFieldPicture(), 0, bug.getFieldPicture().length),150,150,true));
            ((TextView) bugInfo.findViewById(R.id.bugInfoText)).setText(bug.getNumberOfBugs() + "");
            info.addView(bugInfo);
        }
    }

    public void storeCurrentBug(){
        currBug = new ScoutBug();
        if(species != null){
            currBug.setSpecies(species);
        }
        //currBug.setSpecies(species);
        if(imageTaken != null){
            ByteArrayOutputStream stream = new ByteArrayOutputStream();
            imageTaken.compress(Bitmap.CompressFormat.JPEG,100,stream);
            currBug.setFieldPicture(stream.toByteArray());
        }

      //  TableRow rowNumberPicker;
//        if(first) {
     //       rowNumberPicker = (TableRow) table.getChildAt(table.getChildCount() - 3);
//            first = false;
//        }
//        else {
//            rowNumberPicker = (TableRow) table.getChildAt(table.getChildCount() - 4);
//        }

        NumberPicker currNumberPicker = (NumberPicker) findViewById(R.id.npNumBugs);
//        //NumberPicker currNumberPicker = (NumberPicker)findViewById(R.id.npNumBugs1);
        Log.e("Look", String.valueOf(currNumberPicker.getValue()));
        currBug.setNumberOfBugs(currNumberPicker.getValue());
        currBug.setSpecies(species);
        allBugs.add(currBug);
        // currBug



       // currBug.setNumberOfBugs(numBugs);

        //TODO: change to user id eventually
//        sb.setLastModifiedID(1);
//        sb.setTMStamp(new Date());
//        stop.ScoutBugs.add(sb); .setSpecies(spec);
//        sb.setNumberOfBugs(numBugs);
//        ByteArrayOutputStream stream = new ByteArrayOutputStream();
//        fieldImg.compress(Bitmap.CompressFormat.JPEG,100,stream);
//        sb.setFieldPicture(stream.toByteArray());
//        //TODO: change to user id eventually
//        sb.setLastModifiedID(1);
//        sb.setTMStamp(new Date());
//        stop.ScoutBugs.add(sb);
    }
}
