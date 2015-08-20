using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interface.Domain;


namespace DataAccess.Interface
{
    public interface IDbAuthentication
    {
        User GetUserByCredentials(string username, string password);
        bool InsertNewUser(string username, string password, string farmName);
        ICollection<User> GetAllUsers();
        void EditUserRoles(long userId, bool isGrower, bool isAdministrator);
        
    }
}
