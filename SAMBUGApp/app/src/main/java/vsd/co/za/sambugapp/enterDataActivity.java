package vsd.co.za.sambugapp;

import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.CardView;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.CheckedTextView;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.NumberPicker;
import android.widget.Spinner;
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


public class enterDataActivity extends AppCompatActivity {
    private final String BUG_LIST = "za.co.vsd.bug_list";
    private final String SCOUT_STOP = "za.co.vsd.scout_stop";
    private final String BLOCK_INFO_COLLAPSED = "za.co.vsd.block_info_collapsed";
    private boolean blockInfoCollapsed = false;
    private int bugCount = -1;
    ScoutStop stop = null;
    Species species;
    Spinner mySpin;
    NumberPicker npTrees;
    ScoutBug currBug;
    Farm farm;
    Bitmap imageTaken;
    HashSet<ScoutBug> listAddedBugs;
    RecyclerView rvAddedBugs;

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

        acceptBlocks();
        acceptStop(savedInstanceState);

        //set toolbar (ActionBar)
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle(null);
        CheckedTextView txtFarmName = (CheckedTextView) toolbar.findViewById(R.id.txtFarmName);
        txtFarmName.setText(farm.getFarmName());

        listAddedBugs = new HashSet<>();
        if (savedInstanceState != null) {
            listAddedBugs = (HashSet<ScoutBug>) savedInstanceState.get(BUG_LIST);
            blockInfoCollapsed = savedInstanceState.getBoolean(BLOCK_INFO_COLLAPSED);
            updateAddedBugsView();
        }
        receiveGeoLocation();

        rvAddedBugs = (RecyclerView) findViewById(R.id.rvAddedBugs);
        rvAddedBugs.setLayoutManager(new LinearLayoutManager(getApplicationContext()));
        rvAddedBugs.setAdapter(new RVAddedBugsAdapter(listAddedBugs));
        rvAddedBugs.setHasFixedSize(true);

        if (!blockInfoCollapsed) {
            expandScoutStopDetails(null);
            //RelativeLayout layout = (RelativeLayout) findViewById(R.id.bugDetailsLayout);
            //layout.setVisibility(View.INVISIBLE);
            FloatingActionButton btn = (FloatingActionButton) findViewById(R.id.fabAddBug);
            btn.setVisibility(View.INVISIBLE);
        } else {
            collapseScoutStopDetails(null);
        }
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
    public void onSaveInstanceState(Bundle savedInstanceState) {
        super.onSaveInstanceState(savedInstanceState);
        savedInstanceState.putSerializable(BUG_LIST, listAddedBugs);
        savedInstanceState.putSerializable(SCOUT_STOP, stop);
        savedInstanceState.putBoolean(BLOCK_INFO_COLLAPSED, blockInfoCollapsed);
    }

    @Override
    protected void onPause() {
        super.onPause();
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
            Bitmap imageTaken2 = (Bitmap) speciesReceived.getParcelable("Image");
            imageTaken = imageTaken2;
            bugCount = speciesReceived.getInt(IdentificationActivity.BUG_COUNT);
            addBug();
        }
    }

    /**
     * Accepts the Stop Object. If no object is found, it creates one.
     */
    public void acceptStop(Bundle savedInstanceState) {
        if (savedInstanceState != null) {
            stop = (ScoutStop) savedInstanceState.getSerializable(SCOUT_STOP);
        } else {
            createScoutStop();
        }
    }

    /**
     * Gets the blocks from the farm object passed.
     */
    public Farm acceptBlocks() {
        Bundle scoutStop = getIntent().getExtras();
        Farm frm = (Farm) scoutStop.get(ScoutTripActivity.USER_FARM);
        if (frm != null) {
            setFarm(frm);
        } else Log.e("Error", "No block exists!");
        return frm;
    }

    /**
     * Populates the spinners with the appropriate blocks.
     */
    public void populateSpinner() {
        mySpin = (Spinner) findViewById(R.id.spnBlocks);

        HashSet<Block> blockArray;
        blockArray = farm.getBlocks();

        List<Block> list = new ArrayList<Block>(blockArray);

        ArrayAdapter<Block> adapter = new ArrayAdapter<Block>(this, android.R.layout.simple_spinner_item, list);
        adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        mySpin.setAdapter(adapter);
        int pos = 0;
        if (stop.getBlock() != null) {
            for (int i = 0; i < mySpin.getCount(); i++) {
                if (((Block) (mySpin.getItemAtPosition(i))).getBlockName().equals(stop.getBlock().getBlockName())) {
                    pos = i;
                    break;
                }
            }
        }
        mySpin.setSelection(pos);
    }

    public void initialiseTreeNumberPicker() {
        npTrees = (NumberPicker) findViewById(R.id.npNumTrees);
        npTrees.setMinValue(1);
        npTrees.setMaxValue(100);
        npTrees.setWrapSelectorWheel(false);
        npTrees.setValue(stop.getNumberOfTrees());
    }

    public void sendToScoutTripActivity(View view) {

        Intent output = new Intent();
        Bundle b = new Bundle();
        stop.setScoutBugs(listAddedBugs);
        b.putSerializable(ScoutTripActivity.SCOUT_STOP, stop);
        output.putExtras(b);
        setResult(RESULT_OK, output);
        finish();
    }

    public void sendToIdentificationActivity(View view) {

        Intent intent = new Intent(enterDataActivity.this, IdentificationActivity.class);
        startActivityForResult(intent, 0);
    }

    /**
     * Receives the Location Data from the device.
     */
    public Location receiveGeoLocation() {
        mLocationManager = (LocationManager) getSystemService(Context.LOCATION_SERVICE);
        LocationListener locationListener = new MyLocationListener();
        if (!CheckIfGPSON()) {
            Intent gpsOptionsIntent = new Intent(
                    android.provider.Settings.ACTION_LOCATION_SOURCE_SETTINGS);
            startActivity(gpsOptionsIntent);
        } else {
            try {
                mLocationManager.requestSingleUpdate(LocationManager.GPS_PROVIDER, locationListener, null);
                myLocation = getLastKnownLocation(); //mLocationManager.getLastKnownLocation(LocationManager.GPS_PROVIDER);
            } catch (SecurityException ex) {
                ex.printStackTrace();
            }
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
        try {
            if (mLocationManager.isProviderEnabled(LocationManager.GPS_PROVIDER)) {
                for (String provider : providers) {
                    Location l = mLocationManager.getLastKnownLocation(provider);
                    if (l == null) {
                        continue;
                    } else {
                        bestLocation = l;
                    }
                }
            } else {
                //createErrorMessage();
            }
        } catch (SecurityException ex) {
            ex.printStackTrace();
        }

        return bestLocation;
    }

    public void setFarm(Farm farm) {
        this.farm = farm;
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
        if(myLocation != null){
            stop.setLatitude((float) myLocation.getLatitude());
            stop.setLongitude((float) myLocation.getLongitude());
        }
        stop.setNumberOfTrees(0);
    }


    /**
     * Adds a bug
     *
     */
    public void addBug() {
        storeCurrentBug();
        updateAddedBugsView();

    }

    public void collapseScoutStopDetails(View v) {
        //show added bugs
        LinearLayout bugLayout = (LinearLayout) findViewById(R.id.layoutAddedBugs);
        bugLayout.setVisibility(View.VISIBLE);
        LinearLayout layout = (LinearLayout) findViewById(R.id.scoutStopDetailsLayout);
        Block tempBlock = null;
        if (v != null) {
            blockInfoCollapsed = true;
            String blockName = mySpin.getSelectedItem().toString();
            for (Block b : farm.getBlocks()) {
                if (b.getBlockName().equals(blockName)) {
                    tempBlock = b;
                    break;
                }
            }
            stop.setBlockID(tempBlock.getBlockID());
            stop.setBlock(tempBlock);
            stop.setNumberOfTrees(npTrees.getValue());
        }
        layout.removeAllViews();
        layout.inflate(enterDataActivity.this, R.layout.collapsed_scout_stop_details, layout);
        TextView lblBlockName = (TextView) layout.findViewById(R.id.lblBlockName);
        lblBlockName.setText(stop.getBlock().getBlockName());
        TextView lblNumTrees = (TextView) layout.findViewById(R.id.lblNumTrees);
        lblNumTrees.setText(stop.getNumberOfTrees() + "");
        //RelativeLayout openLayout = (RelativeLayout) findViewById(R.id.bugDetailsLayout);
        //openLayout.setVisibility(View.VISIBLE);
        FloatingActionButton openButton = (FloatingActionButton) findViewById(R.id.fabAddBug);
        openButton.setVisibility(View.VISIBLE);
    }

    public void expandScoutStopDetails(View v) {
        //hide added bugs
        LinearLayout bugLayout = (LinearLayout) findViewById(R.id.layoutAddedBugs);
        bugLayout.setVisibility(View.GONE);

        LinearLayout layout = (LinearLayout) findViewById(R.id.scoutStopDetailsLayout);
        //remove layout children
        layout.removeAllViews();
        //set new layout to expanded version
        layout.inflate(enterDataActivity.this, R.layout.expanded_scout_stop_details, layout);
        //initialise pickers and stuff
        populateSpinner();
        initialiseTreeNumberPicker();
        //button click listener
        Button btn = (Button) layout.findViewById(R.id.btnCollapse);
        btn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                collapseScoutStopDetails(v);
            }
        });
    }

    public void updateAddedBugsView(){
       /* RecyclerView info=(LinearLayout)findViewById(R.id.addedBugsContent);
        info.removeAllViews();
        for (ScoutBug bug: listAddedBugs) {
            View bugInfo = ((LayoutInflater) getSystemService(Context.LAYOUT_INFLATER_SERVICE)).inflate(R.layout.bug_info, null);
            ((ImageView) bugInfo.findViewById(R.id.bugInfoImage)).setImageBitmap(Bitmap.createScaledBitmap(BitmapFactory.decodeByteArray(bug.getFieldPicture(), 0, bug.getFieldPicture().length),150,150,true));
            ((TextView) bugInfo.findViewById(R.id.bugInfoText)).setText(bug.getNumberOfBugs() + "");
            info.addView(bugInfo);
        }*/
        RecyclerView rv = (RecyclerView) findViewById(R.id.rvAddedBugs);
        rv.getAdapter().notifyDataSetChanged();
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
            imageTaken.compress(Bitmap.CompressFormat.JPEG, 100, stream);
            currBug.setFieldPicture(stream.toByteArray());
        }
        currBug.setNumberOfBugs(bugCount);
        currBug.setSpecies(species);
        currBug.setSpeciesID(species.getSpeciesID());
        ByteArrayOutputStream stream = new ByteArrayOutputStream();
        imageTaken.compress(Bitmap.CompressFormat.JPEG, 100, stream);
        currBug.setFieldPicture(stream.toByteArray());
        stop.ScoutBugs.add(currBug);
        listAddedBugs.add(currBug);

    }

    public class RVAddedBugsAdapter extends RecyclerView.Adapter<RVAddedBugsAdapter.AddedBugViewHolder> {

        HashSet<ScoutBug> bugs;

        public RVAddedBugsAdapter(HashSet<ScoutBug> bugs) {
            this.bugs = bugs;
        }

        public class AddedBugViewHolder extends RecyclerView.ViewHolder {
            ImageView ivAddedBugPic;
            CheckedTextView tvAddedBugSpecies;
            CheckedTextView tvAddedBugCount;

            AddedBugViewHolder(View itemView) {
                super(itemView);
                tvAddedBugSpecies = (CheckedTextView) itemView.findViewById(R.id.tvAddedBugSpecies);
                tvAddedBugCount = (CheckedTextView) itemView.findViewById(R.id.tvAddedBugCount);
                ivAddedBugPic = (ImageView) itemView.findViewById(R.id.ivAddedBugPic);
            }
        }

        @Override
        public int getItemCount() {
            return bugs.size();
        }

        @Override
        public AddedBugViewHolder onCreateViewHolder(ViewGroup viewGroup, int i) {
            View v = LayoutInflater.from(viewGroup.getContext()).inflate(R.layout.list_added_bug, viewGroup, false);
            AddedBugViewHolder addedBugViewHolder = new AddedBugViewHolder(v);
            return addedBugViewHolder;
        }

        @Override
        public void onBindViewHolder(AddedBugViewHolder addedBugViewHolder, int i) {
            ScoutBug bug = null;
            int pos = 0;
            for (ScoutBug b : bugs) {
                if (pos++ == i)
                    bug = b;
            }
            Bitmap bm = Bitmap.createScaledBitmap(BitmapFactory.decodeByteArray(bug.getFieldPicture(), 0, bug.getFieldPicture().length), 88, 88, true);
            addedBugViewHolder.ivAddedBugPic.setImageBitmap(bm);
            addedBugViewHolder.tvAddedBugSpecies.setText(bug.getSpecies().getSpeciesName() + " Instar " + bug.getSpecies().getLifestage());
            addedBugViewHolder.tvAddedBugCount.setText(bug.getNumberOfBugs() + "");
        }
    }

}
