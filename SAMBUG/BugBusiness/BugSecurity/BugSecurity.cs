using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugBusiness.Interface.BugSecurity;
using DataAccess.Interface.DTOModels;

namespace BugBusiness.BugSecurity
{
    public class BugSecurity : IBugSecurity
    {


        public LoginResponse Login(LoginRequest loginRequest)
        {
            throw new NotImplementedException();
        }

        public RegisterResponse Register(RegisterRequest registerRequest)
        {
            throw new NotImplementedException();
        }

        public RecoverAccountResponse RecoverAccount(RecoverAccountRequest recoverAccountRequest)
        {
            throw new NotImplementedException();
        }
    }
}
