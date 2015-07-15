using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BugWeb.Controllers;
using BugWeb.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugWeb.Tests.Controllers
{
    [TestClass]
    public class AuthenticationControllerTest
    {
        [TestMethod] 
        public void WebLoginTest_ShouldBeAuthenticated()
        {
            // Arrange
            var controller = new AuthenticationController();
            var loginViewModel = new LoginViewModel()
            {
                Username = "michelle@gmail.com",
                Password = "321"
            };

            // Act
            var result = controller.Login(loginViewModel) as ViewResult;
            
        }
    }
}
