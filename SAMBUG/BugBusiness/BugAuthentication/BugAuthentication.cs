using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;
using DataAccess.Interface.Domain;
using BugCentral.HelperClass;
using BugBusiness.Interface.BugSecurity;
using BugBusiness.Interface.BugAuthentication;
using BugBusiness.Interface.BugAuthentication.DTO;
using BugBusiness.Interface.BugAuthentication.Exceptions;

namespace BugBusiness.BugAuthentication
{
    public class BugAuthentication : IBugAuthentication
    {
        const string from = "kaleabtessera@gmail.com";
        const string fromPassword = "27ATEHBruKal1129";

        private readonly IBugSecurity _bugSecurity;

        public BugAuthentication(IBugSecurity bugSecurity)
        {
            _bugSecurity = bugSecurity;
        }


        public RecoverAccountResult RecoverAccount(RecoverAccountRequest recoverAccountRequest)
        {
            EmailSender email = new EmailSender(from, fromPassword, recoverAccountRequest.EmailTo);
            email.setEmail("Recover Password", recoverAccountRequest.Link);


            if (email.sendEmail() == false)
            {
                throw new FailedEmailSendException();
            }

            return new RecoverAccountResult();
            
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
