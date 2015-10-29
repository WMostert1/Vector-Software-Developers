﻿using System;
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
using BugCentral.HelperClass;
using BugBusiness.Interface.BugAuthentication.Exceptions;
using BugBusiness.Interface.BugAuthentication.DTO;


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

            return new LoginResponse()
            {
                User = AutoMapper.Mapper.Map<UserDTO>(user)
            };
           
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

            bool queryResult = _dbBugSecurity.InsertNewUser(registerRequest.Username, registerRequest.Password);

            if (queryResult == false)
            {
                throw new UserExistsException();
            }

            return new RegisterResponse() {};

        }

        public RecoverAccountResponse RecoverAccount(RecoverAccountRequest recoverAccountRequest)
        {
              EmailSender _Email = new EmailSender(recoverAccountRequest.EmailTo);
              _Email.setEmail("Recover Password", GetPassword(recoverAccountRequest.EmailTo));


            if (_Email.sendEmail() == false)
            {
                throw new FailedEmailSendException();
            }

            return new RecoverAccountResponse();
            
        
        }

        public GetUsersResponse GetUsers()
        {
            var users = _dbBugSecurity.GetAllUsers();

            List<UserDTO> userDTOList = AutoMapper.Mapper.Map<List<UserDTO>>(users);

            return new GetUsersResponse { Users = userDTOList };
        }

        public EditUserRoleResponse EditUserRoles(EditUserRoleRequest editUserRoleRequest)
        {

           bool result = _dbBugSecurity.EditUserRoles(editUserRoleRequest.UserId, editUserRoleRequest.IsAdministrator);

           return new EditUserRoleResponse()
           {
               Success = result
           };

        }

        public bool ChangeUserPassword(string username1, string password1){
            return _dbBugSecurity.ChangeUserPassword(username1, password1); 
        }

        public string GetPassword(string username1)
        {
            return _dbBugSecurity.GetPassword(username1);
        }
    }
}
