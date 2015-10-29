using System;
using System.Collections.Generic;

namespace DataAccess.Interface
{
    public interface IDbFarmManagement
    {
        long InsertNewFarm(long id, string farmname);
        bool InsertNewBlock(long id, string blockname);
        ICollection<Models.Farm> GetFarmsByID(long id);
        long UpdateBlock(long id, string blockname);
        bool DeleteFarm(long id);
        bool DeleteBlock(long id);
/*        List<Object> GetTreatmentInfoByUserId(long userId);*/
        bool InsertNewTreatment(long id, DateTime date, string comments);
    }
}
