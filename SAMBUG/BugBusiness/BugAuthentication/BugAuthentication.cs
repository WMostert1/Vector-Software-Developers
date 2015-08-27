using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;
using DataAccess.Interface.Domain;
using BugCentral.HelperClass;
namespace BugBusiness.BugAuthentication
{
    public class BugAuthentication
    {
        const string from = "kaleabtessera@gmail.com";
        const string fromPassword = "27ATEHBruKal1129";
        
        public Boolean RecoverAccount(String to){
            EmailSender email = new EmailSender(from, fromPassword, to);
            email.setEmail("Recover Password", "Old/New");
            
            return email.sendEmail();
 

        }
    }
}
