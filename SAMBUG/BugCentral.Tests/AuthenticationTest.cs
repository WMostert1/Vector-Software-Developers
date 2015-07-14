using System;
using System.Web.UI.WebControls;
using BugCentral.Controllers;
using DataAccess.Interface.DTOModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugCentral.Tests
{
    [TestClass]
    public class AuthenticationTest
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
            /*controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();*/

            var response = controller.Login(loginRequest);

        }
    }
}
