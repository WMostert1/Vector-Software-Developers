using System.Collections.Generic;

namespace DataAccess.Interface
{
    public interface IDbFarmManagement
    {
        bool InsertNewBlock(long id, string blockname);
        ICollection<Domain.Block> GetBlocksByFarm(long id);
        Domain.Block GetBlockByID(long id);
        Domain.Farm GetFarmByID(long id);
        long UpdateBlock(long id, string blockname);
        bool DeleteBlock(long id);
    }
}
