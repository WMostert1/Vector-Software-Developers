using System.Collections.Generic;

namespace DataAccess.Interface
{
    public interface IDbBugSecurity
    {
        Models.User GetUserByCredentials(string username, string password);
        bool InsertNewUser(string username, string password);
        ICollection<Models.User> GetAllUsers();
        bool EditUserRoles(long userId, bool isAdministrator);
        bool ChangeUserPassword(string username, string password);
    }
}
