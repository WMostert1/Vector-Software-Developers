using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
        public Interface.Domain.User GetUserByCredentials(string username, string password)
        {
            var db = new BugDBEntities();

            var entityUser = db.Users.SingleOrDefault(usr => usr.Email.Equals(username) && usr.Password.Equals(password));

            if (entityUser == default(User))
            {
                return null;
            }

            //map EF Farm to Domain Farm
            var farms = entityUser.Farms.Select(farm =>
                new Interface.Domain.Farm()
                {
                    FarmID = farm.FarmID,
                    FarmName = farm.FarmName,
                    Blocks = farm.Blocks.Select(block =>
                        new Interface.Domain.Block()
                        {
                            BlockID = block.BlockID,
                            BlockName = block.BlockName
                        }).ToList()
                }).ToList();

            //map EF Role to Domain Role
            var roles = entityUser.Roles.Select(role =>
            new Interface.Domain.Role()
            {
                Type = role.RoleType,
                Description = role.RoleDescription,
                RoleId = role.RoleID
            }).ToList();

            //map EF user to Domain User
            var domainUser = new Interface.Domain.User()
            {
                UserId = entityUser.UserID,
                Roles = roles,
                Farms = farms
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

            var user = new User()
            {
                Email = username,
                Password = password
            };

            user.Roles.Add(role);

            Farm farm = new Farm()
            {
                FarmName = farmName
            };

            user.Farms.Add(farm);
            db.Users.Add(user);
            db.SaveChanges();

            return true;
        }

        public ICollection<Interface.Domain.User> GetAllUsers()
        {
            var db = new BugDBEntities();
            var db_users = db.Users.ToList();


            return (from user in db_users
                    let roles = user.Roles.Select(role => new Interface.Domain.Role()
                    {
                        Type = role.RoleType,
                        Description = role.RoleDescription,
                        RoleId = role.RoleID
                    }).ToList()
                    select new Interface.Domain.User()
                    {
                        Email = user.Email,
                        Roles = roles,
                        UserId = user.UserID
                    }).ToList();

        }

        public void EditUserRoles(long userId, bool isGrower, bool isAdministrator)
        {
            //TODO: Remove magic numbers and allow for more roles
            var db = new BugDBEntities();
            Role grower = db.Roles.SingleOrDefault(rle => rle.RoleType == 1);
            Role admin = db.Roles.SingleOrDefault(rle => rle.RoleType == 2);

            User user = db.Users.SingleOrDefault(usr => usr.UserID == userId);

            if (user == null) return;

            if (isGrower && !user.Roles.Contains(grower))
            {
                user.Roles.Add(grower);
            }
            else if (!isGrower && user.Roles.Contains(grower))
            {
                user.Roles.Remove(grower);
            }


            if (isAdministrator && !user.Roles.Contains(admin))
            {
                user.Roles.Add(admin);
            }
            else if (!isAdministrator && user.Roles.Contains(admin))
            {
                user.Roles.Remove(admin);
            }
            db.SaveChanges();
        }


        public bool ChangeUserPassword(string username, string password)
        {
            var db = new BugDBEntities();
            try { 
            User user = db.Users.SingleOrDefault(usr => usr.Email == username);
            user.Password = password;
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

    }
}
