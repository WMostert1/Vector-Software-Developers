package vsd.co.za.sambugapp;

import java.util.ArrayList;

/**
 * Created by keaganthompson on 7/9/15.
 */
public class ScoutTrip {

    private ArrayList<ScoutStop> scoutStops;

    public ScoutTrip(){
        scoutStops=new ArrayList<>();
    }

    public void addTrip(ScoutStop scoutStop){
        scoutStops.add(scoutStop);
    }

    public ScoutStop getTrip(int index){
        return scoutStops.get(index);
    }

    public int getNumTrips(){
        return scoutStops.size();
    }

}
