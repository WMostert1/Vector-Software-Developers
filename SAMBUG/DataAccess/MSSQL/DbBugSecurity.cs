using System.Collections.Generic;
using System.Linq;
using DataAccess.Interface;
using DataAccess.Models;
using System;
using System.Data.Entity;

namespace DataAccess.MSSQL
{
    //TODO: possible use of AutoMapper in the future
    public class DbBugSecurity : IDbBugSecurity
    {
        public User GetUserByCredentials(string username, string password)
        {
            var db = new BugDBEntities();

            var user = db.Users.Include(usr => usr.Roles).Include(usr => usr.Farms.Select(frm => frm.Blocks)).SingleOrDefault(usr => usr.Email.Equals(username) && usr.Password.Equals(password, StringComparison.InvariantCulture));

            if (user == default(User))
            {
                return null;
            }

            return user;

        }

        //TODO: Remove magic number => 1
        public bool InsertNewUser(string username, string password)
        {
            var db = new BugDBEntities();

            var userQuery = db.Users.SingleOrDefault(usr => usr.Email.Equals(username));

            if (userQuery != default(User))
            {
                return false;
            }

            Role role = db.Roles.SingleOrDefault(rle => rle.RoleType == 1);

            var user = new User()
            {
                Email = username,
                Password = password
            };

            user.Roles.Add(role);
            db.Users.Add(user);
            db.SaveChanges();

            return true;
        }

        public ICollection<User> GetAllUsers()
        {
            var db = new BugDBEntities();
            var db_users = db.Users.ToList();

            //todo do not manually map here
            return (from user in db_users
                    let roles = user.Roles.Select(role => new Role()
                    {
                        RoleType = role.RoleType,
                
                        RoleDescription = role.RoleDescription,
                        RoleID = role.RoleID
                    }).ToList()
                    select new User()
                    {
                        Email = user.Email,
                        Roles = roles,
                        UserID = user.UserID
                    }).ToList();

        }

        public bool EditUserRoles(long userId, bool isAdministrator)
        {
            //TODO: Remove magic numbers and allow for more roles
            var db = new BugDBEntities();
            Role admin = db.Roles.SingleOrDefault(rle => rle.RoleType == 2);

            User user = db.Users.SingleOrDefault(usr => usr.UserID == userId);

            if (user == null) return false;

            if (isAdministrator && !user.Roles.Contains(admin))
            {
                user.Roles.Add(admin);
            }
            else if (!isAdministrator && user.Roles.Contains(admin))
            {
                user.Roles.Remove(admin);
            }
            db.SaveChanges();

            return true;
        }


        public bool ChangeUserPassword(string username, string password)
        {
            var db = new BugDBEntities();
            try
            {
            User user = db.Users.SingleOrDefault(usr => usr.Email == username);
            user.Password = password;
            }
            catch (Exception)
            {
                return false;
            }
            db.SaveChanges();
            return true;
        }

        public string GetPassword(string username)
        {
            var db = new BugDBEntities();
            String password;
            try
            {
                User user = db.Users.SingleOrDefault(usr => usr.Email == username);
                password = user.Password;
            }
            catch (Exception)
            {
                return "";
            }
            return password;
        }

        public bool RegisterDevice(long id, string token)
        {
            var db = new BugDBEntities();
            DevicePushNotification device = new DevicePushNotification()
            {
                UserID = id,
                RegID = token
            };
            db.DevicePushNotifications.Add(device);
            db.SaveChanges();
            return true;
        }

        public List<Models.DevicePushNotification> GetUserDevices(long id)
        {
            var db = new BugDBEntities();
            return db.DevicePushNotifications.Where(dev => dev.UserID == id).ToList();
        }

        public User GetUserByID(long id)
        {
            var db = new BugDBEntities();
            var user = db.Users.Include(usr => usr.Roles).Include(usr => usr.Farms.Select(frm => frm.Blocks)).SingleOrDefault(usr => usr.UserID.Equals(id));
            return user;
        }
    }
}
