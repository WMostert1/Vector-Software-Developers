using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugWeb.Models
{
    public class RegisterViewModel
    {
        public string Username { get; set; }
        public string UsernameConfirmation { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
        public string FarmName { get; set; }
    }
}