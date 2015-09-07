package vsd.co.za.sambugapp;

import android.test.AndroidTestCase;

import com.google.gson.Gson;



/**
 * Created by Aeolus on 2015-09-06.
 */
public class NetworkingTest extends AndroidTestCase {
    private Gson gson;

    private class Role{
        public int RoleId;
        public int Type;
        public String Description;
    }

    private class Block{
        public int BlockID;
        public String BlockName;
        public Object Treatments;
        public Object ScoutStops;
    }

    private class Farm{
        public int FarmID;
        public String FarmName;
        public Block [] Blocks;
    }

    private class User{
        public int UserID;
        public String Email;
        public Role [] Roles;
        public Farm [] Farms;
    }

    private class UserWrapper{
        public User User;
    }

    @Override
    public void setUp() throws Exception {
        super.setUp();
        gson = new Gson();
    }

    public void testUserParsing () throws Exception{
        String userJson = "{\"User\":{\"UserId\":1,\"Email\":null,\"Roles\":[{\"RoleId\":1,\"Type\":1,\"Description\":\"Grower\"},{\"RoleId\":2,\"Type\":2,\"Description\":\"Admin\"}],\"Farms\":[{\"FarmID\":1,\"FarmName\":\"Werner Farm\",\"Blocks\":[{\"BlockID\":1,\"BlockName\":\"Block A\",\"Treatments\":null,\"ScoutStops\":null},{\"BlockID\":2,\"BlockName\":\"Block B\",\"Treatments\":null,\"ScoutStops\":null},{\"BlockID\":3,\"BlockName\":\"Block C\",\"Treatments\":null,\"ScoutStops\":null},{\"BlockID\":4,\"BlockName\":\"Block D\",\"Treatments\":null,\"ScoutStops\":null},{\"BlockID\":5,\"BlockName\":\"Block E\",\"Treatments\":null,\"ScoutStops\":null},{\"BlockID\":6,\"BlockName\":\"Block F\",\"Treatments\":null,\"ScoutStops\":null},{\"BlockID\":15,\"BlockName\":\"Block G\",\"Treatments\":null,\"ScoutStops\":null}]}]}}";
        User user = gson.fromJson(userJson,User.class);
        UserWrapper userWrapper = gson.fromJson(userJson,UserWrapper.class);

        String blockJson = "{\"BlockID\":1,\n" +
                "\"BlockName\":\"Block A\",\n" +
                "\"Treatments\":null,\n" +
                "\"ScoutStops\":null}";
        Block block = gson.fromJson(blockJson,Block.class);

        String farmJson = "{\"FarmID\":1,\n" +
                "\"FarmName\":\"Werner Farm\",\n" +
                "\"Blocks\":[\n" +
                "\n" +
                "{\"BlockID\":1,\n" +
                "\"BlockName\":\"Block A\",\n" +
                "\"Treatments\":null,\n" +
                "\"ScoutStops\":null},\n" +
                "\n" +
                "{\"BlockID\":2,\n" +
                "\"BlockName\":\"Block B\",\n" +
                "\"Treatments\":null,\n" +
                "\"ScoutStops\":null},\n" +
                "\n" +
                "{\"BlockID\":3,\n" +
                "\"BlockName\":\"Block C\",\n" +
                "\"Treatments\":null,\n" +
                "\"ScoutStops\":null},\n" +
                "\n" +
                "{\"BlockID\":4,\n" +
                "\"BlockName\":\"Block D\",\n" +
                "\"Treatments\":null,\n" +
                "\"ScoutStops\":null},\n" +
                "\n" +
                "{\"BlockID\":5,\n" +
                "\"BlockName\":\"Block E\",\n" +
                "\"Treatments\":null,\n" +
                "\"ScoutStops\":null},\n" +
                "\n" +
                "{\"BlockID\":6,\n" +
                "\"BlockName\":\"Block F\",\n" +
                "\"Treatments\":null,\n" +
                "\"ScoutStops\":null}\n" +
                "\n" +
                "]}";
        Farm farm = gson.fromJson(farmJson,Farm.class);

        String roleJson = "{\"RoleId\":2,\n" +
                "\"Type\":2,\n" +
                "\"Description\":\"Admin\"}";

        Role role = gson.fromJson(roleJson,Role.class);



        assertEquals(user.Farms.length,1);

    }
}
