package vsd.co.za.sambugapp.PushNotifications;

import android.content.Intent;
import android.os.Bundle;
import android.support.v4.content.LocalBroadcastManager;
import android.util.Log;

import com.google.android.gms.gcm.GcmListenerService;

/**
 * Created by keaganthompson on 2016/04/20.
 */
public class GCMListenerService extends GcmListenerService {

    public GCMListenerService(){
        Log.d("toodaloo","Listening");
    }

    @Override
    public void onMessageReceived(String from, Bundle data) {
        String message = data.getString("userInfo");
        Log.d("TOODALOO", "Message received");
        Intent intent=new Intent("UpdateInfo");
        intent.putExtra("UserInfo",message);
        LocalBroadcastManager.getInstance(this).sendBroadcast(intent);

    }

}
