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
using BugBusiness.BugAuthentication;
using BugBusiness.Interface.BugSecurity.DTO;
using BugBusiness.Interface.BugSecurity.Exceptions;
using BugWeb.Models;
using DataAccess.Interface.Domain;
using BugWeb.Security;
using BugBusiness.Interface.BugAuthentication;
using BugBusiness.Interface.BugAuthentication.DTO;


namespace BugWeb.Controllers
{
    public class AuthenticationController : Controller
    {

        private readonly IBugSecurity _bugSecurity;
        private readonly IBugAuthentication _bugAuthentication;

        public AuthenticationController(IBugSecurity bugSecurity)
        {
            _bugSecurity = bugSecurity;
            _bugAuthentication = new BugAuthentication(_bugSecurity);
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
                //todo: we should rather store ids in the session rather than the entire objects
                User user = new User()
                {
                    UserId=loginResponse.User.UserId,
                    Farms=loginResponse.User.Farms,
                    Roles=loginResponse.User.Roles
                };

                Session["UserInfo"] = user;
                //todo: implement muliple farm support (for now farm 1 is default)
                Session["ActiveFarm"] = loginResponse.User.Farms[0].FarmID;

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
                    //todo: I don't know where this is supposed to redirect to
                    //return RedirectToAction("index", "farmmanagement");
                    return RedirectToAction("index", "home");
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

        [HttpGet]
        public ActionResult EditUserRoles()
        {
            if (!SecurityProvider.isAdmin(Session))
                return View("~/Views/Shared/Error.cshtml");
            ViewEditUserRolesResponse response = _bugSecurity.GetUsers();
            return View(response);
        }

        [HttpPost]
        public ActionResult EditUserRoles(EditUserRoleViewModel editUserRoleViewModel)
        {
            if (!SecurityProvider.isAdmin(Session))
                return View("~/Views/Shared/Error.cshtml");
            _bugSecurity.EditUserRoles(new EditUserRoleRequest
            {
                UserId = editUserRoleViewModel.UserId,
                IsAdministrator = editUserRoleViewModel.IsAdministrator,
                IsGrower = editUserRoleViewModel.IsGrower
            });

            return RedirectToAction("EditUserRoles","Authentication");
        }

       // [HttpPost]
        public ActionResult RecoverAccount(RecoverAccountModel recoverAccountModel)
        {
            recoverAccountModel.Link = "http://localhost:53249/Home/ChangePassword";
            BugBusiness.Interface.BugAuthentication.DTO.RecoverAccountRequest recoverAccountRequest = new BugBusiness.Interface.BugAuthentication.DTO.RecoverAccountRequest()
            {
                From = "kaleabtessera@gmail.com",
                FromPassword = "27ATEHBruKal1129",
                EmailTo = recoverAccountModel.EmailTo,
                Link = recoverAccountModel.Link
            };

            _bugAuthentication.RecoverAccount(recoverAccountRequest);
            return RedirectToAction("CheckEmail", "Authentication");
            
       }
        //[HttpGet]
        public ActionResult CheckEmail()
        {
            return View("~/Views/Authentication/CheckEmail.cshtml");
        }

        public ActionResult ChangePassword(ChangePasswordModel changePasswordModel)
        {


            ChangePasswordRequest changePasswordRequest = new ChangePasswordRequest()
            {
                Email = changePasswordModel.Email,
                Password = changePasswordModel.Password
            }; 
            //_bugAuthentication.ChangePassword(loginViewModel.Username, loginViewModel.Password);
            _bugAuthentication.ChangePassword(changePasswordRequest);
            return RedirectToAction("login", "home");
        }

    }
}