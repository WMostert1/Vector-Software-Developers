using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;

namespace DataAccess.MSSQL
{
    public class DbAuthentication : IDbAuthentication
    {
        public DataAccess.Interface.Domain.User GetUserByCredentials(string username, string password)
        {
            var db = new BugDBEntities();
            
            var dataUser = db.Users.First(usr => usr.Email.Equals(username) && usr.Password.Equals(password));

            if (dataUser == null)
            {
                return null;
            }

            var roles = new List<DataAccess.Interface.Domain.Role>();

            foreach (var userRole in dataUser.UserRoles)
            {
                var role = new DataAccess.Interface.Domain.Role()
                {
                    Type = userRole.Role.RoleType,
                    Description = userRole.Role.RoleDescription
                };

                roles.Add(role);
            }

            var user = new DataAccess.Interface.Domain.User()
            {
                Id = dataUser.UserID,
                Roles = roles
            };

            return user;
            
        }
    }
}
