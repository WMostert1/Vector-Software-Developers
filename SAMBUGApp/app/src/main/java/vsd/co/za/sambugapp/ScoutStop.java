package vsd.co.za.sambugapp;

import java.util.ArrayList;

/**
 * Created by keaganthompson on 7/8/15.
 */
public class ScoutStop {

    private String mBlockName;
    private int mNumTrees;
    private ArrayList<ScoutBug> mBugs;

    public ScoutStop() {
        mBlockName="";
        mNumTrees=0;
        mBugs=new ArrayList<>();
    }

    public String getBlockName() {
        return mBlockName;
    }

    public void setBlockName(String blockName) {
        mBlockName = blockName;
    }

    public int getNumTrees() {
        return mNumTrees;
    }

    public void setNumTrees(int numTrees) {
        mNumTrees = numTrees;
    }

    public ArrayList<ScoutBug> getBugs() {
        return mBugs;
    }

    public void addBugEntry(ScoutBug bugEntry) {
        mBugs.add(bugEntry);
    }

    public double getPestsPerTree() {
        return 0.8;
    }
}
