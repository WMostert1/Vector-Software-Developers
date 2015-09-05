using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugWeb.Models;
using BugBusiness.Interface.BugSecurity.DTO;

namespace BugWeb.Controllers
{
    public class ReportingController : Controller
    {
        // GET: reporting
        public ActionResult Index()
        {
            return View();
        }

        // GET: reporting/tabular
        public ActionResult Tabular()
        {
            UserDTO usr = (UserDTO)Session["UserInfo"];
            
            ReportingViewModel report = new ReportingViewModel();
            report.ActiveFarmId = (long)Session["ActiveFarm"];
            //TODO: Using first farm as active farm (In future, dont rely on Session)
            report.Farm = AutoMapper.Mapper.Map<ReportingViewModel.FarmViewModel>(usr.Farms.ElementAt(0));

            return View(report);
        }

        // GET: reporting/charts
        public ActionResult Charts()
        {
            return View();
        }

        public ActionResult Map()
        {
            return View();
        }
    }
}