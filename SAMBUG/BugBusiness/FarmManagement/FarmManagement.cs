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

        public AddBlockResult AddBlock(AddBlockRequest addblockRequest)
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

            return new AddBlockResult();
        }

        public GetBlocksByFarmResult GetBlocksByFarm(GetBlocksByFarmRequest getblocksbyfarmRequest)
        {
            if (getblocksbyfarmRequest.FarmID <= 0)
            {
                throw new InvalidInputException();
            }

            ICollection<Block> blocks = _dbFarmManagement.GetBlocksByFarm(getblocksbyfarmRequest.FarmID);

            if (blocks == null)
            {
                throw new NoBlocksException();
            }

            List<BlockDTO> blockDTOList = AutoMapper.Mapper.Map<List<BlockDTO>>(blocks);

            GetBlocksByFarmResult result = new GetBlocksByFarmResult()
            {
                Blocks = blockDTOList
            };

            return result;
        }

        public GetBlockByIDResult GetBlockByID(GetBlockByIDRequest getblockybyidRequest)
        {
            if (getblockybyidRequest.BlockID <= 0)
            {
                throw new InvalidInputException();
            }

            Block block = _dbFarmManagement.GetBlockByID(getblockybyidRequest.BlockID);

            if (block == null)
            {
                throw new NoSuchBlockExistsException();
            }

            BlockDTO blockDTO = AutoMapper.Mapper.Map<BlockDTO>(block);

            GetBlockByIDResult getblockbyidResult = new GetBlockByIDResult()
            {
                Block=blockDTO
            };

            return getblockbyidResult;
        }

        public GetFarmByIDResult GetFarmByID(GetFarmByIDRequest getfarmbyidRequest)
        {
            if (getfarmbyidRequest.FarmID <= 0)
            {
                throw new InvalidInputException();
            }

            Farm farm = _dbFarmManagement.GetFarmByID(getfarmbyidRequest.FarmID);

            if (farm == null)
            {
                throw new NoSuchFarmExistsException();
            }

            FarmDTO farmDTO = AutoMapper.Mapper.Map<FarmDTO>(farm);

            GetFarmByIDResult getfarmbyidResult = new GetFarmByIDResult()
            {
                Farm = farmDTO
            };

            return getfarmbyidResult;
        }

        public UpdateBlockByIDResult UpdateBlockByID(UpdateBlockByIDRequest updateblockbyidRequest)
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

            return new UpdateBlockByIDResult() { FarmID = queryResult };

        }

        public DeleteBlockByIDResult DeleteBlockByID(DeleteBlockByIDRequest deleteblockbyidRequest)
        {
            if (deleteblockbyidRequest.BlockID <= 0)
            {
                throw new InvalidInputException();
            }

            bool queryResult = _dbFarmManagement.DeleteBlock(deleteblockbyidRequest.BlockID);

            if (!queryResult)
            {
                throw new CouldNotDeleteBlockException();
            }

            return new DeleteBlockByIDResult();
        }

        public GetPestsPerTreeByBlockResult GetPestsPerTreeByBlock(GetPestsPerTreeByBlockRequest getpestspertreebyblockRequest)
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

            return new GetPestsPerTreeByBlockResult() { PestsPerTree = (double)queryResult[0],LastTreatment=queryResult[1].ToString() };
        }

        public AddTreatmentResult AddTreatment(AddTreatmentRequest addtreatmentRequest)
        {
            if (addtreatmentRequest.BlockID <= 0)
            {
                throw new InvalidInputException();
            }

            _dbFarmManagement.InsertNewTreatment(addtreatmentRequest.BlockID, addtreatmentRequest.TreatmentDate, addtreatmentRequest.TreatmentComments);

            return new AddTreatmentResult();
        }
    }
}
