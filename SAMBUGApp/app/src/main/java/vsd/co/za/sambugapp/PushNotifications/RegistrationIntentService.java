package vsd.co.za.sambugapp.PushNotifications;

import android.app.IntentService;
import android.content.Intent;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;
import android.util.Log;
import android.widget.Toast;

import com.google.android.gms.gcm.GcmPubSub;
import com.google.android.gms.gcm.GoogleCloudMessaging;
import com.google.android.gms.iid.InstanceID;

import java.io.IOException;

import vsd.co.za.sambugapp.DataAccess.WebAPI;
import vsd.co.za.sambugapp.R;

/**
 * Created by keaganthompson on 2016/04/20.
 */
public class RegistrationIntentService extends IntentService {

    public RegistrationIntentService(){
        super("RegService");
    }

    @Override
    protected void onHandleIntent(Intent intent) {
        try {
            SharedPreferences pref= PreferenceManager.getDefaultSharedPreferences(this);
            //Not registered
            String token=null;
            if (pref.getString("dev_token",null)==null) {
                InstanceID instanceID = InstanceID.getInstance(this);
                token = instanceID.getToken(getString(R.string.gcm_defaultSenderId), GoogleCloudMessaging.INSTANCE_ID_SCOPE, null);
                pref.edit().putString("dev_token",token).commit();
            } else {
                token=pref.getString("dev_token",null);
            }
            //send to server
            if (!pref.getBoolean("dev_server_reg",false)){
                Toast.makeText(getApplicationContext(),"Registering device with server",Toast.LENGTH_LONG);
                WebAPI.attemptDeviceRegistration(intent.getIntExtra("UserID",-1)+"",token,getApplicationContext());
            }
            subscribeTopics(token);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void subscribeTopics(String token) throws IOException {
        GcmPubSub pubSub = GcmPubSub.getInstance(this);
        pubSub.subscribe(token, "/topics/sambug", null);
    }
}
