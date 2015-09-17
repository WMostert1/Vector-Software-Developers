using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using BugBusiness.Interface.BugSecurity.DTO;
using BugWeb.Security;

namespace BugWeb.Models
{
    public class ReportingViewModel
    {
        public ReportingViewModel(HttpSessionStateBase session)
        {
            RecordsUrlSuffix = SecurityProvider.isAdmin(session) ? "" :  ((UserDTO)session["UserInfo"]).UserID.ToString();
            RecordsUrlSuffix = "";
        }

        public string RecordsUrlSuffix { get; private set; }
    }
}