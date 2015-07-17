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
    public class DBAuthentication : IDbAuthentication
    {
        public LoginResponse GetUserIdRoles(LoginRequest loginRequest)
        {
            var context = new BugDBEntities();

            var userIdQuery =  (from user in context.Users
                where user.Email.Equals(loginRequest.Username) && user.Password.Equals(loginRequest.Password)
                select user.UserID)
                .FirstOrDefault();

            if (userIdQuery == default(int))
                return null;

            var rolesQuery = (from usrRole in context.UserRoles
                join role in context.Roles on usrRole.RoleID equals role.RoleID 
                where usrRole.UserID == userIdQuery
                select new RoleDto(){ Id = role.RoleID, Description = role.RoleDescription}).ToList();

            var loginResponse = new LoginResponse
            {
                Id = userIdQuery,
                Roles = rolesQuery
            };

            return loginResponse;
        }
    }
}
