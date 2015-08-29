using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using BugBusiness.Interface.BugSecurity;
using BugBusiness.Interface.BugSecurity.DTO;
using BugBusiness.Interface.BugSecurity.Exceptions;
using DataAccess.Interface;
using DataAccess.Interface.Domain;

namespace BugBusiness.BugSecurity
{

    public class BugSecurity : IBugSecurity
    {

        private readonly IDbBugSecurity _dbBugSecurity;

        public BugSecurity(IDbBugSecurity dbBugSecurity)
        {
            _dbBugSecurity = dbBugSecurity;
        }

        public LoginResponse Login(LoginRequest loginRequest)
        {
            User user = _dbBugSecurity.GetUserByCredentials(loginRequest.Username, loginRequest.Password);

            if (user == null)
                throw new NotRegisteredException();
            
            var loginResponse = new LoginResponse()
            {
                User = user
            };
        
            return loginResponse;
        }

        //TODO: Farmer allowed to have multiple accounts? Implemented now to not allow it 
        //TODO: Check that System.Net.Mail.MailAddress allows reasonable and also enough emails - otherwise do own regex
        //TODO: Eventually do user email confirmation after registering
        public RegisterResponse Register(RegisterRequest registerRequest)
        {
            
            if (!registerRequest.Username.Equals(registerRequest.UsernameConfirmation) ||
                !registerRequest.Password.Equals(registerRequest.PasswordConfirmation))
            {
                throw new InvalidInputException();
            }

            try
            {
                new MailAddress(registerRequest.Username);
            }
            catch (Exception)
            {
                throw new InvalidInputException();
            }

            bool queryResult = _dbBugSecurity.InsertNewUser(registerRequest.Username, registerRequest.Password, registerRequest.FarmName);

            if (queryResult == false)
            {
                throw new UserExistsException();
            }

            return new RegisterResponse() {};

        }

        public RecoverAccountResponse RecoverAccount(RecoverAccountRequest recoverAccountRequest)
        {
            throw new NotImplementedException();
        }

        public ViewEditUserRolesResponse GetUsers()
        {
            var users = _dbBugSecurity.GetAllUsers();
            return new ViewEditUserRolesResponse { Users = users };
        }

        public void EditUserRoles(EditUserRoleRequest editUserRoleRequest)
        {

            _dbBugSecurity.EditUserRoles(editUserRoleRequest.UserId, editUserRoleRequest.IsGrower,
                editUserRoleRequest.IsAdministrator);

        }

        public bool ChangeUserPassword(string username1, string password1){
            _dbBugSecurity.ChangeUserPassword(username1, password1); // ChangeUserPassword(username, password);
            return true;
        }
    }
}
