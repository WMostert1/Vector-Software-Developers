using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interface
{
    public interface IDbFarmManagement
    {
        bool InsertNewBlock(long id, string blockname);
        IEnumerable<Domain.Block> GetBlocksByFarm(long id);
        Domain.Block GetBlockByID(long id);
        Domain.Farm GetFarmByID(long id);
        bool UpdateBlock(long id, string blockname);
        bool DeleteBlock(long id);
    }
}
