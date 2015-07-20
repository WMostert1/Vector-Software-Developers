package vsd.co.za.sambugapp.DataAccess;

import android.app.DownloadManager;
import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.AsyncTask;
import android.util.Log;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.Volley;

import org.json.JSONObject;

import java.io.IOError;
import java.io.IOException;
import java.io.InputStream;
import java.net.HttpURLConnection;
import java.net.URL;

/**
 * Created by Aeolus on 2015-07-20.
 */
public class SynchronizeTask {
    private static final String SYNC_SERVICE_URL = "http://localhost/sync/pushScoutingData";
    private Context context = null;
    private RequestQueue queue;

    public SynchronizeTask(Context context) {
        this.context = context;
        this.queue = Volley.newRequestQueue(this.context);

        JsonObjectRequest jobr = new JsonObjectRequest(Request.Method.POST, SYNC_SERVICE_URL, new JSONObject(), new Response.Listener<JSONObject>() {
            @Override
            public void onResponse(JSONObject response) {
                Log.e("reponse", response.toString());
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                
            }
        });
    }





}
