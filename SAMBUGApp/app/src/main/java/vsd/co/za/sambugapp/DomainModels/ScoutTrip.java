package vsd.co.za.sambugapp.DomainModels;

import java.lang.reflect.Array;
import java.util.ArrayList;

import vsd.co.za.sambugapp.DomainModels.ScoutStop;

/**
 * Created by keaganthompson on 7/9/15.
 */
public class ScoutTrip {

    private ArrayList<ScoutStop> scoutStops;

    public ScoutTrip() {
        //mock generation
        scoutStops = new ArrayList<>();
        ScoutStop[] objects = new ScoutStop[5];
        for (int i = 0; i < 5; i++) {
            objects[i] = new ScoutStop();
            objects[i].setNumberOfTrees(i + 1);
            scoutStops.add(objects[i]);
        }

    }

    public void addStop(ScoutStop scoutStop) {
        scoutStops.add(scoutStop);
    }

    public ScoutStop getStop(int index) {
        return scoutStops.get(index);
    }

    public ArrayList<ScoutStop> getStopList() {
        return scoutStops;
    }

    public int getNumStops() {
        return scoutStops.size();
    }

}
