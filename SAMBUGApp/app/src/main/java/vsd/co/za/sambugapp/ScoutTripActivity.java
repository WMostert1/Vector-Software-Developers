package vsd.co.za.sambugapp;

import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.RelativeLayout;
import android.widget.TextView;

import java.io.ByteArrayInputStream;
import java.util.ArrayList;
import java.util.HashSet;

import vsd.co.za.sambugapp.DomainModels.*;


public class ScoutTripActivity extends ActionBarActivity {

    public static final String SCOUT_STOP="za.co.vsd.scout_stop";
    private final String UPDATE_INDEX="za.co.vsd.update_index";
    public static final String USER_FARM="za.co.vsd.user_blocks";
    private final int NEW_STOP=0;
    private final int UPDATE_STOP=1;
    private final String TAG="ScoutTripActivity";

    private ScoutTrip scoutTrip;
    private ListView lstStops;
    private ListView lstPestsPerTree;
    private ScoutStopAdapter lstStopsAdapter;
    private PestsPerTreeAdapter lstPestsPerTreeAdapter;
    private Farm farm;

    private int updateIndex=-1;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (savedInstanceState != null) {
            updateIndex = savedInstanceState.getInt(UPDATE_INDEX);
        }
        setContentView(R.layout.activity_scout_trip);

        scoutTrip = new ScoutTrip();

        lstStops = (ListView) findViewById(R.id.lstStops);
        lstStops.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                updateStopActivityStart(position);
            }
        });
        lstStopsAdapter = new ScoutStopAdapter(scoutTrip.getStopList());
        lstStops.setAdapter(lstStopsAdapter);

        lstPestsPerTree = (ListView) findViewById(R.id.lstPestsPerTree);
        lstPestsPerTreeAdapter = new PestsPerTreeAdapter(scoutTrip.getStopList());
        lstPestsPerTree.setAdapter(lstPestsPerTreeAdapter);

        acceptFarm(getIntent());
    }

    @Override
    public void onSaveInstanceState(Bundle savedInstanceState){
        super.onSaveInstanceState(savedInstanceState);
        savedInstanceState.putInt(UPDATE_INDEX, updateIndex);
    }

    public void addStopActivityStart(View v){
        Intent intent=new Intent(this,enterDataActivity.class);
        Bundle b = new Bundle();
        b.putSerializable(SCOUT_STOP,null);
        b.putSerializable(USER_FARM,farm);
        intent.putExtras(b);
        startActivityForResult(intent, NEW_STOP);
        //handle new stop object
    }

    private void addStop(ScoutStop stop){
        scoutTrip.addStop(stop);
    }

    public void updateStopActivityStart(int position){
        //Enter EnterDataActivity for editing the stop
        updateIndex=position;
        Intent intent=new Intent(this,enterDataActivity.class);
        Bundle bundle=new Bundle();
        bundle.putSerializable(SCOUT_STOP, scoutTrip.getStop(position));
        bundle.putSerializable(USER_FARM, farm);
        intent.putExtras(bundle);
        startActivityForResult(intent, UPDATE_STOP);
    }

    public void updateStop(ScoutStop stop){
        scoutTrip.getStopList().set(updateIndex,stop);
    }

    public void acceptFarm(Intent i){
        farm=new Farm();
        farm.setFarmID(1);
        farm.setFarmName("DEEZ NUTS");
        HashSet<Block> blocks=new HashSet<>();
        for (int j=1;j<=10;j++){
            Block obj=new Block();
            obj.setBlockID(j);
            obj.setBlockName("Block #" + j);
            blocks.add(obj);
        }
        farm.setBlocks(blocks);
        Log.d(TAG, "Intent");
        //Bundle b=i.getExtras();
        //farm=(Farm)b.get(LoginActivity.USER_FARM);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        Bundle bundle=data.getExtras();
        ScoutStop stop=(ScoutStop)bundle.get(SCOUT_STOP);
        if (requestCode == NEW_STOP && resultCode == RESULT_OK) {
            addStop(stop);
            Log.d(TAG,"Added");
        } else if (requestCode==UPDATE_STOP && resultCode==RESULT_OK){
            updateStop(stop);
            Log.d(TAG,"Updated");
        }
        lstStopsAdapter.notifyDataSetChanged();
        lstPestsPerTreeAdapter.notifyDataSetChanged();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_scout_trip, menu);
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

    private class ScoutStopAdapter extends ArrayAdapter<ScoutStop> {
        public ScoutStopAdapter(ArrayList<ScoutStop> stops) {
            super(getApplication().getApplicationContext(), 0, stops);
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {
            if (convertView == null) {
                convertView = getLayoutInflater()
                        .inflate(R.layout.list_scout_stop, null);
            }
            ScoutStop stop = getItem(position);
            TextView lblBlockName =
                    (TextView)convertView.findViewById(R.id.lblBlockName);
            lblBlockName.setText(stop.Block.getBlockName());
            TextView lblTreeAmount =
                    (TextView)convertView.findViewById(R.id.lblTreeAmount);
            lblTreeAmount.setText(stop.getNumberOfTrees() + "");
            LinearLayout hscrollBugInfo=(LinearLayout)convertView.findViewById(R.id.hscrollBugInfo);
            hscrollBugInfo.removeAllViews();
            for (ScoutBug bug:stop.getScoutBugs()) {
                ImageView img = new ImageView(this.getContext());
                Bitmap bitmap=BitmapFactory.decodeByteArray(bug.getFieldPicture(),0,bug.getFieldPicture().length);
                img.setImageBitmap(bitmap);
                img.setLayoutParams(new RelativeLayout.LayoutParams(50, 50));
                hscrollBugInfo.addView(img);
            }
            return convertView;
        }
    }

    private class PestsPerTreeAdapter extends ArrayAdapter<ScoutStop> {
        public PestsPerTreeAdapter(ArrayList<ScoutStop> stops){
            super(getApplication().getApplicationContext(),0,stops);
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {
            if (convertView==null) {
                convertView=getLayoutInflater().inflate(R.layout.list_pests_per_tree,null);
            }
            ScoutStop stop = getItem(position);
            TextView lblBlockName=(TextView)convertView.findViewById(R.id.lblBlockName);
            lblBlockName.setText(stop.getBlock().getBlockName());
            TextView lblPestsPerTree=(TextView)convertView.findViewById(R.id.lblPestsPerTree);
            lblPestsPerTree.setText(stop.getPestsPerTree() + "");
            return convertView;
        }
    }

}
