package vsd.co.za.sambugapp.DomainModels;

import java.lang.reflect.Array;
import java.util.ArrayList;

import vsd.co.za.sambugapp.DomainModels.ScoutStop;

/**
 * Created by keaganthompson on 7/9/15.
 */
public class ScoutTrip {

    private ArrayList<ScoutStop> scoutStops;

    public ScoutTrip(){
        scoutStops=new ArrayList<>();

    }

    public void addStop(ScoutStop scoutStop){
        scoutStops.add(scoutStop);
    }

    public void updateStop(ScoutStop scoutStop, int position){
        scoutStops.set(position,scoutStop);
    }

    public ScoutStop getStop(int index){
        return scoutStops.get(index);
    }

    public ArrayList<ScoutStop> getStopList(){
        return scoutStops;
    }

    public int getNumStops(){
        return scoutStops.size();
    }

}
