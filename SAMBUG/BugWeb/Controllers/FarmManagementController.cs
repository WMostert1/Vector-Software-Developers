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

        // GET: FarmManagement/Details/5
        public ActionResult Details(long id)
        {
            return View();
        }

        // POST: FarmManagement/Create
        [HttpPost]
        public ActionResult AddBlock(BlockViewModel blockViewModel)
        {
                User user = (User)Session["UserInfo"];
                AddBlockRequest addblockRequest = new AddBlockRequest()
                {
                    FarmID=blockViewModel.FarmID,
                    BlockName = blockViewModel.BlockName
                };

                try
                {
                    AddBlockResult addblockResult = _farmManagement.AddBlock(addblockRequest);
                    user.Farms.RemoveAll(farm => farm.FarmID.Equals(addblockRequest.FarmID));
                    Farm updatedFarm=_farmManagement.GetFarmByID(new GetFarmByIDRequest(){FarmID=addblockRequest.FarmID}).Farm;
                    user.Farms.Add(updatedFarm);
                    return RedirectToAction("EditBlocks", "FarmManagement");
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
        [HttpGet]
        public ActionResult EditBlocks()
        {
            //check not logged in
            if (Session.Count == 0)
                return View("~/Views/Authentication/Login.cshtml");
            User user = (User)Session["UserInfo"];
                // GetBlocksByFarmResult getblocksbyfarmResult = _farmManagement.GetBlocksByFarm(getblocksbyfarmRequest);
            return View(user.Farms);
        }

        // POST: FarmManagement/Edit/5
        [HttpPost]
        public ActionResult EditBlock(BlockViewModel blockViewModel)
        {
            User user = (User)Session["UserInfo"];
            UpdateBlockByIDRequest updateblockbyidRequest = new UpdateBlockByIDRequest()
            {
                BlockID = blockViewModel.BlockID,
                BlockName = blockViewModel.BlockName
            };
            try
            {
                UpdateBlockByIDResult updateblockbyidResult=_farmManagement.UpdateBlockByID(updateblockbyidRequest);
                user.Farms.RemoveAll(farm => farm.FarmID.Equals(updateblockbyidResult.FarmID));
                Farm updatedFarm = _farmManagement.GetFarmByID(new GetFarmByIDRequest() { FarmID = updateblockbyidResult.FarmID }).Farm;
                user.Farms.Add(updatedFarm);
                return RedirectToAction("EditBlocks", "FarmManagement");
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

        // POST: FarmManagement/Delete/5
        [HttpPost]
        public ActionResult DeleteBlock(BlockViewModel blockViewModel)
        {
            User user = (User)Session["UserInfo"];
            DeleteBlockByIDRequest deleteblockbyidRequest = new DeleteBlockByIDRequest()
            {
                BlockID=blockViewModel.BlockID
            };

            try
            {
                DeleteBlockByIDResult deleteblockbyidResult = _farmManagement.DeleteBlockByID(deleteblockbyidRequest);
                user.Farms.RemoveAll(farm => farm.FarmID.Equals(blockViewModel.FarmID));
                Farm updatedFarm = _farmManagement.GetFarmByID(new GetFarmByIDRequest() { FarmID = blockViewModel.FarmID }).Farm;
                user.Farms.Add(updatedFarm);
                return RedirectToAction("EditBlocks", "FarmManagement");
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
