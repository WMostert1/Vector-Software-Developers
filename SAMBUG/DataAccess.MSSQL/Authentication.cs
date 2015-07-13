using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            var context = new BugDBEntities();

            var result = (from user in context.Users
                where user.Email.Equals(loginRequest.Username) && user.Password.Equals(loginRequest.Password)
                select user)
                .FirstOrDefault();

            if (result == null || !loginRequest.Password.Equals(result.Password))
                return null;

            var loginResponse = new LoginResponse
            {
                Id = result.UserID,
                Role = result.RoleID
            };

            return loginResponse;
        }
    }
}
