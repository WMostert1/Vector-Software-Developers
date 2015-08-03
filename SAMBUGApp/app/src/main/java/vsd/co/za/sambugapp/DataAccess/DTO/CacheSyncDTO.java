package vsd.co.za.sambugapp.DataAccess.DTO;

import java.util.List;

import vsd.co.za.sambugapp.DomainModels.ScoutBug;
import vsd.co.za.sambugapp.DomainModels.ScoutStop;

/**
 * Created by Aeolus on 2015-08-03.
 */
public class CacheSyncDTO {
    public List<ScoutStop> scoutStops;
    public List<ScoutBug> scoutBugs;

    public CacheSyncDTO(List<ScoutStop> scoutStops, List<ScoutBug> scoutBugs) {
        this.scoutStops = scoutStops;
        this.scoutBugs = scoutBugs;
    }

    public CacheSyncDTO() {
    }
}
