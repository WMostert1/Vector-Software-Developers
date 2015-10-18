package vsd.co.za.sambugapp;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.net.Uri;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.Toast;

import java.util.HashSet;
import java.util.List;

import vsd.co.za.sambugapp.DomainModels.Farm;
import vsd.co.za.sambugapp.DomainModels.ScoutTrip;

public class HomeScreenActivity extends AppCompatActivity {

    HashSet<Farm> farms;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_home_screen);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle(null);

        acceptFarms();

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
        Uri webpage = Uri.parse("http://sambug.apphb.com");
        Intent i = new Intent(Intent.ACTION_VIEW, webpage);
        startActivity(i);
    }

    public void logout(View v) {
        getApplicationContext().getSharedPreferences(getApplicationContext().getString(R.string.preference_file_key), Context.MODE_PRIVATE).edit().clear().commit();
        Toast.makeText(getApplicationContext(), "Logged out successfully", Toast.LENGTH_LONG).show();
        Intent i = new Intent(HomeScreenActivity.this, LoginActivity.class);
        startActivity(i);
    }

}
