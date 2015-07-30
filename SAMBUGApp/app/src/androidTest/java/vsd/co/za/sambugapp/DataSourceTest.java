package vsd.co.za.sambugapp;

import android.test.AndroidTestCase;
import android.test.RenamingDelegatingContext;

import java.lang.Override;
import java.util.Date;
import java.util.List;

import vsd.co.za.sambugapp.DataAccess.DBHelper;
import vsd.co.za.sambugapp.DataAccess.ScoutBugDAO;
import vsd.co.za.sambugapp.DataAccess.ScoutStopDAO;
import vsd.co.za.sambugapp.DataAccess.SpeciesDAO;
import vsd.co.za.sambugapp.DataAccess.UserDAO;
import vsd.co.za.sambugapp.DomainModels.ScoutBug;
import vsd.co.za.sambugapp.DomainModels.ScoutStop;
import vsd.co.za.sambugapp.DomainModels.Species;
import vsd.co.za.sambugapp.DomainModels.User;


/**
 * Created by Aeolus on 2015-07-17.
 * Tests the DAO objects and their implementations
 */
public class DataSourceTest extends AndroidTestCase {
    private DBHelper db;
    RenamingDelegatingContext context;
    @Override
    public void setUp() throws Exception {
        super.setUp();
         context = new RenamingDelegatingContext(getContext(),"_test");
        db = new DBHelper(context);
    }

    @Override
    public void tearDown() throws Exception {
        db.close();
        super.tearDown();
    }

    public void testScoutStopCRD() throws Exception{
        ScoutStopDAO scoutStopDAO = new ScoutStopDAO(context);
        scoutStopDAO.open();
        ScoutStop scoutStop = new ScoutStop();
        scoutStop.setDate(new Date());
        scoutStop.setTMStamp(new Date());

        int id = (int)scoutStopDAO.insert(scoutStop);

        assertEquals("Insertion couldn't take place",true,id >= 0);
        scoutStop.setScoutStopID(id);

        ScoutStop returnedBug = scoutStopDAO.getScoutStopByID(id);

        assertEquals(scoutStop.getScoutStopID(), returnedBug.getScoutStopID());

        ScoutStop newScoutStop = new ScoutStop();
        newScoutStop.setDate(new Date());
        newScoutStop.setTMStamp(new Date());
        newScoutStop.setScoutStopID((int) scoutStopDAO.insert(newScoutStop));

        List<ScoutStop> all = scoutStopDAO.getAllScoutStops();

        assertEquals(2,scoutStopDAO.getAllScoutStops().size());

        assertEquals(all.get(0).getScoutStopID(),scoutStop.getScoutStopID());

        assertEquals(all.get(1).getScoutStopID(),newScoutStop.getScoutStopID());

        assertEquals(false, scoutStopDAO.isEmpty());

        scoutStopDAO.delete(scoutStop);
        assertEquals(false, scoutStopDAO.isEmpty());

        all = scoutStopDAO.getAllScoutStops();

        assertEquals(all.get(0).getScoutStopID(),newScoutStop.getScoutStopID());

        scoutStopDAO.delete(newScoutStop);

        assertEquals(true,scoutStopDAO.isEmpty());


        scoutStopDAO.close();
    }

    public void testSpeciesCRD() throws Exception{
        SpeciesDAO speciesDAO = new SpeciesDAO(context);
        speciesDAO.open();
            Species species = new Species();
            species.setSpeciesName("TestSpecies");
            species.setIdealPicture(new byte[]{});
            species.setTMStamp(new Date());
        
            int id = (int)speciesDAO.insert(species);
            
            assertEquals("Insertion couldn't take place",true,id >= 0);
            species.setSpeciesID(id);

        Species returnedBug = speciesDAO.getSpeciesByID(id);
        assertEquals(species.getSpeciesName(), returnedBug.getSpeciesName());
        assertEquals(species.getSpeciesID(), returnedBug.getSpeciesID());

        Species newSpecies = new Species();
        newSpecies.setSpeciesName("All Species");
        newSpecies.setIdealPicture(new byte[]{});
        newSpecies.setTMStamp(new Date());
        newSpecies.setSpeciesID((int) speciesDAO.insert(newSpecies));

        List<Species> all = speciesDAO.getAllSpecies();

        assertEquals(2,speciesDAO.getAllSpecies().size());

        assertEquals(all.get(0).getSpeciesName(),species.getSpeciesName());
        assertEquals(all.get(0).getSpeciesID(),species.getSpeciesID());

        assertEquals(all.get(1).getSpeciesName(),newSpecies.getSpeciesName());
        assertEquals(all.get(1).getSpeciesID(),newSpecies.getSpeciesID());

        assertEquals(false,speciesDAO.isEmpty());

        speciesDAO.delete(species);
        assertEquals(false, speciesDAO.isEmpty());

        all = speciesDAO.getAllSpecies();

        assertEquals(all.get(0).getSpeciesName(),newSpecies.getSpeciesName());
        assertEquals(all.get(0).getSpeciesID(),newSpecies.getSpeciesID());

        speciesDAO.delete(newSpecies);

        assertEquals(true,speciesDAO.isEmpty());


        speciesDAO.close();
    }

    public void testUserCRD() throws Exception{
        /*
        UserDAO userDAO = new UserDAO(context);
        userDAO.open();
        User user = new User();
        user.setEmail("TestUser");

        user.setTMStamp(new Date());

        int id = (int)userDAO.insert(user);

        assertEquals("Insertion couldn't take place",true,id >= 0);
        user.setUserID(id);

        User returnedBug = userDAO.getUserByID(id);
        assertEquals(user.getEmail(), returnedBug.getEmail());
        assertEquals(user.getUserID(), returnedBug.getUserID());

        User newUser = new User();
        newUser.setEmail("All User");
        newUser.setTMStamp(new Date());
        newUser.setUserID((int) userDAO.insert(newUser));

        List<User> all = userDAO.getAllUsers();

        assertEquals(2,userDAO.getAllUsers().size());

        assertEquals(all.get(0).getEmail(),user.getEmail());
        assertEquals(all.get(0).getUserID(),user.getUserID());

        assertEquals(all.get(1).getEmail(),newUser.getEmail());
        assertEquals(all.get(1).getUserID(),newUser.getUserID());

        assertEquals(false,userDAO.isEmpty());

        userDAO.delete(user);
        assertEquals(false, userDAO.isEmpty());

        all = userDAO.getAllUsers();

        assertEquals(all.get(0).getEmail(),newUser.getEmail());
        assertEquals(all.get(0).getUserID(),newUser.getUserID());

        userDAO.delete(newUser);

        assertEquals(true,userDAO.isEmpty());


        userDAO.close();
        */
    }

    public void testScoutBugCRD() throws Exception{
        ScoutBugDAO scoutBugDAO = new ScoutBugDAO(context);
        scoutBugDAO.open();
            ScoutBug bug = new ScoutBug();
            bug.Comments = "Testing single insert and query";
            bug.setFieldPicture(new byte[]{});
            bug.setTMStamp(new Date());
            bug.setScoutBugID((int) scoutBugDAO.insert(bug));

            int id = bug.getScoutBugID();
            assertEquals("Insertion couldn't take place", true, id >= 0);

            ScoutBug returnedBug = scoutBugDAO.getScoutBugByID(id);
            assertEquals(bug.getComments(), returnedBug.getComments());
            assertEquals(bug.getScoutBugID(), returnedBug.getScoutBugID());
            
            ScoutBug newBug = new ScoutBug();
            newBug.Comments = "Testing get all";
            newBug.setFieldPicture(new byte[]{});
            newBug.setTMStamp(new Date());
            newBug.setScoutBugID((int) scoutBugDAO.insert(newBug));

            List<ScoutBug> all = scoutBugDAO.getAllScoutBugs();

            assertEquals(2,scoutBugDAO.getAllScoutBugs().size());

            assertEquals(all.get(0).getComments(),bug.getComments());
            assertEquals(all.get(0).getScoutBugID(),bug.getScoutBugID());

            assertEquals(all.get(1).getComments(),newBug.getComments());
            assertEquals(all.get(1).getScoutBugID(),newBug.getScoutBugID());

            assertEquals(false,scoutBugDAO.isEmpty());

            scoutBugDAO.delete(bug);
            assertEquals(false, scoutBugDAO.isEmpty());

            all = scoutBugDAO.getAllScoutBugs();

            assertEquals(all.get(0).getComments(),newBug.getComments());
            assertEquals(all.get(0).getScoutBugID(),newBug.getScoutBugID());

            scoutBugDAO.delete(newBug);

            assertEquals(true,scoutBugDAO.isEmpty());


        scoutBugDAO.close();
    }
}
