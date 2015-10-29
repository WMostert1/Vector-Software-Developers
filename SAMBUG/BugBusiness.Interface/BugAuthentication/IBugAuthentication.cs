using BugBusiness.Interface.BugAuthentication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBusiness.Interface.BugAuthentication
{
    public interface IBugAuthentication
    {
        //RecoverAccountResponse RecoverAccount(RecoverAccountRequest recoverAccountRequest);
        ChangePasswordResult ChangePassword(ChangePasswordRequest changePasswordRequest);
    }
}
