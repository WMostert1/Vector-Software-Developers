using System.Collections.Generic;

namespace DataAccess.Interface
{
    public interface IDbBugSecurity
    {
        Domain.User GetUserByCredentials(string username, string password);
        bool InsertNewUser(string username, string password, string farmName);
        ICollection<Domain.User> GetAllUsers();
        void EditUserRoles(long userId, bool isGrower, bool isAdministrator);
        bool ChangeUserPassword(string username, string password);
    }
}
