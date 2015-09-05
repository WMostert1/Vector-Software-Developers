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
using DataAccess.Models;


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

            UserDTO userDTO = AutoMapper.Mapper.Map<UserDTO>(user);

            var loginResponse = new LoginResponse()
            {
                User = userDTO
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

            List<UserDTO> userDTOList = AutoMapper.Mapper.Map<List<UserDTO>>(users);

            return new ViewEditUserRolesResponse { Users = userDTOList };
        }

        public void EditUserRoles(EditUserRoleRequest editUserRoleRequest)
        {

            _dbBugSecurity.EditUserRoles(editUserRoleRequest.UserId, editUserRoleRequest.IsGrower,
                editUserRoleRequest.IsAdministrator);

        }

        public bool ChangeUserPassword(string username1, string password1){
            return _dbBugSecurity.ChangeUserPassword(username1, password1); 
        }
    }
}
