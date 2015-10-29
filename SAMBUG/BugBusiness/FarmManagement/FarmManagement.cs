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
        public GetTreatmentInfoResponse GetTreatmentInfo(long id)
        {
            if (id <= 0)
            {
                throw new InvalidInputException();
            }

            ICollection<Farm> farms= _dbFarmManagement.GetFarmsByID(id);


            if (farms == null)
            {
                throw new NoSuchFarmExistsException();;
            }

            List<FarmForTreatmentDto> farmsArray = new List<FarmForTreatmentDto>();
            FarmForTreatmentDto farm;
            BlockTreatmentDto block;
            ScoutStop stop;
            DateTime latestScoutStop;
            int numOfBugs;
            DateTime currentDate = DateTime.Now;
            DateTime lastDate;
            DateTime nextDate;
            

            foreach (var frms in farms)
            {
                farm = new FarmForTreatmentDto();
                farm.Blocks = new List<BlockTreatmentDto>();
                farm.FarmName = frms.FarmName;
                farm.FarmID = frms.FarmID;

                foreach (var blck in frms.Blocks)
                {
                    block = new BlockTreatmentDto();
                    block.BlockID = blck.BlockID;
                    block.BlockName = blck.BlockName;

                    if (blck.ScoutStops.Any())
                    {
                        latestScoutStop = blck.ScoutStops.Max(s => s.Date);
                        stop = blck.ScoutStops.SingleOrDefault(s => s.Date.Equals(latestScoutStop));

                        numOfBugs = stop.ScoutBugs.Where(bugs => bugs.Species.IsPest).Sum(bugs => bugs.NumberOfBugs);

                        block.PestsPerTree = (double) numOfBugs/stop.NumberOfTrees;
                    }
                    else
                    {
                        block.PestsPerTree = -1;
                    }

                    if (blck.Treatments.Any())
                    {
                        lastDate = DateTime.MinValue;
                        nextDate = DateTime.MaxValue;

                        foreach (var trtment in blck.Treatments)
                        {
                            if (DateTime.Compare(trtment.Date, lastDate) > 0  && DateTime.Compare(trtment.Date, currentDate) < 0)
                            {
                                lastDate = trtment.Date;
                            }
                            if (DateTime.Compare(trtment.Date, nextDate) < 0 && DateTime.Compare(trtment.Date, currentDate) > 0)
                            {
                                nextDate = trtment.Date;
                            }
                        }

                        if (lastDate.Equals(DateTime.MinValue))
                        {
                            block.LastTreatment = "-";
                        }
                        else
                        {
                            block.LastTreatment = lastDate.ToString("yyyy-MM-dd");
                        }

                        if (nextDate.Equals(DateTime.MaxValue))
                        {
                            block.NextTreatment = "-";
                        }
                        else
                        {
                            block.NextTreatment = nextDate.ToString("yyyy-MM-dd");
                        }
                        
                    }
                    else
                    {
                        block.NextTreatment = "-";
                        block.LastTreatment = "-";
                    }

                    farm.Blocks.Add(block);
                }

                farmsArray.Add(farm);
            }

            //PestsPerTree = (double)queryResult[0],LastTreatment=queryResult[1].ToString()
            return new GetTreatmentInfoResponse(){Farms = farmsArray};
        }

        public AddTreatmentResponse AddTreatment(AddTreatmentRequest addtreatmentRequest)
        {
            if (addtreatmentRequest.BlockID <= 0)
            {
                throw new InvalidInputException();
            }

            _dbFarmManagement.InsertNewTreatment(addtreatmentRequest.BlockID, Convert.ToDateTime(addtreatmentRequest.TreatmentDate), addtreatmentRequest.TreatmentComments);

            return new AddTreatmentResponse();
        }
    }
}
