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
using BugCentral.HelperClass;
using BugBusiness.Interface.BugAuthentication.Exceptions;
using BugBusiness.Interface.BugAuthentication.DTO;
using System.Net;
using System.IO;
using Newtonsoft.Json;

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

        public RegisterDeviceResponse RegisterDevice(RegisterDeviceRequest request)
        {
            return new RegisterDeviceResponse { Registered = _dbBugSecurity.RegisterDevice(request.UserID,request.DeviceToken) };
        }

        public UpdateUserDeviceResponse UpdateUserDevice(UpdateUserDeviceRequest updateUserDeviceRequest)
        {
            User user = _dbBugSecurity.GetUserByID(updateUserDeviceRequest.UserID);

            if (user == null)
                throw new NotRegisteredException();

            UserDTO userDto = AutoMapper.Mapper.Map<UserDTO>(user);
            string userjson = JsonConvert.SerializeObject(userDto);
            var devices = _dbBugSecurity.GetUserDevices(updateUserDeviceRequest.UserID);
            if (devices.Count > 0)
            {
                string recipients = "[\""+devices.ElementAt(0).RegID+"\"";
                devices.RemoveAt(0);
                foreach (var device in devices)
                {
                    recipients += "\"," + device.RegID + "\"";
                }
                recipients += "]";
                string postData = "{\"registration_ids\":" + recipients + ",\"data\":{\"userInfo\":"+userjson+"}}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://gcm-http.googleapis.com/gcm/send");
                request.ContentType = "application/json";
                request.Method = "POST";
                request.Headers.Add(String.Format("Authorization: {0}", "key=AIzaSyDSZF2futAZDTXVo-mm9LjSjr1FPNtRrjI"));
                request.ContentLength = byteArray.Length;
                Stream contentStream = request.GetRequestStream();
                contentStream.Write(byteArray, 0, byteArray.Length);
                contentStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                int code = (int)response.StatusCode;
                return new UpdateUserDeviceResponse() { Result = code };
            }
            else
            {
                return new UpdateUserDeviceResponse() { Result = -1 };
            }
            
        }
    }
}
