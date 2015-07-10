package vsd.co.za.sambugapp;

import java.io.Serializable;
import java.util.ArrayList;

/**
 * Created by keaganthompson on 7/8/15.
 */
public class ScoutStop implements Serializable{

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

    public void duplicateStop(ScoutStop sp){
        setNumTrees(sp.getNumTrees());
        setBlockName(sp.getBlockName());

        for(ScoutBug sb:sp.getBugs()){
            addBugEntry(sb);
        }
    }
}
