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

            GetBlocksByFarmRequest getblocksbyfarmRequest = new GetBlocksByFarmRequest()
            {
                FarmID = 1
            };

            try
            {
                GetBlocksByFarmResult getblocksbyfarmResult = _farmManagement.GetBlocksByFarm(getblocksbyfarmRequest);
                return View("~/Views/FarmManagement/BlockEdit.cshtml", getblocksbyfarmResult.Blocks);
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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FarmManagement/Create
        public ActionResult Create()
        {
            return View("~/Views/FarmManagement/CreateBlock.cshtml");
        }

        // POST: FarmManagement/Create
        [HttpPost]
        public ActionResult Create(BlockViewModel blockViewModel)
        {
                AddBlockRequest addblockRequest = new AddBlockRequest()
                {
                    FarmID = 1,
                    BlockName = blockViewModel.BlockName
                };

                try
                {
                    AddBlockResult addblockResponse = _farmManagement.AddBlock(addblockRequest);
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
        public ActionResult Edit(long id)
        {
            GetBlockByIDRequest getblockbyidRequest=new GetBlockByIDRequest()
            {
                BlockID=id
            };

            try
            {
                GetBlockByIDResult getblockbyidResult=_farmManagement.GetBlockByID(getblockbyidRequest);
                return View("~/Views/FarmManagement/UpdateBlock.cshtml", getblockbyidResult.Block);
            }
            catch(InvalidInputException ex)
            {
                return RedirectToAction("index","home");
            }
            catch(NoSuchBlockExistsException ex){
                return RedirectToAction("login","authentication");
            }
        }

        // POST: FarmManagement/Edit/5
        [HttpPost]
        public ActionResult Edit(BlockViewModel blockViewModel)
        {
            
            UpdateBlockByIDRequest updateblockbyidRequest = new UpdateBlockByIDRequest()
            {
                BlockID = blockViewModel.BlockID,
                BlockName = blockViewModel.BlockName
            };
            try
            {
                UpdateBlockByIDResult updateblockbyidResult=_farmManagement.UpdateBlockByID(updateblockbyidRequest);
                return RedirectToAction("index", "farmmanagement");
            }
            catch (InvalidInputException ex)
            {
                return RedirectToAction("index", "home");
            }
            catch (CouldNotUpdateException ex)
            {
                return RedirectToAction("login", "authentication");
            }
        }

        // GET: FarmManagement/Delete/5
        public ActionResult Delete(int id)
        {
            GetBlockByIDRequest getblockbyidRequest = new GetBlockByIDRequest()
            {
                BlockID = id
            };

            try
            {
                GetBlockByIDResult getblockbyidResult = _farmManagement.GetBlockByID(getblockbyidRequest);
                return View("~/Views/FarmManagement/DeleteBlock.cshtml", getblockbyidResult.Block);
            }
            catch (InvalidInputException ex)
            {
                return RedirectToAction("index", "home");
            }
            catch (NoSuchBlockExistsException ex)
            {
                return RedirectToAction("login", "authentication");
            }
        }

        // POST: FarmManagement/Delete/5
        [HttpPost]
        public ActionResult Delete(BlockViewModel blockViewModel)
        {
            DeleteBlockByIDRequest deleteblockbyidRequest = new DeleteBlockByIDRequest()
            {
                BlockID=blockViewModel.BlockID
            };

            try
            {
                DeleteBlockByIDResult deleteblockbyidResult = _farmManagement.DeleteBlockByID(deleteblockbyidRequest);
                return RedirectToAction("index", "farmmanagement");
            }
            catch (InvalidInputException ex)
            {
                return RedirectToAction("index", "home");
            }
            catch (CouldNotDeleteBlockException ex)
            {
                return RedirectToAction("login", "authentication");
            }
        }
    }
}
