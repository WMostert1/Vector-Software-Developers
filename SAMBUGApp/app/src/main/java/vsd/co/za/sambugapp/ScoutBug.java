package vsd.co.za.sambugapp;

import java.io.Serializable;

/**
 * Created by Kale-ab on 2015-07-09.
 */
public class ScoutBug implements Serializable{
    Species species;
    int numberOfBugs;

    public Species getSpecies() {
        return species;
    }

    public void setSpecies(Species species) {
        this.species = species;
    }

    public int getNumberOfBugs() {
        return numberOfBugs;
    }

    public void setNumberOfBugs(int numberOfBugs) {
        this.numberOfBugs = numberOfBugs;
    }
}
