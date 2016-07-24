package vsd.co.za.sambugapp.DataAccess;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.AsyncTask;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.util.Log;
import android.widget.Toast;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.google.gson.Gson;

import org.json.JSONException;
import org.json.JSONObject;

import java.sql.SQLException;
import java.util.HashSet;

import vsd.co.za.sambugapp.DataAccess.DTO.CacheSyncDTO;
import vsd.co.za.sambugapp.DataAccess.DTO.ClassificationRequestDTO;
import vsd.co.za.sambugapp.DataAccess.DTO.ClassificationResultDTO;
import vsd.co.za.sambugapp.DomainModels.Farm;
import vsd.co.za.sambugapp.DomainModels.ScoutBug;
import vsd.co.za.sambugapp.DomainModels.ScoutStop;
import vsd.co.za.sambugapp.DomainModels.User;
import vsd.co.za.sambugapp.HomeScreenActivity;
import vsd.co.za.sambugapp.IdentificationActivity;
import vsd.co.za.sambugapp.LoginActivity;
import vsd.co.za.sambugapp.R;

/**
 * Created by Aeolus on 2015-07-20.
 *
 */
public class WebAPI {
    public static final String HOST = "192.168.0.11:53249";
    private static final String AUTHENTICATION_URL = "http://"+HOST+"/api/authentication/login";
    private static final String REGISTER_URL = "http://"+HOST+"/api/authentication/registerdevice";
    private static final String SYNC_SERVICE_URL = "http://"+HOST+"/api/synchronization";
    private static final String CLASSIFICATION_URL= "http://"+"sambug.azurewebsites.net"+"/api/apiSpeciesClassification";
    private static final int SOCKET_TIMEOUT_MS = 100000; //10 seconds
    private static final int MAX_RETRIES = 3;

    private WebAPI() {
    }

    public static AsyncTask attemptAPIClassification(byte[] pictureData, Context context) {
        ClassificationRequestDTO request = new ClassificationRequestDTO();
        request.FieldPicture = pictureData;
        return new ClassificationTask(context).execute(request);
    }

    public static void attemptSyncCachedScoutingData(Context context) {
        new CachedPersistenceTask(context).execute();
    }

    public static void attemptLogin(String username, String password, Context context) {
        (new AuthLoginTask(context)).execute(username, password);
    }

    public static void attemptDeviceRegistration(String id, String token, Context context) {
        (new RegisterDeviceTask(context)).execute(id, token);
    }

    public static void attemptUpload(Context context){
        (new UploadTask(context)).execute();
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
                    /*MaterialDialog dialog = new MaterialDialog.Builder(context)
                            .title("Number of Bugs")
                            .content(error.getMessage())
                            .positiveText("Finish")
                            .titleGravity(GravityEnum.CENTER)
                            .show();*/
                    Toast.makeText(context, "Error connecting to server. After next scout trip, data will synchronise.", Toast.LENGTH_LONG).show();
                    Log.e("NetworkingError:", error.toString());
                    error.printStackTrace();
                }
            });

            jsObjRequest.setRetryPolicy(new DefaultRetryPolicy(
                    SOCKET_TIMEOUT_MS,
                    MAX_RETRIES,
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

    private static class ClassificationTask extends AsyncTask<ClassificationRequestDTO,Void,Void>{

        private Context context;

        public ClassificationTask(Context _context){
            context = _context;
        }


        @Override
        protected Void doInBackground(ClassificationRequestDTO... classificationRequestDTOs) {
            JSONObject classificationRequest;
            try{
                classificationRequest = new JSONObject(new Gson().toJson(classificationRequestDTOs[0]));

            }catch (JSONException e){
                if (!isCancelled())
                    Toast.makeText(context, "A JSON conversion error occurred.", Toast.LENGTH_SHORT).show();
                return null;
            }

            String jsonString = classificationRequest.toString();
            //System.out.println(jsonString);
            JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.POST,CLASSIFICATION_URL,classificationRequest,new Response.Listener<JSONObject>(){
                @Override
                public void onResponse(JSONObject response) {
                    if (!isCancelled()) {
                        final Gson gson = new Gson();
                        ClassificationResultDTO result = gson.fromJson(response.toString(), ClassificationResultDTO.class);
                        ((IdentificationActivity) context).changeEntrySelection(result);
                    }
                }
            },new Response.ErrorListener(){
                @Override
                public void onErrorResponse(VolleyError error) {
                    if (!isCancelled()) {
                        Toast.makeText(context, "Could not classify bug. Please choose manually.", Toast.LENGTH_LONG).show();
                    }
                }
            });

            jsObjRequest.setRetryPolicy(new DefaultRetryPolicy(
                    SOCKET_TIMEOUT_MS,
                    MAX_RETRIES,
                    DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));

            VolleySingleton.getInstance(context).addToRequestQueue(jsObjRequest);

            return null;
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
                Toast.makeText(context, "A JSON conversion error occurred.", Toast.LENGTH_SHORT).show();
                return null;
            }

            JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.POST,AUTHENTICATION_URL,loginRequest,new Response.Listener<JSONObject>(){
                @Override
                public void onResponse(JSONObject response) {
                    final Gson gson = new Gson();
                    UserWrapper userWrapper = gson.fromJson(response.toString(), UserWrapper.class);
                    User user = userWrapper.User;

                    SharedPreferences sharedPref = PreferenceManager.getDefaultSharedPreferences(context);

                    SharedPreferences.Editor editor = sharedPref.edit();
                    editor.putString(context.getString(R.string.logged_in_user), response.toString());
                    editor.commit();

                    Intent intent = new Intent(context, HomeScreenActivity.class);
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
                    error.printStackTrace();
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

    private static class RegisterDeviceTask extends AsyncTask<String,Void,User>{
        private Context context;

        public RegisterDeviceTask(Context _context){
            context = _context;
        }

        @Override
        protected User doInBackground(String... params) {
            JSONObject devRegRequest = new JSONObject();
            try {
                devRegRequest.put("UserID", Integer.valueOf(params[0]));
                devRegRequest.put("DeviceToken",params[1]);
            }catch (JSONException e){
                Toast.makeText(context, "A JSON conversion error occurred.", Toast.LENGTH_SHORT).show();
                return null;
            }

            JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.POST,REGISTER_URL,devRegRequest,new Response.Listener<JSONObject>(){
                @Override
                public void onResponse(JSONObject response) {
                    boolean registered=false;
                    try {
                        registered = response.getBoolean("Registered");
                    }catch (org.json.JSONException ex){
                        ex.printStackTrace();
                        registered=false;
                    }
                    //SharedPreferences sharedPref = context.getSharedPreferences(
                      //      context.getString(R.string.preference_file_key), Context.MODE_PRIVATE);
                    SharedPreferences pref = PreferenceManager.getDefaultSharedPreferences(context);
                    pref.edit().putBoolean("dev_server_reg",registered).commit();
                    if (registered){
                        Toast.makeText(context,"Device successfully registered with server",Toast.LENGTH_LONG);
                    } else {
                        Toast.makeText(context,"Device could not be registered with server",Toast.LENGTH_LONG);
                    }
                }
            },new Response.ErrorListener(){
                @Override
                public void onErrorResponse(VolleyError error) {
                    error.printStackTrace();
                    Toast.makeText(context,"An error occurred while registering device on server",Toast.LENGTH_LONG).show();
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

    private static class UploadTask extends AsyncTask<Void, Void, Void> {

        private Context context;

        public UploadTask(Context _context){
            context = _context;
        }

        @Override
        protected Void doInBackground(Void... params) {
            ConnectivityManager cm =
                    (ConnectivityManager)context.getSystemService(Context.CONNECTIVITY_SERVICE);

            NetworkInfo activeNetwork = cm.getActiveNetworkInfo();
            boolean isConnected = activeNetwork != null &&
                    activeNetwork.isConnectedOrConnecting();
            return null;
        }
    }
}
