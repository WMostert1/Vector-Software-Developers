//package vsd.co.za.sambugapp;
//
//import android.test.ActivityInstrumentationTestCase2;
//import android.widget.Button;
//
//import java.util.Date;
//
//import vsd.co.za.sambugapp.DomainModels.ScoutStop;
//import org.mockito.*;
//
///**
// * Created by keaganthompson on 7/14/15.
// */
//public class ScoutTripActivityTest extends ActivityInstrumentationTestCase2<ScoutTripActivity>{
//
//    private ScoutTripActivity scoutTripActivity;
//
//    public ScoutTripActivityTest(){
//        super(ScoutTripActivity.class);
//    }
//
//    @Override
//    public void setUp() throws Exception {
//        super.setUp();
//        scoutTripActivity=getActivity();
//    }
//
//    public void testPreconditions() throws Exception {
//        assertNotNull("ScoutTripActivity is null", scoutTripActivity);
//    }
//
//    public void testAddStop() throws Exception {
//
//        final int expectedResult=1;
//
//        scoutTripActivity.addStop(new ScoutStop());
//        final int actualResult=scoutTripActivity.scoutTrip.getNumStops();
//
//        assertEquals(actualResult, expectedResult);
//    }
//
//    public void testUpdateStop() throws Exception {
//
//        final int expectedResult=10;
//
//        ScoutStop stop=new ScoutStop();
//        stop.setNumberOfTrees(5);
//        scoutTripActivity.addStop(stop);
//        ScoutStop updateStop=new ScoutStop();
//        updateStop.setNumberOfTrees(10);
//        scoutTripActivity.updateIndex=0;
//        scoutTripActivity.updateStop(updateStop);
//
//        final int actualResult = scoutTripActivity.scoutTrip.getStop(0).getNumberOfTrees();
//
//        assertEquals(actualResult,expectedResult);
//    }
//
//    public void testPersistData() throws Exception {
//
//        final boolean expectedResult = true;
//
//        ScoutStop stop=new ScoutStop();
//        stop.setUserID(1);
//        stop.setBlockID(1);
//        stop.setNumberOfTrees(10);
//        stop.setLongitude(12);
//        stop.setLatitude(12);
//        stop.setDate(new Date());
//        stop.setLastModifiedID(1);
//        stop.setTMStamp(new Date());
//        scoutTripActivity.scoutTrip.addStop(stop);
//        final boolean actualResult = scoutTripActivity.persistData();
//
//        assertEquals(actualResult,expectedResult);
//
//    }
//
//    public void testFinishActivity() throws Exception {
//
//        final boolean expectedResult=true;
//
//        Button button=(Button)scoutTripActivity.findViewById(R.id.btnFinishTrip);
//        scoutTripActivity.finishTrip(button);
//        final boolean actualResult=scoutTripActivity.isFinishing();
//
//        assertEquals(expectedResult,actualResult);
//    }
//}
