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
        AddBlockResult AddBlock(AddBlockRequest addblockRequest);
        GetBlocksByFarmResult GetBlocksByFarm(GetBlocksByFarmRequest getblocksbyfarmRequest);
        GetBlockByIDResult GetBlockByID(GetBlockByIDRequest getblockybyidRequest);
        GetFarmByIDResult GetFarmByID(GetFarmByIDRequest getfarmbyidRequest);
        UpdateBlockByIDResult UpdateBlockByID(UpdateBlockByIDRequest updateblockbyidRequest);
        DeleteBlockByIDResult DeleteBlockByID(DeleteBlockByIDRequest deleteblockbyidRequest);
        GetPestsPerTreeByBlockResult GetPestsPerTreeByBlock(GetPestsPerTreeByBlockRequest getpestspertreebyblockRequest);
        AddTreatmentResult AddTreatment(AddTreatmentRequest addtreatmentRequest);
    }
}
