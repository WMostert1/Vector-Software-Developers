using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;
using DataAccess.Interface.Domain;
using BugCentral.HelperClass;
using BugBusiness.Interface.BugSecurity;
namespace BugBusiness.BugAuthentication
{
    public class BugAuthentication
    {
        const string from = "kaleabtessera@gmail.com";
        const string fromPassword = "27ATEHBruKal1129";

        private readonly IBugSecurity _bugSecurity;

        public BugAuthentication(IBugSecurity bugSecurity)
        {
            _bugSecurity = bugSecurity;
        }


        public Boolean RecoverAccount(String to, String link){
            EmailSender email = new EmailSender(from, fromPassword, to);
            email.setEmail("Recover Password", link);
            
            return email.sendEmail();
        }

        public void ChangePassword(String uName, String uPass)
        {
            _bugSecurity.ChangeUserPassword(uName, uPass);

        }

    }
}
