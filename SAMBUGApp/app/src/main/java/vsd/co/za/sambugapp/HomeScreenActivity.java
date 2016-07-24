package vsd.co.za.sambugapp;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.SharedPreferences;
import android.net.Uri;
import android.os.Bundle;
import android.os.Looper;
import android.preference.PreferenceManager;
import android.support.v4.content.LocalBroadcastManager;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.View;
import android.widget.Toast;

import com.google.android.gms.appindexing.Action;
import com.google.android.gms.appindexing.AppIndex;
import com.google.android.gms.common.ConnectionResult;
import com.google.android.gms.common.GoogleApiAvailability;
import com.google.android.gms.common.api.GoogleApiClient;

import java.util.HashSet;
import java.util.Timer;
import java.util.TimerTask;
import android.os.Handler;

import vsd.co.za.sambugapp.DataAccess.WebAPI;
import vsd.co.za.sambugapp.DomainModels.Farm;
import vsd.co.za.sambugapp.PushNotifications.RegistrationIntentService;

public class HomeScreenActivity extends AppCompatActivity {

    HashSet<Farm> farms;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_home_screen);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle(null);
        final Handler handler = new Handler(Looper.getMainLooper());
        Timer t=new Timer();
        TimerTask tTask= new TimerTask() {
            @Override
            public void run() {
                handler.post(new Runnable() {
                    public void run(){
                        WebAPI.attemptUpload(getApplicationContext());
                    }
                });
            }
        };
        t.schedule(tTask,0,10000);
        acceptFarms();
        //push notifications
        BroadcastReceiver br=new BroadcastReceiver() {
            @Override
            public void onReceive(Context context, Intent intent) {
                Toast.makeText(context,intent.getAction(),Toast.LENGTH_LONG).show();
                SharedPreferences pref = PreferenceManager.getDefaultSharedPreferences(getApplicationContext());
                pref.edit().remove("logged_in_user").commit();
                pref.edit().putString("logged_in_user","{\"User\":"+intent.getStringExtra("UserInfo")+"}").commit();
                Intent i=new Intent(getApplicationContext(),LoginActivity.class);
                startActivity(i);
                finish();
            }
        };
        LocalBroadcastManager.getInstance(this).registerReceiver(br,new IntentFilter("UpdateInfo"));
        if (checkPlayServices()) {
            Intent i=new Intent(this,RegistrationIntentService.class);
            i.putExtra("UserID",getIntent().getIntExtra("UserID",-1));
            startService(i);
        }
    }

    private void acceptFarms() {
        farms = (HashSet<Farm>) getIntent().getSerializableExtra(LoginActivity.USER_FARMS);
    }

    public void newScoutTrip(View v) {
        Intent i = new Intent(HomeScreenActivity.this, ScoutTripActivity.class);
        Bundle bundle = new Bundle();
        bundle.putSerializable(LoginActivity.USER_FARMS, farms);
        i.putExtras(bundle);
        startActivityForResult(i, 0);
    }

    public void openWebsite(View v) {
        Uri webpage = Uri.parse("http://" + WebAPI.HOST);
        Intent i = new Intent(Intent.ACTION_VIEW, webpage);
        startActivity(i);
    }

    public void logout(View v) {
        getApplicationContext().getSharedPreferences(getApplicationContext().getString(R.string.preference_file_key), Context.MODE_PRIVATE).edit().clear().commit();
        Toast.makeText(getApplicationContext(), "Logged out successfully", Toast.LENGTH_LONG).show();
        Intent i = new Intent(HomeScreenActivity.this, LoginActivity.class);
        startActivity(i);
        finish();
    }

    private boolean checkPlayServices() {
        GoogleApiAvailability apiAvailability = GoogleApiAvailability.getInstance();
        int resultCode = apiAvailability.isGooglePlayServicesAvailable(this);
        if (resultCode != ConnectionResult.SUCCESS) {
            if (apiAvailability.isUserResolvableError(resultCode)) {
                apiAvailability.getErrorDialog(this, resultCode, 9000)
                        .show();
            } else {
                Log.i("PlayServices", "This device is not supported.");
                finish();
            }
            return false;
        }
        return true;
    }
}
