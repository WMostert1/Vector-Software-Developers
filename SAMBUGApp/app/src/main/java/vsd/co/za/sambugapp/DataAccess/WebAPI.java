package vsd.co.za.sambugapp.DataAccess;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.widget.Toast;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;

import org.json.JSONException;
import org.json.JSONObject;

import java.sql.SQLException;
import java.util.ArrayList;
import java.util.HashSet;

import vsd.co.za.sambugapp.DataAccess.DTO.CacheSyncDTO;
import vsd.co.za.sambugapp.DomainModels.Farm;
import vsd.co.za.sambugapp.DomainModels.ScoutBug;
import vsd.co.za.sambugapp.DomainModels.ScoutStop;
import vsd.co.za.sambugapp.DomainModels.User;
import vsd.co.za.sambugapp.LoginActivity;
import vsd.co.za.sambugapp.R;
import vsd.co.za.sambugapp.ScoutTripActivity;

/**
 * Created by Aeolus on 2015-07-20.
 *
 */
public class WebAPI {
    private static final String AUTHENTICATION_URL = "http://sambug.apphb.com/api/authentication/login";
    private static final String SYNC_SERVICE_URL = "http://sambug.apphb.com/api/Synchronization/persistcacheddata";
    private static final int SOCKET_TIMEOUT_MS = 10000; //10 seconds


    private WebAPI() {
    }

    public static void attemptSyncCachedScoutingData(Context context) {
        new CachedPersistenceTask(context).execute();
    }

    public static void attemptLogin(String username, String password, Context context) {
        (new AuthLoginTask(context)).execute(username, password);
    }

    private static class CachedPersistenceTask extends AsyncTask<Void, Void, Void> {
        private Context context;

        public CachedPersistenceTask(Context _context) {
            context = _context;
        }

        @Override
        protected Void doInBackground(Void... params) {
            pushCachedData();
            return null;
        }

        private void pushCachedData() {
            final CacheSyncDTO cacheDTO = getCachedScoutingDTO();

            JSONObject jsonDTO;

            try {
                jsonDTO = new JSONObject(new Gson().toJson(cacheDTO));

            } catch (JSONException e) {
                e.printStackTrace();
                jsonDTO = new JSONObject();
            }

            JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.POST, SYNC_SERVICE_URL, jsonDTO, new Response.Listener<JSONObject>() {
                @Override
                public void onResponse(JSONObject response) {

                    Toast.makeText(context, "Server contacted.", Toast.LENGTH_SHORT).show();

                    Toast.makeText(context, "Scout data successfully pushed to server", Toast.LENGTH_SHORT).show();
                            ScoutBugDAO scoutBugDAO = new ScoutBugDAO(context);
                            ScoutStopDAO scoutStopDAO = new ScoutStopDAO(context);
                            try {
                                scoutBugDAO.open();
                                scoutStopDAO.open();

                                for (ScoutBug bug : cacheDTO.scoutBugs)
                                    scoutBugDAO.delete(bug);

                                for (ScoutStop stop : cacheDTO.scoutStops)
                                    scoutStopDAO.delete(stop);

                                scoutBugDAO.close();
                                scoutStopDAO.close();
                            } catch (SQLException e) {
                                Log.e("Deletion", e.toString());
                            }

                }
            }, new Response.ErrorListener() {
                @Override
                public void onErrorResponse(VolleyError error) {
                    Toast.makeText(context, "Error connecting to server.", Toast.LENGTH_SHORT).show();
                    Log.e("NetworkingError:", error.toString());
                }
            });

            jsObjRequest.setRetryPolicy(new DefaultRetryPolicy(
                    SOCKET_TIMEOUT_MS,
                    DefaultRetryPolicy.DEFAULT_MAX_RETRIES,
                    DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));

            VolleySingleton.getInstance(context).addToRequestQueue(jsObjRequest);
        }


        private CacheSyncDTO getCachedScoutingDTO() {
            ScoutBugDAO scoutBugDAO = new ScoutBugDAO(context);
            ScoutStopDAO scoutStopDAO = new ScoutStopDAO(context);
            try {
                scoutBugDAO.open();
                scoutStopDAO.open();

                return new CacheSyncDTO(scoutStopDAO.getAllScoutStops(), scoutBugDAO.getAllScoutBugs());


            } catch (SQLException e) {
                e.printStackTrace();
                return new CacheSyncDTO();
            }
        }
    }


    private static class AuthLoginTask extends AsyncTask<String,Void,User>{

        private Context context;

        public AuthLoginTask(Context _context){
            context = _context;
        }

        @Override
        protected User doInBackground(String... params) {
            JSONObject loginRequest = new JSONObject();
            try {
                loginRequest.put("username", params[0]);
                loginRequest.put("password",params[1]);
            }catch (JSONException e){
                Toast.makeText(context, "A parsing error occurred.", Toast.LENGTH_SHORT).show();
                return null;
            }

            JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.POST,AUTHENTICATION_URL,loginRequest,new Response.Listener<JSONObject>(){
                @Override
                public void onResponse(JSONObject response) {
                    final Gson gson = new Gson();
                    UserWrapper userWrapper = gson.fromJson(response.toString(), UserWrapper.class);
                    User user = userWrapper.User;

                    SharedPreferences sharedPref = context.getSharedPreferences(
                            context.getString(R.string.preference_file_key), Context.MODE_PRIVATE);

                    SharedPreferences.Editor editor = sharedPref.edit();
                    editor.putString(context.getString(R.string.logged_in_user), response.toString());
                    editor.commit();

                    Intent intent = new Intent(context,ScoutTripActivity.class);
                    intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                    Bundle bundle=new Bundle();
                    HashSet<Farm> activeFarms = user.getFarms();
                    bundle.putSerializable(LoginActivity.USER_FARMS, activeFarms);
                    intent.putExtras(bundle);
                    context.startActivity(intent);
                }
            },new Response.ErrorListener(){
                @Override
                public void onErrorResponse(VolleyError error) {

                    Intent intent = new Intent(context,LoginActivity.class);
                    intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                    context.startActivity(intent);
                    Toast.makeText(context,"Login unsuccessful. Please try again.",Toast.LENGTH_LONG).show();
                }
            });

            jsObjRequest.setRetryPolicy(new DefaultRetryPolicy(
                    SOCKET_TIMEOUT_MS,
                    DefaultRetryPolicy.DEFAULT_MAX_RETRIES,
                    DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));

            VolleySingleton.getInstance(context).addToRequestQueue(jsObjRequest);

            return null;
        }
    }
}
