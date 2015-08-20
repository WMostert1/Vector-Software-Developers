using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugWeb.Models
{
    public class EditUserRoleViewModel
    {
        public int UserId { get; set; }
        public bool IsGrower { get; set; }
        public bool IsAdministrator { get; set; }
    }
}