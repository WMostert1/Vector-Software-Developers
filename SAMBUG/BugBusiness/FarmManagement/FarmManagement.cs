using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugBusiness.Interface.FarmManagement;
using BugBusiness.Interface.FarmManagement.DTO;
using BugBusiness.Interface.FarmManagement.Exceptions;
using DataAccess.Interface;
using DataAccess.Models;

namespace BugBusiness.FarmManagement
{
    public class FarmManagement : IFarmManagement
    {
        private readonly IDbFarmManagement _dbFarmManagement;

        public FarmManagement(IDbFarmManagement dbFarmManagement)
        {
            _dbFarmManagement = dbFarmManagement;
        }

        public AddFarmResponse AddFarm(AddFarmRequest addfarmRequest)
        {
            if (addfarmRequest.FarmName.Equals(""))
            {
                throw new InvalidInputException();
            }

            long queryResult = _dbFarmManagement.InsertNewFarm(addfarmRequest.UserID, addfarmRequest.FarmName);

            if (queryResult==-1)
            {
                throw new FarmExistsException();
            }

            return new AddFarmResponse();
        }

        public AddBlockResponse AddBlock(AddBlockRequest addblockRequest)
        {
            if (addblockRequest.BlockName.Equals(""))
            {
                throw new InvalidInputException();
            }

            bool queryResult = _dbFarmManagement.InsertNewBlock(addblockRequest.FarmID, addblockRequest.BlockName);

            if (queryResult == false)
            {
                throw new BlockExistsException();
            }

            return new AddBlockResponse();
        }

        public GetFarmsByUserIDResponse GetFarmsByUserID(GetFarmsByUserIDRequest getfarmbyidRequest)
        {
            if (getfarmbyidRequest.id < 0)
            {
                throw new InvalidInputException();
            }

            ICollection<Farm> farms = _dbFarmManagement.GetFarmsByID(getfarmbyidRequest.id);
            if (farms == null)
            {
                throw new NoSuchFarmExistsException();
            }

            List<FarmForFarmManDto> farmsDto = AutoMapper.Mapper.Map<List<FarmForFarmManDto>>(farms);

            GetFarmsByUserIDResponse getfarmbyidResult = new GetFarmsByUserIDResponse()
            {
                Farms = farmsDto
            };

            return getfarmbyidResult;
        }

        public UpdateBlockByIDResponse UpdateBlockByID(UpdateBlockByIDRequest updateblockbyidRequest)
        {
            if (updateblockbyidRequest.BlockID <= 0 || updateblockbyidRequest.BlockName.Equals(""))
            {
                throw new InvalidInputException();
            }

            long queryResult = _dbFarmManagement.UpdateBlock(updateblockbyidRequest.BlockID, updateblockbyidRequest.BlockName);

            if (queryResult==-1)
            {
                throw new CouldNotUpdateException();
            }

            return new UpdateBlockByIDResponse() { FarmID = queryResult };

        }

        public DeleteFarmByIDResponse DeleteFarmByID(long id)
        {
            if (id<=0)
            {
                throw new InvalidInputException();
            }

            bool queryResult = _dbFarmManagement.DeleteFarm(id);

            if (!queryResult)
            {
                throw new CouldNotDeleteFarmException();
            }

            return new DeleteFarmByIDResponse();
        }

        public DeleteBlockByIDResponse DeleteBlockByID(long id)
        {
            if (id <= 0)
            {
                throw new InvalidInputException();
            }

            bool queryResult = _dbFarmManagement.DeleteBlock(id);

            if (!queryResult)
            {
                throw new CouldNotDeleteBlockException();
            }

            return new DeleteBlockByIDResponse();
        }


        //-------------------------------------------------------------------Treatment code--------------------------------------------------------------------------------------------
        public BlockSprayDto GetPestsPerTreeByBlock(GetTreatmentInfoRequest getpestspertreebyblockRequest)
        {
            if (getpestspertreebyblockRequest.BlockID <= 0)
            {
                throw new InvalidInputException();
            }

            List<Object> queryResult = _dbFarmManagement.GetTreatmentInfoByBlock(getpestspertreebyblockRequest.BlockID);

            if (queryResult == null)
            {
                throw new NoSuchBlockExistsException();
            }

            return new BlockSprayDto() { PestsPerTree = (double)queryResult[0],LastTreatment=queryResult[1].ToString() };
        }

        public AddTreatmentResponse AddTreatment(AddTreatmentRequest addtreatmentRequest)
        {
            if (addtreatmentRequest.BlockID <= 0)
            {
                throw new InvalidInputException();
            }

            _dbFarmManagement.InsertNewTreatment(addtreatmentRequest.BlockID, addtreatmentRequest.TreatmentDate, addtreatmentRequest.TreatmentComments);

            return new AddTreatmentResponse();
        }
    }
}
