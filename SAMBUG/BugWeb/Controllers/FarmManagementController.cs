using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugBusiness.Interface.FarmManagement;
using BugBusiness.Interface.FarmManagement.DTO;
using BugBusiness.Interface.FarmManagement.Exceptions;
using BugWeb.Models;
using DataAccess.Interface.Domain;


namespace BugWeb.Controllers
{
    public class FarmManagementController : Controller
    {
        private readonly IFarmManagement _farmManagement;


        public FarmManagementController(IFarmManagement farmManagement)
        {
            _farmManagement = farmManagement;
        }

        // GET: FarmManagement
        public ActionResult Index()
        {
            User user = (User)Session["UserInfo"];

            //GetBlocksByFarmRequest getblocksbyfarmRequest = new GetBlocksByFarmRequest()
            //{
            //    FarmID = user.FarmID
            //};
            try
            {
               // GetBlocksByFarmResult getblocksbyfarmResult = _farmManagement.GetBlocksByFarm(getblocksbyfarmRequest);
                return View("~/Views/FarmManagement/BlockEdit.cshtml", user.Farms);
            }
            catch (InvalidInputException)
            {
                return RedirectToAction("index", "home");
            }
            catch (NoBlocksException)
            {
                return RedirectToAction("login", "home");
            }
        }

        // GET: FarmManagement/Details/5
        public ActionResult Details(long id)
        {
            return View();
        }

        // GET: FarmManagement/Create
        public ActionResult Create(long id)
        {
            TempData["CreateID"] = id;
            return View("~/Views/FarmManagement/CreateBlock.cshtml");
        }

        // POST: FarmManagement/Create
        [HttpPost]
        public ActionResult Create(BlockViewModel blockViewModel)
        {
                User user = (User)Session["UserInfo"];
                long farmID=(long)TempData["CreateID"];
                AddBlockRequest addblockRequest = new AddBlockRequest()
                {
                    FarmID = farmID,
                    BlockName = blockViewModel.BlockName
                };

                try
                {
                    AddBlockResult addblockResponse = _farmManagement.AddBlock(addblockRequest);
                    user.Farms.RemoveAll(farm => farm.FarmID.Equals(farmID));
                    Farm updatedFarm=_farmManagement.GetFarmByID(new GetFarmByIDRequest(){FarmID=farmID}).Farm;
                    user.Farms.Add(updatedFarm);
                    return RedirectToAction("index", "farmmanagement");
                }
                catch (InvalidInputException)
                {
                    return RedirectToAction("index", "home");
                }
                catch (BlockExistsException)
                {
                    return RedirectToAction("login", "authentication");
                }
        }

        // GET: FarmManagement/Edit/5
        public ActionResult Edit(long id,long farmID)
        {
            TempData["FarmID"] = farmID;
            GetBlockByIDRequest getblockbyidRequest=new GetBlockByIDRequest()
            {
                BlockID=id
            };

            try
            {
                GetBlockByIDResult getblockbyidResult=_farmManagement.GetBlockByID(getblockbyidRequest);
                return View("~/Views/FarmManagement/UpdateBlock.cshtml", getblockbyidResult.Block);
            }
            catch(InvalidInputException)
            {
                return RedirectToAction("index","home");
            }
            catch(NoSuchBlockExistsException){
                return RedirectToAction("login","authentication");
            }
        }

        // POST: FarmManagement/Edit/5
        [HttpPost]
        public ActionResult Edit(BlockViewModel blockViewModel)
        {
            User user = (User)Session["UserInfo"];
            long farmID=(long)TempData["FarmID"];
            UpdateBlockByIDRequest updateblockbyidRequest = new UpdateBlockByIDRequest()
            {
                BlockID = blockViewModel.BlockID,
                BlockName = blockViewModel.BlockName
            };
            try
            {
                UpdateBlockByIDResult updateblockbyidResult=_farmManagement.UpdateBlockByID(updateblockbyidRequest);
                user.Farms.RemoveAll(farm => farm.FarmID.Equals(farmID));
                Farm updatedFarm = _farmManagement.GetFarmByID(new GetFarmByIDRequest() { FarmID = farmID }).Farm;
                user.Farms.Add(updatedFarm);
                return RedirectToAction("index", "farmmanagement");
            }
            catch (InvalidInputException)
            {
                return RedirectToAction("index", "home");
            }
            catch (CouldNotUpdateException)
            {
                return RedirectToAction("login", "authentication");
            }
        }

        // GET: FarmManagement/Delete/5
        public ActionResult Delete(long id,long farmID)
        {
            TempData["FarmID"] = farmID;
            GetBlockByIDRequest getblockbyidRequest = new GetBlockByIDRequest()
            {
                BlockID = id
            };

            try
            {
                GetBlockByIDResult getblockbyidResult = _farmManagement.GetBlockByID(getblockbyidRequest);
                return View("~/Views/FarmManagement/DeleteBlock.cshtml", getblockbyidResult.Block);
            }
            catch (InvalidInputException)
            {
                return RedirectToAction("index", "home");
            }
            catch (NoSuchBlockExistsException)
            {
                return RedirectToAction("login", "authentication");
            }
        }

        // POST: FarmManagement/Delete/5
        [HttpPost]
        public ActionResult Delete(BlockViewModel blockViewModel)
        {
            User user = (User)Session["UserInfo"];
            long farmID = (long)TempData["FarmID"];
            DeleteBlockByIDRequest deleteblockbyidRequest = new DeleteBlockByIDRequest()
            {
                BlockID=blockViewModel.BlockID
            };

            try
            {
                DeleteBlockByIDResult deleteblockbyidResult = _farmManagement.DeleteBlockByID(deleteblockbyidRequest);
                user.Farms.RemoveAll(farm => farm.FarmID.Equals(farmID));
                Farm updatedFarm = _farmManagement.GetFarmByID(new GetFarmByIDRequest() { FarmID = farmID }).Farm;
                user.Farms.Add(updatedFarm);
                return RedirectToAction("index", "farmmanagement");
            }
            catch (InvalidInputException)
            {
                return RedirectToAction("index", "home");
            }
            catch (CouldNotDeleteBlockException)
            {
                return RedirectToAction("login", "authentication");
            }
        }
    }
}
