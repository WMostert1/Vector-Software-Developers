using System.Collections.Generic;
using System.Linq;
using System;
using DataAccess.Interface;
using DataAccess.Models;

namespace DataAccess.MSSQL
{
    public class DbFarmManagement : IDbFarmManagement
    {
        public long InsertNewFarm(long id, string farmname)
        {
            var db = new BugDBEntities();

            var check = db.Farms.SingleOrDefault(frm => frm.UserID.Equals(id) && frm.FarmName.Equals(farmname));
            if (check != default(Farm))
            {
                return -1;
            }

            var farm = new Farm()
            {
                UserID = id,
                FarmName = farmname
            };

            db.Farms.Add(farm);
            db.SaveChanges();
            db.Entry(farm).GetDatabaseValues();
            return farm.FarmID;
        }

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

        public ICollection<Farm> GetFarmsByID(long id)
        {
            var db = new BugDBEntities();

            var farms = db.Farms.Where(frm => frm.UserID.Equals(id)).ToList();

            return farms;
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

        public bool DeleteFarm(long id)
        {
            var db = new BugDBEntities();

            var farm = db.Farms.SingleOrDefault(frm => frm.FarmID.Equals(id));

            if (farm != null)
            {
                db.Farms.Remove(farm);
                db.SaveChanges();
                return true;
            }

            return false;
        }

        public bool DeleteBlock(long id)
        {
            var db = new BugDBEntities();

            var block = db.Blocks.SingleOrDefault(blk=>blk.BlockID.Equals(id));

            if (block!=null){
                db.Blocks.Remove(block);
                db.SaveChanges();
                return true;
            }

            return false;
        }

/*        public List<Object> GetTreatmentInfoByUserId(long userId)
        {
            var db=new BugDBEntities();

            var farms = db.Farms.Where(frm => frm.UserID.Equals(userId)).ToList();

            if (farms == null)
            {
                return null;
            }

            foreach(var frm in farms)
            //calculate average, return -1 if no scoutstops
            double average;
            if (!block.ScoutStops.Count.Equals(0))
            {
                double bugs, trees;
                bugs = 0;
                trees = 0;
                foreach (ScoutStop stop in block.ScoutStops)
                {
                    bugs += stop.ScoutBugs.Sum(bug => bug.NumberOfBugs);
                    trees += stop.NumberOfTrees;
                }
                average = Math.Round(bugs / trees, 2);
            }
            else
            {
                average = -1;
            }

            //TODO: maybe include months if weeks>4?
            string difference;
            if (!block.Treatments.Count.Equals(0))
            {
                DateTime lastTreatment = block.Treatments.Max(trt => trt.Date);
                DateTime today = DateTime.Today;
                difference = ((int)today.Subtract(lastTreatment).TotalDays / 7) + " weeks ago";
            }
            else
            {
                difference = "N/A";
            }

            return new List<Object>{average,difference};
        }*/

        public bool InsertNewTreatment(long id, DateTime date, string comments)
        {
            var db = new BugDBEntities();

            var treatment = new Treatment()
            {
                BlockID = id,
                Date = date,
                Comments = comments
            };

            db.Treatments.Add(treatment);
            db.SaveChanges();
            return true;
        }
    }
}
