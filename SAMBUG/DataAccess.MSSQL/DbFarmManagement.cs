using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;
using DataAccess.Interface.Domain;

namespace DataAccess.MSSQL
{
    public class DbFarmManagement : IDbFarmManagement
    {
        //TODO: Remove magic number => 1
        public bool InsertNewBlock(long id,string blockname)
        {
            var db = new BugDBEntities();

            //TODO:Check that block for specific user doesnt already exist
            var check = db.Blocks.SingleOrDefault(blk => blk.FarmID.Equals(id) && blk.BlockName.Equals(blockname));
            if (check != default(Block))
            {
                return false;
            }

            var block = new Block()
            {
                FarmID=id,
                BlockName = blockname
            };
            db.Blocks.Add(block);
            db.SaveChanges();
            return true;
        }

        public ICollection<Interface.Domain.Block> GetBlocksByFarm(long id){
            var db=new BugDBEntities();

            var farm=db.Farms.SingleOrDefault(frm => frm.FarmID.Equals(id));

            if (farm==null)
            {
                return null;
            }

            var blocks = farm.Blocks.Select(blk =>
                new Interface.Domain.Block() { BlockID = blk.BlockID, BlockName = blk.BlockName }).ToList();

            return blocks;
        }

        public Interface.Domain.Block GetBlockByID(long id)
        {
            var db = new BugDBEntities();

            var block = db.Blocks.SingleOrDefault(blk => blk.BlockID.Equals(id));

            if (block != null)
            {
                return new Interface.Domain.Block() { BlockID = block.BlockID, BlockName = block.BlockName };
            }

            return null;
        }

        public Interface.Domain.Farm GetFarmByID(long id)
        {
            var db=new BugDBEntities();

            var farm = db.Farms.SingleOrDefault(frm => frm.FarmID.Equals(id));

            if (farm != null)
            {
                return new Interface.Domain.Farm()
                {
                    FarmID=farm.FarmID,
                    FarmName=farm.FarmName,
                    Blocks=farm.Blocks.Select(blk =>
                        new Interface.Domain.Block() 
                        { BlockID = blk.BlockID, 
                          BlockName = blk.BlockName 
                        }).ToList()
                };
            }

            return null;
        }

        public long UpdateBlock(long id, string blockname)
        {
            var db = new BugDBEntities();

            var block = db.Blocks.SingleOrDefault(blk => blk.BlockID.Equals(id));

            if (block != null)
            {
                block.BlockName = blockname;
                db.SaveChanges();
                db.Entry(block).GetDatabaseValues();
                return block.FarmID;
            }

            return -1;
        }

        public bool DeleteBlock(long id)
        {
            var db = new BugDBEntities();

            var block=db.Blocks.SingleOrDefault(blk=>blk.BlockID.Equals(id));

            if (block!=null){
                db.Blocks.Remove(block);
                db.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
