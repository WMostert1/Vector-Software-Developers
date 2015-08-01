using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface;

namespace DataAccess.MSSQL
{
    //TODO: possible use of AutoMapper in the future
    public class DbAuthentication : IDbAuthentication
    {
        public DataAccess.Interface.Domain.User GetUserByCredentials(string username, string password)
        {
            var db = new BugDBEntities();
            
            var entityUser = db.Users.SingleOrDefault(usr => usr.Email.Equals(username) && usr.Password.Equals(password));

            if (entityUser == default(User))
            {
                return null;
            }

            //map EF Role to Domain Role
            var roles = entityUser.UserRoles.Select(userRole => new DataAccess.Interface.Domain.Role()
            {
                Type = userRole.Role.RoleType, 
                Description = userRole.Role.RoleDescription
            }).ToList();

            //map EF user to Domain User
            var domainUser = new DataAccess.Interface.Domain.User()
            {
                Id = entityUser.UserID,
                Roles = roles
            };

            return domainUser;
            
        }

        //TODO: Remove magic number => 1
        public bool InsertNewUser(string username, string password, string farmName)
        {
            var userQuery = GetUserByCredentials(username, password);

            if (userQuery != null)
            {
                return false;
            }

            var db = new BugDBEntities();

            Role role = db.Roles.SingleOrDefault(rle => rle.RoleType == 1);

            User user = new User()
            {
                Email = username,
                Password = password,
            };

            UserRole userRole = new UserRole()
            {
                Role = role
            };

            Farm farm = new Farm()
            {
                FarmName = farmName
            };

            user.Farms.Add(farm);
            user.UserRoles.Add(userRole);

            db.Users.Add(user);

            db.SaveChanges();

            return true;
        }
    }
}
