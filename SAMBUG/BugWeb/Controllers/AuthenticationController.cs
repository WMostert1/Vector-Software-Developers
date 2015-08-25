using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using BugBusiness.Interface.BugSecurity;
using BugBusiness.Interface.BugSecurity.DTO;
using BugBusiness.Interface.BugSecurity.Exceptions;
using BugWeb.Models;
using Newtonsoft.Json;
using DataAccess.Interface.Domain;

namespace BugWeb.Controllers
{
    public class AuthenticationController : Controller
    {

        private readonly IBugSecurity _bugSecurity;


        public AuthenticationController(IBugSecurity bugSecurity)
        {
            _bugSecurity = bugSecurity;
        }

        public ActionResult Login(LoginViewModel loginViewModel)
        {

            LoginRequest loginRequest = new LoginRequest()
            {
                Username = loginViewModel.Username,
                Password = loginViewModel.Password
            }; 

            try
            {
                LoginResponse loginResponse = _bugSecurity.Login(loginRequest);

                //set up session
                User user = new User()
                {
                    Id=loginResponse.User.Id,
                    Farms=loginResponse.User.Farms,
                    Roles=loginResponse.User.Roles
                };

                Session["UserInfo"] = user;
                //todo: implement muliple farm support
                Session["ActiveFarm"] = loginResponse.User.Farms[0];

                //check to go to home page or farm setup
                int blockCount = 0;
                foreach (Farm f in user.Farms){
                    blockCount += f.Blocks.Count;
                }
                if (blockCount > 0)
                {
                    return RedirectToAction("index", "home");
                }
                else
                {
                    return RedirectToAction("index", "farmmanagement");
                }
            }
            catch (NotRegisteredException)
            {
                return RedirectToAction("login", "home");
            }
        }

        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            RegisterRequest registerRequest = new RegisterRequest()
            {
                Username = registerViewModel.Username,
                UsernameConfirmation = registerViewModel.UsernameConfirmation,
                Password = registerViewModel.Password,
                PasswordConfirmation = registerViewModel.PasswordConfirmation,
                FarmName = registerViewModel.FarmName
            };

            try
            {
                RegisterResponse registerResponse = _bugSecurity.Register(registerRequest);
                return RedirectToAction("login", "home");
            }
            catch (InvalidInputException)
            {
                return RedirectToAction("register", "home");
            }
            catch (UserExistsException)
            {
                return RedirectToAction("register", "home");
            }
        } 
    }
}