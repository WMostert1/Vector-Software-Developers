package vsd.co.za.sambugapp.UnitTests.ScoutTripActivity.tests;

import android.test.ActivityInstrumentationTestCase2;

import vsd.co.za.sambugapp.ScoutTripActivity;

/**
 * Created by keaganthompson on 7/14/15.
 */
public class ScoutTripActivityTest extends ActivityInstrumentationTestCase2<ScoutTripActivity>{

    private ScoutTripActivity scoutTripActivity;

    public ScoutTripActivityTest(){
        super(ScoutTripActivity.class);
    }

    @Override
    public void setUp() throws Exception {
        super.setUp();
        scoutTripActivity=getActivity();
    }
}
