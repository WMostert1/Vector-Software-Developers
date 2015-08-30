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
            report.Farms = new List<ReportingViewModel.FarmViewModel>();


            report.ActiveFarmId = (long)Session["ActiveFarm"];

            User usr = (User)Session["UserInfo"];
            report.UserId = usr.UserId;


            foreach (var frm in usr.Farms)
            {
                ReportingViewModel.FarmViewModel farmModel = new ReportingViewModel.FarmViewModel();
                farmModel.Blocks = new List<ReportingViewModel.BlockViewModel>();
                farmModel.FarmName = frm.FarmName;


                foreach (var block in frm.Blocks)
                {
                    ReportingViewModel.BlockViewModel blockModel = new ReportingViewModel.BlockViewModel()
                    {
                        BlockName = block.BlockName
                    };

                    farmModel.Blocks.Add(blockModel);
                }

                report.Farms.Add(farmModel);
            }

            return View(report);
        }

        // GET: reporting/graphical
        public ActionResult Charts()
        {
            return View();
        }
    }
}