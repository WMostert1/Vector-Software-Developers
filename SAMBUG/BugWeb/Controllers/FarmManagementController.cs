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
    }
}
