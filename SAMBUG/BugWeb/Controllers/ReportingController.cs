using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BugWeb.Models;
using DataAccess.Interface.Domain;


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
            var usr = (User)Session["UserInfo"];

            var report = new ReportingViewModel();
            report.ActiveFarmId = (long)Session["ActiveFarm"];
            report.Farm = AutoMapper.Mapper.Map<ReportingViewModel.FarmViewModel>(
                usr.Farms.Single(farm => farm.FarmID.Equals(report.ActiveFarmId)));

            return View(report);
        }

        // GET: reporting/charts
        public ActionResult Charts()
        {
            User usr = (User)Session["UserInfo"];

            var report = new ReportingViewModel();
            //report.ActiveFarmId = (long) Session["ActiveFarm"];
            //report.Farm = AutoMapper.Mapper.Map<ReportingViewModel.FarmViewModel>(
            //   usr.Farms.Single(farm => farm.FarmID.Equals(report.ActiveFarmId)));
            
           return View(report);
        }

        public ActionResult Map()
        {
            return View();
        }
    }
}