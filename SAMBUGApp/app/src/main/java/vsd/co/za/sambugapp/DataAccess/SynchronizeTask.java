package vsd.co.za.sambugapp.DataAccess;

import android.app.DownloadManager;
import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.AsyncTask;
import android.util.Log;
import android.widget.Toast;

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
import java.util.List;

import vsd.co.za.sambugapp.DomainModels.ScoutBug;
import vsd.co.za.sambugapp.DomainModels.ScoutStop;

/**
 * Created by Aeolus on 2015-07-20.
 */
public class SynchronizeTask extends AsyncTask<Void,Void,Void>{
    private static final String SYNC_SERVICE_URL = "http://localhost:53358/api/test/get";
    private Context context = null;

    public SynchronizeTask(Context context) {
        this.context = context;
    }

    @Override
    protected Void doInBackground(Void... params) {
        while(!isCancelled()){
            try {
                pushCachedData();
                Thread.sleep(10000);
            }catch (Exception e){
                break;
            }
        }

        return null;
    }

    private JSONObject getCachedDTO(){
        ScoutBugDAO scoutBugDAO = new ScoutBugDAO(context);
        ScoutStopDAO scoutStopDAO = new ScoutStopDAO(context);
        SpeciesDAO speciesDAO = new SpeciesDAO(context);

        try {
            scoutBugDAO.open();
            scoutStopDAO.open();
            speciesDAO.open();

            List<ScoutStop> dto = scoutStopDAO.getAllScoutStops();

            JSONObject result;

            try {
                result = new JSONObject(new Gson().toJson(dto));
            }catch (JSONException e){
                e.printStackTrace();
                result = new JSONObject();
            }

            return result;

        }catch(SQLException e){
            e.printStackTrace();
            return new JSONObject();
        }
    }

    public void pushCachedData(){
        JSONObject cacheDTO = getCachedDTO();

        JsonObjectRequest jsObjRequest = new JsonObjectRequest(Request.Method.GET, SYNC_SERVICE_URL, cacheDTO, new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject response) {
                Toast.makeText(context, response.toString(), Toast.LENGTH_SHORT).show();
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Toast.makeText(context, error.toString(), Toast.LENGTH_SHORT).show();
            }
        });

// Access the RequestQueue through your singleton class.
        VolleySingleton.getInstance(context).addToRequestQueue(jsObjRequest);
    }





}
