package vsd.co.za.sambugapp.DataAccess;

import android.app.DownloadManager;
import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.AsyncTask;
import android.util.Log;
import android.widget.Toast;

import com.android.volley.DefaultRetryPolicy;
import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.Volley;
import com.google.gson.Gson;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOError;
import java.io.IOException;
import java.io.InputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

import vsd.co.za.sambugapp.DataAccess.DTO.CacheSyncDTO;
import vsd.co.za.sambugapp.DomainModels.ScoutBug;
import vsd.co.za.sambugapp.DomainModels.ScoutStop;

/**
 * Created by Aeolus on 2015-07-20.
 *
 */
public class SynchronizeTask extends AsyncTask<Void,Void,Void>{
    private static final String SYNC_SERVICE_URL = "http://localhost:53358/api/authentication/login";
    private static final int SOCKET_TIMEOUT_MS = 10000; //10 seconds
    private static SynchronizeTask mInstance;
    private static Context context;

    private SynchronizeTask(Context context) {
        SynchronizeTask.context = context;
    }

    public static synchronized SynchronizeTask getInstance(Context context) {
        if (mInstance == null) {
            mInstance = new SynchronizeTask(context);
        }
        return mInstance;
    }

    @Override
    protected Void doInBackground(Void... params) {
                pushCachedData();
        return null;
    }

    private CacheSyncDTO getCachedDTO() {
        ScoutBugDAO scoutBugDAO = new ScoutBugDAO(context);
        ScoutStopDAO scoutStopDAO = new ScoutStopDAO(context);
        try {
            scoutBugDAO.open();
            scoutStopDAO.open();

            return new CacheSyncDTO(scoutStopDAO.getAllScoutStops(), scoutBugDAO.getAllScoutBugs());


        }catch(SQLException e){
            e.printStackTrace();
            return new CacheSyncDTO();
        }
    }

    public void pushCachedData(){
        final CacheSyncDTO cacheDTO = getCachedDTO();

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
                try {
                    if (response.get("ResponseCode").equals(true)) {
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
                } catch (JSONException e) {
                    Log.e("JSONError", e.toString());
                }
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Log.e("NetworkingError:", error.toString());
            }
        });

        jsObjRequest.setRetryPolicy(new DefaultRetryPolicy(
                SOCKET_TIMEOUT_MS,
                DefaultRetryPolicy.DEFAULT_MAX_RETRIES,
                DefaultRetryPolicy.DEFAULT_BACKOFF_MULT));

        VolleySingleton.getInstance(context).addToRequestQueue(jsObjRequest);
    }
}
