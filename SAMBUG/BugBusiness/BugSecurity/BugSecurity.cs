using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugBusiness.Interface.BugSecurity;
using DataAccess.Interface;
using DataAccess.Interface.DTOModels;
using DataAccess.Interface.Domain;

namespace BugBusiness.BugSecurity
{
    public class BugSecurity : IBugSecurity
    {

        private readonly IDbAuthentication _dbAuthentication;

        public BugSecurity(IDbAuthentication dbAuthentication)
        {
            _dbAuthentication = dbAuthentication;
        }

        public LoginResponse Login(LoginRequest loginRequest)
        {
            User user = _dbAuthentication.GetUserByCredentials(loginRequest.Username, loginRequest.Password);
            return null;
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
