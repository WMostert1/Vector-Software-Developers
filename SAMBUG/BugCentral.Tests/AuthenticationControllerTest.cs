using System;
using System.Web.Http;
using System.Web.UI.WebControls;
using BugCentral.Controllers;
using DataAccess.Interface.DTOModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace BugCentral.Tests
{
    [TestClass]
    public class AuthenticationControllerTest
    {
        [TestMethod]
        public void TestLogin()
        {
            var loginRequest = new LoginRequest()
            {
                Username = "michelle@gmail.com",
                Password = "321"
            };

            var controller = new AuthenticationController();
            

            var response = controller.Login(loginRequest);


            response.Id.ShouldEqual(1);
            response.Roles[0].Id.ShouldEqual(0);

        }
    }
}
