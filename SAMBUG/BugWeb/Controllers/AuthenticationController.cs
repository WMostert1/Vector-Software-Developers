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
using DataAccess.Interface.Domain;
using Newtonsoft.Json;

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
                return RedirectToAction("index", "home");
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

        [HttpGet]
        public ActionResult EditUserRoles()
        {
            ViewEditUserRolesResponse response = _bugSecurity.GetUsers();
            return View(response);
        }

        [HttpPost]
        public ActionResult EditUserRoles(EditUserRoleViewModel editUserRoleViewModel)
        {
            
            _bugSecurity.EditUserRoles(new EditUserRoleRequest
            {
                UserId = editUserRoleViewModel.UserId,
                IsAdministrator = editUserRoleViewModel.IsAdministrator,
                IsGrower = editUserRoleViewModel.IsGrower
            });

            return RedirectToAction("EditUserRoles","Authentication");
        }
    }
}