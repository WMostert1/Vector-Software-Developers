using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

            var userIdQuery =  (from user in context.Users
                where user.Email.Equals(loginRequest.Username) && user.Password.Equals(loginRequest.Password)
                select user.UserID)
                .FirstOrDefault();

            if (userIdQuery == default(int))
                return null;

            var rolesQuery = from usrRole in context.UserRoles
                join rle in context.Roles on usrRole.RoleID equals rle.RoleID 
                          where usrRole.UserID == userIdQuery
                select rle;

            
                


/*            var loginResponse = new LoginResponse
            {
                Id = result.UserID,
                Role = result.RoleID
            }*/;

            return null;
        }
    }
}
