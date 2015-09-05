using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugBusiness.Interface.FarmManagement.DTO;

namespace BugWeb.Models
{
    public class TreatmentViewModel
    {
        public double PestsPerTree { get; set; }
        public string LastTreatment { get; set; }
    }
}