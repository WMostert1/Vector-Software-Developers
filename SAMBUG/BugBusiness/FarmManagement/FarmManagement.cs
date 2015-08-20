using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugBusiness.Interface.FarmManagement;
using BugBusiness.Interface.FarmManagement.DTO;
using BugBusiness.Interface.FarmManagement.Exceptions;
using DataAccess.Interface;
using DataAccess.Interface.Domain;

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

            GetBlocksByFarmResult result = new GetBlocksByFarmResult();
            result.Blocks = blocks;

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

            GetBlockByIDResult getblockbyidResult = new GetBlockByIDResult();
            getblockbyidResult.Block = block;

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

            GetFarmByIDResult getfarmbyidResult = new GetFarmByIDResult()
            {
                Farm = farm
            };

            return getfarmbyidResult;
        }

        public UpdateBlockByIDResult UpdateBlockByID(UpdateBlockByIDRequest updateblockbyidRequest)
        {
            if (updateblockbyidRequest.BlockID <= 0 || updateblockbyidRequest.BlockName.Equals(""))
            {
                throw new InvalidInputException();
            }

            bool queryResult = _dbFarmManagement.UpdateBlock(updateblockbyidRequest.BlockID, updateblockbyidRequest.BlockName);

            if (!queryResult)
            {
                throw new CouldNotUpdateException();
            }

            return new UpdateBlockByIDResult();

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
    }
}
