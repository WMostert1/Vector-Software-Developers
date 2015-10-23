using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugBusiness.Interface.FarmManagement;
using BugBusiness.Interface.FarmManagement.DTO;
using BugBusiness.Interface.FarmManagement.Exceptions;
using BugWeb.Models;
using BugWeb.Security;
using BugBusiness.Interface.BugSecurity.DTO;


namespace BugWeb.Controllers
{
    public class FarmManagementController : Controller
    {
        private readonly IFarmManagement _farmManagement;

        public FarmManagementController(IFarmManagement farmManagement)
        {
            _farmManagement = farmManagement;
        }

        [Authenticate(Roles = "1", Alternate = false)]
        public ActionResult SprayData()
        {
            return View();
        }

        [Authenticate(Roles = "1", Alternate = false)]
        public ActionResult EditFarms()
        {
            return View();
        }

       /* // POST: FarmManagement/Create
        [HttpPost]
        public ActionResult AddBlock(BlockViewModel blockViewModel)
        {
            if (!SecurityProvider.isGrower(Session))
                return View("~/Views/Shared/Error.cshtml");
             UserDTO user = (UserDTO)Session["UserInfo"];
            AddBlockRequest addblockRequest = new AddBlockRequest()
            {
                FarmID=blockViewModel.FarmID,
                BlockName = blockViewModel.BlockName
            };

            try
            {
                AddBlockResult addblockResult = _farmManagement.AddBlock(addblockRequest);
                user.Farms.RemoveAll(farm => farm.FarmID.Equals(addblockRequest.FarmID));
                FarmDTO updatedFarm=_farmManagement.GetFarmByID(new GetFarmByIDRequest(){FarmID=addblockRequest.FarmID}).Farm;
                user.Farms.Add(updatedFarm);
                return RedirectToAction("EditFarms","FarmManagement");
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

        // POST: FarmManagement/EditFarms/AddFarm
        [HttpPost]
        public ActionResult AddFarm(FarmViewModel farmviewModel)
        {
            if (!SecurityProvider.isGrower(Session))
                return View("~/Views/Shared/Error.cshtml");
            UserDTO user = (UserDTO)Session["UserInfo"];
            AddFarmRequest addfarmRequest = new AddFarmRequest()
            {
                UserID =  user.UserID,
                FarmName = farmviewModel.FarmName
            };

            try
            {
                AddFarmResult addfarmResult = _farmManagement.AddFarm(addfarmRequest);
                FarmDTO farm = _farmManagement.GetFarmByID(new GetFarmByIDRequest() { FarmID = addfarmResult.FarmID }).Farm;
                user.Farms.Add(farm);
                
                return RedirectToAction("EditFarms", "FarmManagement");
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
            if (!SecurityProvider.isGrower(Session))
                return View("~/Views/Shared/Error.cshtml");
            UserDTO user = (UserDTO)Session["UserInfo"];
            return View(user.Farms);
        }

        [HttpGet]
        public ActionResult EditFarms()
        {
            if (!SecurityProvider.isGrower(Session))
                return View("~/Views/Shared/Error.cshtml");
            UserDTO user = (UserDTO)Session["UserInfo"];
            return View(user.Farms);
        }

        // POST: FarmManagement/Edit/5
        [HttpPost]
        public ActionResult EditBlock(BlockViewModel blockViewModel)
        {
            if (!SecurityProvider.isGrower(Session))
                return View("~/Views/Shared/Error.cshtml");
            UserDTO user = (UserDTO)Session["UserInfo"];
            UpdateBlockByIDRequest updateblockbyidRequest = new UpdateBlockByIDRequest()
            {
                BlockID = blockViewModel.BlockID,
                BlockName = blockViewModel.BlockName
            };
            try
            {
                UpdateBlockByIDResult updateblockbyidResult = _farmManagement.UpdateBlockByID(updateblockbyidRequest);
                user.Farms.RemoveAll(farm => farm.FarmID.Equals(updateblockbyidResult.FarmID));
                FarmDTO updatedFarm = _farmManagement.GetFarmByID(new GetFarmByIDRequest() { FarmID = updateblockbyidResult.FarmID }).Farm;
                user.Farms.Add(updatedFarm);
                return RedirectToAction("EditFarms", "FarmManagement");
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

        // POST: FarmManagement/EditFarms/DeleteFarm
        [HttpPost]
        public ActionResult DeleteFarm(FarmViewModel farmViewModel)
        {
            if (!SecurityProvider.isGrower(Session))
                return View("~/Views/Shared/Error.cshtml");
            UserDTO user = (UserDTO)Session["UserInfo"];
            DeleteFarmByIDRequest deletefarmbyidRequest = new DeleteFarmByIDRequest()
            {
                FarmID = farmViewModel.FarmID
            };

            try
            {
                DeleteFarmByIDResult deletefarmbyidResult = _farmManagement.DeleteFarmByID(deletefarmbyidRequest);
                user.Farms.RemoveAll(farm => farm.FarmID.Equals(farmViewModel.FarmID));
                return RedirectToAction("EditFarms", "FarmManagement");
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

        // POST: FarmManagement/Delete/5
        [HttpPost]
        public ActionResult DeleteBlock(BlockViewModel blockViewModel)
        {
            if (!SecurityProvider.isGrower(Session))
                return View("~/Views/Shared/Error.cshtml");
            UserDTO user = (UserDTO)Session["UserInfo"];
            DeleteBlockByIDRequest deleteblockbyidRequest = new DeleteBlockByIDRequest()
            {
                BlockID=blockViewModel.BlockID
            };

            try
            {
                DeleteBlockByIDResult deleteblockbyidResult = _farmManagement.DeleteBlockByID(deleteblockbyidRequest);
                user.Farms.RemoveAll(farm => farm.FarmID.Equals(blockViewModel.FarmID));
                FarmDTO updatedFarm = _farmManagement.GetFarmByID(new GetFarmByIDRequest() { FarmID = blockViewModel.FarmID }).Farm;
                user.Farms.Add(updatedFarm);
                return RedirectToAction("EditFarms", "FarmManagement");
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

        public ActionResult Treatments()
        {
            if (!SecurityProvider.isGrower(Session))
                return View("~/Views/Shared/Error.cshtml");
            UserDTO user = (UserDTO)Session["UserInfo"];
            var model = new List<List<TreatmentViewModel>>();

            try
            {
                foreach (FarmDTO farm in user.Farms)
                {
                    List<TreatmentViewModel> sublist=new List<TreatmentViewModel>();
                    foreach (BlockDTO block in farm.Blocks)
                    {
                        GetPestsPerTreeByBlockRequest getpestspertreebyblockRequest = new GetPestsPerTreeByBlockRequest()
                        {
                            BlockID = block.BlockID
                        };
                        GetPestsPerTreeByBlockResult getpestspertreebyblockResult = _farmManagement.GetPestsPerTreeByBlock(getpestspertreebyblockRequest);
                        sublist.Add(new TreatmentViewModel() { PestsPerTree = getpestspertreebyblockResult.PestsPerTree, LastTreatment=getpestspertreebyblockResult.LastTreatment });
                    }
                    model.Add(sublist);
                }
            }
            catch (InvalidInputException)
            {
                return RedirectToAction("Error", "Home");
            }
            catch (NoSuchFarmExistsException)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddTreatment(int blockID,DateTime treatmentDate,string treatmentComments)
        {
            if (!SecurityProvider.isGrower(Session))
                return View("~/Views/Shared/Error.cshtml");

            AddTreatmentRequest addtreatmentrequest = new AddTreatmentRequest()
            {
                BlockID = blockID,
                TreatmentDate = treatmentDate,
                TreatmentComments = treatmentComments
            };

            try
            {
                AddTreatmentResult addtreatmentResult = _farmManagement.AddTreatment(addtreatmentrequest);
                return RedirectToAction("Treatments", "FarmManagement");
            }
            catch (InvalidInputException)
            {
                return RedirectToAction("Error", "Home");
            }
        }*/
    }
}
