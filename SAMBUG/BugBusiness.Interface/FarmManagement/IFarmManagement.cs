using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugBusiness.Interface.FarmManagement.DTO;

namespace BugBusiness.Interface.FarmManagement
{
    public interface IFarmManagement
    {
        AddFarmResponse AddFarm(AddFarmRequest addfarmRequest);
        AddBlockResponse AddBlock(AddBlockRequest addblockRequest);
        GetFarmsByUserIDResponse GetFarmsByUserID(GetFarmsByUserIDRequest getfarmbyidRequest);
        UpdateBlockByIDResponse UpdateBlockByID(UpdateBlockByIDRequest updateblockbyidRequest);
        DeleteFarmByIDResponse DeleteFarmByID(long id);
        DeleteBlockByIDResponse DeleteBlockByID(long id);
        BlockSprayDto GetPestsPerTreeByBlock(GetTreatmentInfoRequest getpestspertreebyblockRequest);
        AddTreatmentResponse AddTreatment(AddTreatmentRequest addtreatmentRequest);
    }
}
