using DataAccess.Models;
using System;
using System.Collections.Generic;

namespace DataAccess.Interface
{
    public interface IDbScouting
    {
        bool PersistScoutStops(ICollection<ScoutStop> stops);
        

    }
}
