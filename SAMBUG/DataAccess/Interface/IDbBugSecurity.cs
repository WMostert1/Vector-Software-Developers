using System.Collections.Generic;

namespace DataAccess.Interface
{
    public interface IDbBugSecurity
    {
        Models.User GetUserByCredentials(string username, string password);
        bool InsertNewUser(string username, string password);
        ICollection<Models.User> GetAllUsers();
        void EditUserRoles(long userId, bool isGrower, bool isAdministrator);
        bool ChangeUserPassword(string username, string password);
    }
}
