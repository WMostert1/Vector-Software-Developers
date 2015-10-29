using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;
using BugCentral.HelperClass;
using BugBusiness.Interface.BugSecurity;
using BugBusiness.Interface.BugAuthentication;
using BugBusiness.Interface.BugAuthentication.DTO;
using BugBusiness.Interface.BugAuthentication.Exceptions;
using System.Net.Mail;

namespace BugBusiness.BugAuthentication
{
    public class BugAuthentication : IBugAuthentication
    {


        private readonly IBugSecurity _bugSecurity;


        public BugAuthentication(IBugSecurity bugSecurity)
        {
            _bugSecurity = bugSecurity;
            

        }

        public ChangePasswordResult ChangePassword(ChangePasswordRequest changePasswordRequest)
        {

            if (_bugSecurity.ChangeUserPassword(changePasswordRequest.Email, changePasswordRequest.Password) == false)
            {
                throw new FailedChangePasswordException();
            }
                

            return new ChangePasswordResult();

        }

    }
}
