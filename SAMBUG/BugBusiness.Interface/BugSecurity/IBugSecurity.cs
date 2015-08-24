using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugBusiness.Interface.BugSecurity.DTO;

namespace BugBusiness.Interface.BugSecurity
{
    public interface IBugSecurity
    {
        LoginResponse Login(LoginRequest loginRequest);
        RegisterResponse Register(RegisterRequest registerRequest);
        RecoverAccountResponse RecoverAccount(RecoverAccountRequest recoverAccountRequest);
        ViewEditUserRolesResponse GetUsers();
        void EditUserRoles(EditUserRoleRequest editUserRoleRequest);

    }
}
