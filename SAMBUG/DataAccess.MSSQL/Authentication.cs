using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;
using DataAccess.Interface.DTOModels;

namespace DataAccess.MSSQL
{
    public class Authentication : IAuthentication
    {
        public LoginResponse Login(LoginRequest loginRequest)
        {
            return null;
        }
    }
}
