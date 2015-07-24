package vsd.co.za.sambugapp;
import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.test.ActivityInstrumentationTestCase2;
import android.widget.Spinner;

import org.junit.Test;
import org.junit.runner.RunWith;

import java.util.HashSet;

import vsd.co.za.sambugapp.DomainModels.Block;
import vsd.co.za.sambugapp.DomainModels.Farm;
import vsd.co.za.sambugapp.DomainModels.ScoutStop;

import org.mockito.Mock;
import org.mockito.Mockito;

import static org.mockito.Matchers.notNull;
import static org.mockito.Mockito.CALLS_REAL_METHODS;
import static org.mockito.Mockito.doReturn;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.when;


/**
 * Created by Kale-ab on 2015-07-15.
 */
public class EnterDataActivityTests extends ActivityInstrumentationTestCase2<enterDataActivity> {

        public static final String SCOUT_STOP="za.co.vsd.scout_stop";
        private final String UPDATE_INDEX="za.co.vsd.update_index";
        public static final String USER_FARM="za.co.vsd.user_blocks";
        public final String SCOUT_STOP_LIST="za.co.vsd.scout_stop_list";


        public EnterDataActivityTests() {
                super(enterDataActivity.class);

        }

        @Test
        public void testGeoLocation(){
                // enterDataActivity activity = mock(enterDataActivity.class, CALLS_REAL_METHODS);
                enterDataActivity activity = Mockito.spy(new enterDataActivity());
                //activity.CheckIfGPSON() =
                doReturn(true).when(activity).CheckIfGPSON();
                // Mockito.when(activity.CheckIfGPSON()).thenReturn(true);
                //   when(activity.receiveGeoLocation()).thenCallRealMethod();
                //  activity.CheckIfGPSON();
                assertNotNull("Testing gps location", Mockito.when(activity.receiveGeoLocation()).thenCallRealMethod());
        }












//        @Test
//        public void testStartActivity() {
//
////                Farm fm = new Farm();
////
////                HashSet<Block> blocks = new HashSet<Block>();
////                Block blockA = new Block();
////                blockA.setBlockName("BlockA");
////                Block blockB = new Block();
////                blockB.setBlockName("BlockB");
////                Block blockC = new Block();
////                blockC.setBlockName("BlockC");
////
////                blocks.add(blockA);
////                blocks.add(blockB);
////                blocks.add(blockC);
////
////                ScoutTripActivity sta = mock(ScoutTripActivity.class);
////                when(sta.addStopActivityStart();)
////                enterDataActivity activity = null;
////
////
////                new enterDataActivity();
////                assertNotNull(activity);
//        }
//
//        @Test
//        public void testLoadingBlocks(){
////                ScoutStop sp = new ScoutStop();
////
////                Farm fm = new Farm();
////
////                HashSet<Block> blocks = new HashSet<Block>();
////                Block blockA = new Block();
////                blockA.setBlockName("BlockA");
////                Block blockB = new Block();
////                blockB.setBlockName("BlockB");
////                Block blockC = new Block();
////                blockC.setBlockName("BlockC");
////
////                blocks.add(blockA);
////                blocks.add(blockB);
////                blocks.add(blockC);
////
////                fm.setBlocks(blocks);
////
////
////                enterDataActivity activity = mock(enterDataActivity.class);
////                //enterDataActivity activity = getActivity();
////              //  when(activity.getFarm()).thenReturn(fm);
////
////
////                Intent iReceive = new Intent();
////
////                Bundle bundle=new Bundle();
////                bundle.putSerializable(USER_FARM, fm);
////                iReceive.putExtras(bundle);
////
////                when(activity.acceptBlocks(iReceive)).then();
////
////                activity.acceptBlocks(iReceive);
////                activity.populateSpinner();
////                //Spinner spnNumBlocks = (Spinner)activity.findViewById(R.id.spnBlocks);
////                /*
////                Testing to see if the num of blocks is correct.
////                 */
////                assertEquals(3, activity.getMySpin().getCount());
////
////
//
//
//        }
//
//        @Test
//        public void testAcceptStop(){
////                enterDataActivity activity = mock(enterDataActivity.class);
////                activity.populateSpinner();
//        }

}
