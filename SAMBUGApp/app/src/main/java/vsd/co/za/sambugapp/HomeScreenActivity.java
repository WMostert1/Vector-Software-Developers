package vsd.co.za.sambugapp;

import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;

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

}
