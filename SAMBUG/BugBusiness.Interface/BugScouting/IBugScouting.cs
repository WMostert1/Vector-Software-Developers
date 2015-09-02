using BugBusiness.Interface.BugScouting.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugScouting
{
    public interface IBugScouting
    {
        void persistScoutingData(SyncRequest request);
    }
}
