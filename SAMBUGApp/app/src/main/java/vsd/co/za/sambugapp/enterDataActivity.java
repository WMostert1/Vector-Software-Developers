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
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
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

    //constants for saved variables
    private final String BUG_LIST = "za.co.vsd.bug_list";
    private final String SCOUT_STOP = "za.co.vsd.scout_stop";
    private final String BLOCK_INFO_COLLAPSED = "za.co.vsd.block_info_collapsed";
    private final String HAS_BUGS = "za.co.vsd.has_bugs";

    //UI variables
    private boolean blockInfoCollapsed = false;
    private boolean hasBugs = false;

    ScoutStop scoutStop;
    Spinner mySpin;
    NumberPicker npTrees;
    Farm farm;
    HashSet<ScoutBug> listAddedBugs;
    RecyclerView rvAddedBugs;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_enter_data);

        //Get user farm
        farm = getFarm();

        //Create base scout stop object
        scoutStop = createScoutStop();

        //Set toolbar (ActionBar)
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle(null);
        CheckedTextView txtFarmName = (CheckedTextView) toolbar.findViewById(R.id.txtFarmName);
        txtFarmName.setText(farm.getFarmName());

        //check for previous activity state
        if (savedInstanceState != null) {
            listAddedBugs = (HashSet<ScoutBug>) savedInstanceState.get(BUG_LIST);
            blockInfoCollapsed = savedInstanceState.getBoolean(BLOCK_INFO_COLLAPSED);
            hasBugs = savedInstanceState.getBoolean(HAS_BUGS);
        } else {
            listAddedBugs = new HashSet<>();
        }

        //set bug list adapter
        rvAddedBugs = (RecyclerView) findViewById(R.id.rvAddedBugs);
        rvAddedBugs.setLayoutManager(new LinearLayoutManager(getApplicationContext()));
        rvAddedBugs.setAdapter(new RVAddedBugsAdapter(listAddedBugs));
        rvAddedBugs.setHasFixedSize(true);

        //add default instruction item if no bugs added
        if (!hasBugs) {
            listAddedBugs.add(null);
        }

        rvAddedBugs.getAdapter().notifyDataSetChanged();

        if (!blockInfoCollapsed) {
            expandScoutStopDetails(null);

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
        savedInstanceState.putSerializable(SCOUT_STOP, scoutStop);
        savedInstanceState.putBoolean(BLOCK_INFO_COLLAPSED, blockInfoCollapsed);
        savedInstanceState.putBoolean(HAS_BUGS, hasBugs);
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
            //remove default instruction item
            if (!hasBugs) {
                listAddedBugs.clear();
                hasBugs = true;
            }
            //get activity variables for bug
            Bundle speciesReceived = data.getExtras();
            Species species = (Species) speciesReceived.get(IdentificationActivity.IDENTIFICATION_SPECIES);
            Bitmap imageTaken = speciesReceived.getParcelable(IdentificationActivity.FIELD_BITMAP);
            int bugCount = speciesReceived.getInt(IdentificationActivity.BUG_COUNT);
            //add bug to list
            addBug(imageTaken, species, bugCount);
        }
    }

    private ScoutStop createScoutStop() {
        Location location = getGeoLocation();
        ScoutStop stop = new ScoutStop();
        stop.setDate(new Date());
        stop.setLatitude((float) location.getLatitude());
        stop.setLongitude((float) location.getLongitude());
        stop.setNumberOfTrees(0);
        return stop;
    }

    /**
     * Gets the blocks from the farm object passed.
     */
    public Farm getFarm() {
        Bundle bundle = getIntent().getExtras();
        Farm farm = (Farm) bundle.get(ScoutTripActivity.USER_FARM);
        return farm;
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
        if (scoutStop.getBlock() != null) {
            for (int i = 0; i < mySpin.getCount(); i++) {
                if (((Block) (mySpin.getItemAtPosition(i))).getBlockName().equals(scoutStop.getBlock().getBlockName())) {
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
        npTrees.setValue(scoutStop.getNumberOfTrees());
    }

    public void sendToScoutTripActivity(View view) {
        Intent output = new Intent();
        Bundle b = new Bundle();
        scoutStop.setScoutBugs(listAddedBugs);
        b.putSerializable(ScoutTripActivity.SCOUT_STOP, scoutStop);
        output.putExtras(b);
        setResult(RESULT_OK, output);
        finish();
    }

    public void startIdentificationActivity(View view) {
        Intent intent = new Intent(enterDataActivity.this, IdentificationActivity.class);
        startActivityForResult(intent, 0);
    }

    /**
     * Receives the Location Data from the device.
     */
    public Location getGeoLocation() {
        LocationManager mLocationManager = (LocationManager) getSystemService(Context.LOCATION_SERVICE);
        LocationListener locationListener = new MyLocationListener();
        if (!mLocationManager.isProviderEnabled(LocationManager.GPS_PROVIDER)) {
            Intent gpsOptionsIntent = new Intent(android.provider.Settings.ACTION_LOCATION_SOURCE_SETTINGS);
            startActivity(gpsOptionsIntent);
        }
        try {
            mLocationManager.requestSingleUpdate(LocationManager.GPS_PROVIDER, locationListener, null);
            Location location = getLastKnownLocation(mLocationManager); //mLocationManager.getLastKnownLocation(LocationManager.GPS_PROVIDER);
            return location;
        } catch (SecurityException ex) {
            ex.printStackTrace();
        }
        return null;
    }


    /**
     * Gets the last known location if GPS is on. Else it goes to an error screen.
     * @return Location object.
     */
    private synchronized Location getLastKnownLocation(LocationManager mLocationManager) {
        mLocationManager = (LocationManager)getApplicationContext().getSystemService(LOCATION_SERVICE);
        List<String> providers = mLocationManager.getProviders(true);
        Location bestLocation = null;
        try {
            for (String provider : providers) {
                Location l = mLocationManager.getLastKnownLocation(provider);
                if (l == null) {
                    continue;
                } else {
                    bestLocation = l;
                }
            }
        } catch (SecurityException ex) {
            ex.printStackTrace();
        }

        return bestLocation;
    }


    /**
     * Adds a bug
     *
     */
    public void addBug(Bitmap fieldPic, Species idSpecies, int bugCount) {
        ScoutBug temp = new ScoutBug();
        if (idSpecies != null) {
            temp.setSpecies(idSpecies);
        }
        if (fieldPic != null) {
            ByteArrayOutputStream stream = new ByteArrayOutputStream();
            fieldPic.compress(Bitmap.CompressFormat.JPEG, 100, stream);
            temp.setFieldPicture(stream.toByteArray());
        }
        temp.setNumberOfBugs(bugCount);
        listAddedBugs.add(temp);
        rvAddedBugs.getAdapter().notifyDataSetChanged();
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
            scoutStop.setBlockID(tempBlock.getBlockID());
            scoutStop.setBlock(tempBlock);
            scoutStop.setNumberOfTrees(npTrees.getValue());
        }
        layout.removeAllViews();
        View.inflate(enterDataActivity.this, R.layout.collapsed_scout_stop_details, layout);
        TextView lblBlockName = (TextView) layout.findViewById(R.id.lblBlockName);
        lblBlockName.setText(scoutStop.getBlock().getBlockName());
        TextView lblNumTrees = (TextView) layout.findViewById(R.id.lblNumTrees);
        lblNumTrees.setText(scoutStop.getNumberOfTrees() + "");
        FloatingActionButton openButton = (FloatingActionButton) findViewById(R.id.fabAddBug);
        openButton.setVisibility(View.VISIBLE);
    }

    public void expandScoutStopDetails(View v) {
        //hide added bugs
        LinearLayout bugLayout = (LinearLayout) findViewById(R.id.layoutAddedBugs);
        bugLayout.setVisibility(View.GONE);

        //hide FAB
        FloatingActionButton fab = (FloatingActionButton) findViewById(R.id.fabAddBug);
        fab.setVisibility(View.INVISIBLE);

        LinearLayout layout = (LinearLayout) findViewById(R.id.scoutStopDetailsLayout);
        //remove layout children
        layout.removeAllViews();
        //set new layout to expanded version
        View.inflate(enterDataActivity.this, R.layout.expanded_scout_stop_details, layout);
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
            if (hasBugs) {
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
            } else {
                addedBugViewHolder.ivAddedBugPic.setMaxWidth(0);
                addedBugViewHolder.tvAddedBugSpecies.setText("No bugs added yet. Click '+' to add.");
                addedBugViewHolder.tvAddedBugCount.setText("");
            }
        }
    }

}
