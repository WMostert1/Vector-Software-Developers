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
            ReportingViewModel report = new ReportingViewModel();
            
            report.ActiveFarmId = (long)Session["ActiveFarm"];

            User usr = (User)Session["UserInfo"];

            report.Farms = AutoMapper.Mapper.Map<List<ReportingViewModel.FarmViewModel>>(usr.Farms);

            return View(report);
        }

        // GET: reporting/graphical
        public ActionResult Charts()
        {
            return View();
        }
    }
}